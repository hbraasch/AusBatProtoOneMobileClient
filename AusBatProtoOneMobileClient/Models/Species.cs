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
        public string DataTag { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> CallImages { get; set; } = new List<string>();
        public List<int> RegionIds { get; set; } = new List<int>();
        public string DistributionMapImage => $"{GenusId}_{SpeciesId}_dist.jpg".ToLower();

        internal void LoadDetails()
        {
            var filename = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}_details.html".ToAndroidFilenameFormat();
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.SpeciesDetails.{filename}"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Details file for [{filename}] does not exist");
                        DetailsHtml = @"<p style=""color: white"">No details defined</p>";
                        return;
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string detailsHtml = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(detailsHtml))
                        {
                            throw new BusinessException($"No data inside details file for [{filename}]");
                        }
                        DetailsHtml = detailsHtml;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading details file for [{DataTag}]. {ex.Message}");
            }
        }
        internal async Task LoadImages()
        {
            if (DataTag == null)
            {
                Debug.WriteLine($"Cannot set image data species[{GenusId} {SpeciesId}]. DataTag value is null");
                return;
            }
            var toRemoveImages = new List<string>();
            foreach (var imageName in Images)
            {
                if (!await ImageChecker.DoesImageExist(imageName))
                {
                    toRemoveImages.Add(imageName);
                }
            }
            toRemoveImages.ForEach(o => Images.Remove(o));

            if (Images.Count == 0)
            {
                Debug.WriteLine($"No images exist for[{DataTag}]");
                Images.Add("bat.png");
            }
        }
        internal async Task LoadCalls()
        {
            CallImages.Clear();
            if (DataTag == null)
            {
                Debug.WriteLine($"Cannot set call data for species[{GenusId} {SpeciesId}]. DataTag value is null");
                return;
            }

            var imageName = $"{DataTag.ToLower()}_call_image.jpg";
            if (await ImageChecker.DoesImageExist(imageName))
            {
                CallImages.Add(imageName);
            }
            else
            {
                Debug.WriteLine($"No call images exist for[{DataTag}]");
            }
        }

        internal Classification GetFamily(Dbase dbase)
        {
            var batFamilyId = dbase.Classifications.FirstOrDefault(o => o.Id == GenusId).Parent;
            return dbase.Classifications.FirstOrDefault(o => o.Id == batFamilyId);
        }
    }

}
