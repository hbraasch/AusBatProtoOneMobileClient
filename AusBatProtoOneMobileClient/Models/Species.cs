#define OnlyReportMissings
// #undef OnlyReportMissings

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
        private bool OnlyReportMissings = false;

        public string GenusId { get; set; }
        public string SpeciesId { get; set; }
        public string Name { get; set; }
        public string DetailsHtml { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> CallImages { get; set; } = new List<string>();
        public List<string> CallAudios { get; set; } = new List<string>();
        public List<int> RegionIds { get; set; } = new List<int>();
        public string DistributionMapImage => $"{GenusId}_{SpeciesId}_dist.jpg".ToLower();

        public List<SimilarSpeciesData> SimilarSpecies = new List<SimilarSpeciesData> { };

        internal void LoadDetails()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            if (!OnlyReportMissings) Debug.WriteLine($"Start loading species [{speciesName}] details json file");
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
                if (!OnlyReportMissings) Debug.WriteLine($"End loading species [{speciesName}] details json file");
            }
        }
        internal async Task LoadImages()
        {
            bool hasHeadFile = false;
            int MAX_FILE_AMOUNT = 5;
            var toRemoveImages = new List<string>();
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            if (!OnlyReportMissings) Debug.WriteLine($"Start loading species [{speciesName}] general images");

            Images.Clear(); 

            #region *// Get all hires image that comply with naming standard
            var headImageName = $"{speciesName}_head.jpg".ToAndroidFilenameFormat();
            if (File.Exists(ZippedFiles.GetFullFilename(headImageName)))
            {
                // Check if [Resizetizer SharedImage] exists
                if (!await ImageChecker.DoesImageExist(headImageName))
                {
                    Debug.WriteLine($"Missing Resizetizer SharedImage head image [{headImageName}] for species [{speciesName}]");
                }
                else
                {
                    Images.Add(headImageName);
                    hasHeadFile = true;
                }
            }
            else
            {
                Debug.WriteLine($"Missing head image for species [{speciesName}]");
            }

            for (int imageNumber = 0; imageNumber < MAX_FILE_AMOUNT; imageNumber++)
            {
                var imageName = GenerateNumberedFilename(speciesName, "", "jpg", imageNumber);
                if (File.Exists(ZippedFiles.GetFullFilename(imageName)))
                {
                    Images.Add(imageName);
                }
            }

            if (Images.Count == 0)
            {
                Debug.WriteLine($"Missing images for species [{speciesName}]");
            }
            if (!hasHeadFile)
            {
                Debug.WriteLine($"Missing head image for species [{speciesName}]");
            }
            #endregion

            if (!OnlyReportMissings) Debug.WriteLine($"End loading species [{speciesName}] general images");
        }
        internal void LoadCalls()
        {
            int MAX_FILE_AMOUNT = 5;
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            if (!OnlyReportMissings) Debug.WriteLine($"Start loading species [{speciesName}] call data");

            CallImages.Clear();
            CallAudios.Clear();

            #region *// Load call images            
            for (int imageNumber = 0; imageNumber < MAX_FILE_AMOUNT; imageNumber++)
            {
                var imageName = GenerateNumberedFilename($"{GenusId.ToLower()}_{SpeciesId.ToLower()}", "_call_image", "jpg", imageNumber);
                if (File.Exists(ZippedFiles.GetFullFilename(imageName)))
                {
                    CallImages.Add(imageName);
                } 
            }
            #endregion

            #region *// Load call audios
            for (int audioNumber = 0; audioNumber < MAX_FILE_AMOUNT; audioNumber++)
            {
                var audioName = GenerateNumberedFilename($"{GenusId.ToLower()}_{SpeciesId.ToLower()}", "_call_audio", "mp3", audioNumber);
                if (File.Exists(ZippedFiles.GetFullFilename(audioName)))
                {
                    CallAudios.Add(audioName);
                }
            }
            #endregion
            if ((CallImages.Count == 0) && (CallAudios.Count == 0))
            {
                Debug.WriteLine($"Missing call image or audio for[{speciesName}]");
            }
            else
            {
                if (!OnlyReportMissings) Debug.WriteLine($"{CallImages.Count} species [{speciesName}] call data files loaded");
                if (!OnlyReportMissings) Debug.WriteLine($"{CallAudios.Count} species [{speciesName}] call audio files loaded");
            }

            if (!OnlyReportMissings) Debug.WriteLine($"End loading species [{speciesName}] call data");
        }

        private string GenerateNumberedFilename(string speciesName, string postFix, string extension, int imageNumber)
        {
            string number = (imageNumber == 0) ? "": imageNumber.ToString("N0");
            return $"{speciesName}{postFix}{number}.{extension}".ToAndroidFilenameFormat();
        }

        public class CallData
        {
            public string ImageName { get; set; }
            public string AudioName { get; set; }
        }

        internal void LoadDistributionMaps()
        {
            var speciesName = $"{GenusId.ToUpperFirstChar()} {SpeciesId}";
            if (!OnlyReportMissings) Debug.WriteLine($"Start loading species [{speciesName}] distribution map image");

            if (!File.Exists(ZippedFiles.GetFullFilename(DistributionMapImage)))
            {
                Debug.WriteLine($"Missing distribution map for[{speciesName}]");
            }
            if (!OnlyReportMissings) Debug.WriteLine($"End loading species [{speciesName}] distribution map image");
        }

        internal Classification GetFamily(Dbase dbase)
        {
            var batFamilyId = dbase.Classifications.FirstOrDefault(o => o.Id == GenusId).Parent;
            return dbase.Classifications.FirstOrDefault(o => o.Id == batFamilyId);
        }

        public class SimilarSpeciesData
        {
            public string GenusId { get; set; }
            public string SpeciesId { get; set; }
        }
    }

}
