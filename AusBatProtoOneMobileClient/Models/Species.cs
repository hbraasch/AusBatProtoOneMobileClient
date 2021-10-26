using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Data
{
    public class Species
    {
        public string GenusId { get; set; }
        public string SpeciesId { get; set; }
        public string Name { get; set; }
        public string DetailsHtml { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> CallImages { get; set; } = new List<string>();
        public List<int> RegionIds { get; set; } = new List<int>();
        public string DistributionMapImage => $"{GenusId}_{SpeciesId}_dist.jpg".ToLower();

        internal void LoadDetails()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            Debug.WriteLine($"Start loading species [{speciesName}] details json file");
            var filename = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_details.html".ToAndroidFilenameFormat();
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.SpeciesDetails.{filename}"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Missing details file for [{speciesName}]");
                        DetailsHtml = @"<p style=""color: white"">No details defined</p>";
                        return;
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string detailsHtml = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(detailsHtml))
                        {
                            throw new BusinessException($"No data inside details file for [{speciesName}]");
                        }
                        DetailsHtml = detailsHtml;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading details file for [speciesName]. {ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"End loading species [{speciesName}] details json file");
            }
        }
        internal async Task LoadImages()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            Debug.WriteLine($"Start loading species [{speciesName}] general images");
            var toRemoveImages = new List<string>();
            foreach (var imageName in Images)
            {
                if (imageName.Contains("head"))
                {
                    // Check is [SharedImage] exists
                    if (!await ImageChecker.DoesImageExist(imageName))
                    {
                        Debug.WriteLine($"Missing SharedImage [{imageName}] for species [{speciesName}]");
                        toRemoveImages.Add(imageName);
                        continue;
                    }
                }
                #region *// Check if hires image exists
                if (!File.Exists(HiresImages.GetFullFilename(imageName)))
                {
                    Debug.WriteLine($"Missing hires image [{imageName}] for species [{speciesName}]");
                    toRemoveImages.Add(imageName);
                    continue;
                } 
                #endregion
            }
            toRemoveImages.ForEach(o => Images.Remove(o));

            if (Images.Count == 0)
            {
                Debug.WriteLine($"Missing images for species [{speciesName}]");
                Images.Add("bat.png");
            }
            Debug.WriteLine($"End loading species [{speciesName}] general images");
        }
        internal void LoadCalls()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            Debug.WriteLine($"Start loading species [{speciesName}] call images");
            var imageName = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_call_image.jpg".ToAndroidFilenameFormat();

            CallImages.Clear();

            if (File.Exists(HiresImages.GetFullFilename(imageName)))
            {
                CallImages.Add(imageName);
            }
            else
            {
                Debug.WriteLine($"Missing call images exist for[{speciesName}]");
            }
            Debug.WriteLine($"End loading species [{speciesName}] call images");
        }

        internal void LoadDistributionMaps()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            Debug.WriteLine($"Start loading species [{speciesName}] distribution map image");
            var imageName = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_dist.jpg".ToAndroidFilenameFormat();

            if (!File.Exists(HiresImages.GetFullFilename(DistributionMapImage)))
            {
                Debug.WriteLine($"Missing distribution map for[{speciesName}]");
            }
            Debug.WriteLine($"End loading species [{speciesName}] distribution map image");
        }

        internal Classification GetFamily(Dbase dbase)
        {
            var batFamilyId = dbase.Classifications.FirstOrDefault(o => o.Id == GenusId).Parent;
            return dbase.Classifications.FirstOrDefault(o => o.Id == batFamilyId);
        }
    }

}
