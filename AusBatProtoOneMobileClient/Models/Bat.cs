using AusBatProtoOneMobileClient.Helpers;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class Bat
    {
        public string GenusId { get; set; }
        public string SpeciesId { get; set; }
        public string Name { get; set; }
        public string DetailsHtml { get; set; }
        public string DataTag { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<CallDataItem> Calls { get; set; } = new List<CallDataItem>();
        public List<MapRegion> MapRegions { get; set; } = new List<MapRegion>();
        public FloatRange ForeArmLength { get; set; } = FloatRange.GetRandom(57, 65);
        public FloatRange OuterCanineWidth { get; set; } = FloatRange.GetRandom(5.8f, 6.4f);
        public FloatRange TailLength { get; set; } = FloatRange.GetRandom(39, 52);
        public FloatRange FootWithClawLength { get; set; } = FloatRange.GetRandom(10, 12.5f);
        public FloatRange TibiaLength { get; set; } = FloatRange.GetRandom(20, 24);
        public FloatRange PenisLength { get; set; } = FloatRange.GetRandom(3.0f, 4.0f);
        public FloatRange HeadToBodyLength { get; set; } = FloatRange.GetRandom(75, 87);
        public FloatRange Weight { get; set; } = FloatRange.GetRandom(30, 48);
        public FloatRange ThreeMet{ get; set; } = FloatRange.GetRandom(33, 40);
        public IsCharacteristicPresent IsGularPoachPresent { get; set; } = GetRandomIsPresent();
        public IsCharacteristicPresent HasFleshyGenitalProjections { get; set; } = GetRandomIsPresent();



        public static IsCharacteristicPresent GetRandomIsPresent()
        {
            System.Random random = new System.Random();
            return (random.NextDouble() > 0.5) ? IsCharacteristicPresent.Is_present : IsCharacteristicPresent.Is_not_present;
        }


        public void GenerateMockDetails()
        {
            DetailsHtml = $"<p><strong>ForeArmLength</strong><br />{ForeArmLength.Min} - {ForeArmLength.Max} mm<br /><strong>" +
                $"OuterCanineWidth</strong><br />{OuterCanineWidth.Min} - {OuterCanineWidth.Max} mm<br /><strong>" +
                $"TailLength</strong><br />{TailLength.Min} - {TailLength.Max} mm<br /><strong>" +
                $"FootWithClawLength</strong><br />{FootWithClawLength.Min} - {FootWithClawLength.Max} mm<br /><strong>" +
                $"TibiaLength</strong><br />{TibiaLength.Min} - {TibiaLength.Max} mm<br /><strong>" +
                $"PenisLength</strong><br />{PenisLength.Min} - {PenisLength.Max} mm<br /><strong>" +
                $"HeadToBodyLength</strong><br />{HeadToBodyLength.Min} - {HeadToBodyLength.Max} mm<br /><strong>" +
                $"Weight</strong> <br />{Weight.Min} - {Weight.Max} g<br /><strong>" +
                $"3-Met</strong><br />{ThreeMet.Min} - {ThreeMet.Max} mm<br /><strong>" +
                $"IsGularPoachPresent</strong> = {IsGularPoachPresent}<br /><strong>" +
                $"HasFleshyGenitalProjections</strong> = {HasFleshyGenitalProjections}</p>";
        }


        internal void LoadDetails()
        {
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.BatDetails.{DataTag.ToLower()}.html"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Details file for [{DataTag}] does not exist");
                        DetailsHtml = @"<p>No detials defined</p>";
                        return;
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string detailsHtml = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(detailsHtml))
                        {
                            throw new BusinessException($"No data inside details file for [{DataTag}]");
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

        internal void LoadImages()
        {
            var postFixes = new List<string> { "_head.jpg", "2.jpg", "3.jpg" };
            foreach (var postFix in postFixes)
            {
                var imageName = $"{DataTag.ToLower()}{postFix}";
                if (ImageChecker.DoesImageExist(imageName))
                {
                    Images.Add(imageName);
                }
            }
            if (Images.Count == 0)
            {
                Debug.WriteLine($"No images exist for[{DataTag}]");
                Images.Add("bat.png");
            }
        }

        

        internal void LoadCalls()
        {
            try
            {
                bool isCallAudioExist = true;
                bool IsCallImageExist = true;
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.CallAudio.{DataTag.ToLower()}.wav"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Call audio file for [{DataTag}] does not exist");
                        isCallAudioExist = false;
                    }
                    else
                    {
                        using (Stream stream2 = FileHelper.GetStreamFromFile($"Data.CallImages.{DataTag.ToLower()}.jpg"))
                        {
                            if (stream2 == null)
                            {
                                Debug.WriteLine($"Call image file for [{DataTag}] does not exist");
                                IsCallImageExist = false;
                            }
                        }
                    }
                    if (isCallAudioExist && IsCallImageExist)
                    {
                        Calls.Add(new CallDataItem { CallImage = $"{DataTag}.wav", CallFilename = $"{DataTag}.jpg" });
                    }
                    else if (isCallAudioExist)
                    {
                        Calls.Add(new CallDataItem { CallImage = $"{DataTag}.wav", CallFilename = "" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem finding call files for [{DataTag}]. {ex.Message}");
            }
        }

        internal void LoadRegions()
        {
            try
            {
                var regionFilename = $"Data.BatRegions.{DataTag.ToLower()}_regions.json";
                using (Stream stream = FileHelper.GetStreamFromFile(regionFilename))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"Regions file for [{DataTag}] does not exist");
                        return;
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string regionsJson = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(regionsJson))
                        {
                            throw new BusinessException($"No data inside regions file for [{DataTag}]");
                        }
                        try
                        {
                            var regionIds = JsonConvert.DeserializeObject<List<int>>(regionsJson);
                            foreach (var regionId in regionIds)
                            {
                                var mapRegion = App.dbase.MapRegions.FirstOrDefault(o=>o.Id == regionId);
                                if (mapRegion == null)
                                {
                                    throw new BusinessException($"Data inside [{DataTag}] regions file refers to non-existing region [{regionId}]");
                                }
                                MapRegions.Add(mapRegion);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new BusinessException($"JSON paring error in [{DataTag}] regions file. {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading details file for [{DataTag}]. {ex.Message}");
            }
        }
    }

    public class FloatRange
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public static FloatRange GetRandom(float min, float max)
        {
            var mean = (max - min) / 2;
            var newMin = Math.Max(0,NextFloat(min - mean, min + mean));
            var newMax = NextFloat(max - mean, max + mean);
            return new FloatRange { Min = newMin, Max = newMax };

            float NextFloat(float minVal, float maxVal)
            {
                System.Random random = new System.Random();
                double val = (random.NextDouble() * (maxVal - minVal) + minVal);
                return (float) val;
            }
        }


    }

    public class CallDataItem
    {
        public string CallImage { get; set; }
        public string CallFilename { get; set; }
    }

    public enum IsCharacteristicPresent
    {
        Is_present, Is_not_present, Do_not_care
    }


}
