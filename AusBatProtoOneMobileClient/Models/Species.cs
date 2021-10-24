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
            var filename = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_details.html".ToAndroidFilenameFormat();
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.SpeciesDetails.{filename}"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Details file for [{speciesName}] does not exist");
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
        }
        internal async Task LoadImages()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            var toRemoveImages = new List<string>();
            foreach (var imageName in Images)
            {
                if (imageName.Contains("head"))
                {
                    // Check is [SharedImage] exists
                    if (!await ImageChecker.DoesImageExist(imageName))
                    {
                        Debug.WriteLine($"SharedImage [{imageName}] for species [{speciesName}] is missing");
                        toRemoveImages.Add(imageName);
                        continue;
                    }
                }
                #region *// Check if hires image exists
                if (!File.Exists(HiresImages.GetFullFilename(imageName)))
                {
                    Debug.WriteLine($"Hires image [{imageName}] for species [{speciesName}] is missing");
                    toRemoveImages.Add(imageName);
                    continue;
                } 
                #endregion
            }
            toRemoveImages.ForEach(o => Images.Remove(o));

            if (Images.Count == 0)
            {
                Debug.WriteLine($"Missing images exist for species [{speciesName}]");
                Images.Add("bat.png");
            }
        }
        internal void LoadCalls()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            var imageName = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_call_image.jpg".ToAndroidFilenameFormat();

            CallImages.Clear();

            if (File.Exists(HiresImages.GetFullFilename(imageName)))
            {
                CallImages.Add(imageName);
            }
            else
            {
                Debug.WriteLine($"No call images exist for[{speciesName}]");
            }
        }

        internal void LoadDistributionMaps()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            var imageName = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_dist.jpg".ToAndroidFilenameFormat();

            if (!File.Exists(HiresImages.GetFullFilename(DistributionMapImage)))
            {
                Debug.WriteLine($"No distribution map exist for[{speciesName}]");
            }
        }

        internal Classification GetFamily(Dbase dbase)
        {
            var batFamilyId = dbase.Classifications.FirstOrDefault(o => o.Id == GenusId).Parent;
            return dbase.Classifications.FirstOrDefault(o => o.Id == batFamilyId);
        }
    }

}
