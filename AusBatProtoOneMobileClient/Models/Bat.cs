using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TreeApp.Helpers;

namespace AusBatProtoOneMobileClient.Data
{
    public class Bat
    {
        public string ClassificationId { get; set; } 
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageTag { get; set; }
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
        public bool IsGularPoachPresent { get; set; } = GetRandomBool();
        public bool HasFleshyGenitalProjections { get; set; } = GetRandomBool();

        public static bool GetRandomBool()
        {
            System.Random random = new System.Random();
            return (random.NextDouble() > 0.5) ? true : false;
        }

        public void GenerateMockImageIds()
        {
            Images.Add($"{ImageTag}_head.jpg");
            Images.Add($"{ImageTag}2.jpg");
            Images.Add($"{ImageTag}3.jpg");
        }

        public void GenerateMockDetails()
        {
            Details = $"<p><strong>ForeArmLength</strong><br />{ForeArmLength.Min} - {ForeArmLength.Max} mm<br /><strong>" +
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

        public void GenerateMockRegions(List<MapRegion> regions)
        {
            var rnd = new Random();
            var randomRegions = regions.ToRandomOrder();
            var regionAmount = rnd.Next(1, regions.Count - 1);
            for (int i = 0; i < regionAmount; i++)
            {
                MapRegions.Add(randomRegions[i]);
            }
        }

        public void GenerateMockCalls()
        {
            var images = new List<string> { "Aust_australis.jpg", "Chaer_job.jpg", "Chal_dwyeri.jpg", "Chal_gouldi.jpg", "Chal_morio.jpg", "Chal_nigro.jpg", "Doryhi_semoni.jpg" };
            var audioFiles = new List<string> { "bat.wav", "bat2.wav", "bat3.wav" };
            var rnd = new Random();
            var imagesAmount = rnd.Next(1, images.Count - 1);
            Calls = new List<CallDataItem>();
            var randomImages = images.ToRandomOrder();
            for (int i = 0; i < imagesAmount; i++)
            {
                var randomAudioFile = audioFiles[rnd.Next(0, audioFiles.Count - 1)];
                Calls.Add(new CallDataItem { CallImage = randomImages[i], CallFilename = randomAudioFile });                          
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


}
