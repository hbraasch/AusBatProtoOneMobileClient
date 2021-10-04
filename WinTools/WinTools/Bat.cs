using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public string DistributionMapImage { get; set; }
        public SpeciesCharacteristics Characteristics { get; set; } = new SpeciesCharacteristics();
        public class SpeciesCharacteristics
        {
            public FloatRange ForeArmLength { get; set; } = FloatRange.GetRandom(57, 65);
            public FloatRange OuterCanineWidth { get; set; } = FloatRange.GetRandom(5.8f, 6.4f);
            public FloatRange TailLength { get; set; } = FloatRange.GetRandom(39, 52);
            public FloatRange FootWithClawLength { get; set; } = FloatRange.GetRandom(10, 12.5f);
            public FloatRange TibiaLength { get; set; } = FloatRange.GetRandom(20, 24);
            public FloatRange PenisLength { get; set; } = FloatRange.GetRandom(3.0f, 4.0f);
            public FloatRange HeadToBodyLength { get; set; } = FloatRange.GetRandom(75, 87);
            public FloatRange Weight { get; set; } = FloatRange.GetRandom(30, 48);
            public FloatRange ThreeMet { get; set; } = FloatRange.GetRandom(33, 40);
            public IsCharacteristicPresent IsGularPoachPresent { get; set; } = GetRandomIsPresent();
            public IsCharacteristicPresent BothSexesHasFleshyGenitalProjections { get; set; } = GetRandomIsPresent();
        }

        public static IsCharacteristicPresent GetRandomIsPresent()
        {
            System.Random random = new System.Random();
            return (random.NextDouble() > 0.5) ? IsCharacteristicPresent.Is_present : IsCharacteristicPresent.Is_not_present;
        }

        internal void LoadDetails(Dbase dbase)
        {
            
        }

        internal async Task LoadImages(Dbase dbase)
        {
            var postFixes = new List<string> { "_head.jpg", "2.jpg", "3.jpg" };
            foreach (var postFix in postFixes)
            {
                var imageName = $"{DataTag.ToLower()}{postFix}";
                Images.Add(imageName);
            }
            if (Images.Count == 0)
            {
                Debug.WriteLine($"No images exist for[{DataTag}]");
                Images.Add("bat.png");
            }
        }

        internal void LoadCalls(Dbase dbase)
        {
            try
            {
                Calls.Add(new CallDataItem { CallAudioFilename = $"{DataTag.ToLower()}.wav" });
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        internal void LoadRegions(Dbase dbase)
        {

        }

        public void SetDistributionMapFilename()
        {
            DistributionMapImage = $"{GenusId.ToLower()}_{SpeciesId.ToLower()}.jpg";
        }
        internal Classification GetFamily(Dbase dbase)
        {
            var batFamilyId = dbase.Classifications.FirstOrDefault(o => o.Id == GenusId).Parent;
            return dbase.Classifications.FirstOrDefault(o => o.Id == batFamilyId);
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
        public string CallAudioFilename { get; set; }
    }

    public enum IsCharacteristicPresent
    {
        Is_present, Is_not_present, Do_not_care
    }

    #region *// Family Key Characteristics
    public class TailPresentCharacteristic : CharacteristicEnumBase
    {
        public enum TailPresentEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "",  "Is absent", "Is present" };

        public TailPresentEnum Key { get; set; }

        public TailPresentCharacteristic(TailPresentEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is TailPresentCharacteristic)
                {
                    if (Key == ((TailPresentCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TailPresentCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TailPresentEnum) promptIndex;
            return new TailPresentCharacteristic(key);
        }
    }

    public class TailMembraneStructureCharacteristic : CharacteristicEnumBase
    {
       
        public enum TailMembraneStructureEnum
        {
            Undefined, Absent, PresentNotAttached, PresentFullyEnclosed, PresentProjectingThrough, PresentProjectingFree
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Present, not attached to tail membrane", "Present, fully enclosed in membrane", "Present, projecting through upper surface of tail membrane", "Present, projecting free foe > 8mm past tail membrane" };

        public TailMembraneStructureEnum Key { get; set; }

        public TailMembraneStructureCharacteristic(TailMembraneStructureEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is TailMembraneStructureCharacteristic)
                {
                    if (Key == ((TailMembraneStructureCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TailMembraneStructureCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TailMembraneStructureEnum)promptIndex;
            return new TailMembraneStructureCharacteristic(key);
        }
    }

    public class SecondFingerClawCharacteristic : CharacteristicEnumBase
    {

        public enum SecondFingerClawEnum
        {
            Undefined, Present, Absent
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Present", "Not present" };

        public SecondFingerClawEnum Key { get; set; }

        public SecondFingerClawCharacteristic(SecondFingerClawEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is SecondFingerClawCharacteristic)
                {
                    if (Key == ((SecondFingerClawCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static SecondFingerClawCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (SecondFingerClawEnum)promptIndex;
            return new SecondFingerClawCharacteristic(key);
        }
    }

    public class FaceStructureNoseLeafCharacteristic : CharacteristicEnumBase
    {

        public enum FaceStructureNoseLeafEnum
        {
            Undefined, None, LargeHorshoe, LargeFlattenned, SmallTransverseLeaf
        }
        public static List<string> Prompts { get; set; } = new List<string>() { 
            "", 
            "None, face simple", 
            "Large complex noseleaf with the lower section horse-shoe shaped",
            "Large complex noseleaf flattenned square, oval", 
            "Small transverse leaf above nostrils"
        };

        public FaceStructureNoseLeafEnum Key { get; set; }

        public FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is FaceStructureNoseLeafCharacteristic)
                {
                    if (Key == ((FaceStructureNoseLeafCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static FaceStructureNoseLeafCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (FaceStructureNoseLeafEnum)promptIndex;
            return new FaceStructureNoseLeafCharacteristic(key);
        }
    }

    public class WingThirdFingerCharacteristic : CharacteristicEnumBase
    {

        public enum WingThirdFingerEnum
        {
            Undefined, Short, Long
        }
        public static List<string> Prompts { get; set; } = new List<string>() {
            "",
            "Termina phalage < 3x the length of the second last phalage",
            "Termina phalage > 3x the length of the second last phalage"
        };

        public WingThirdFingerEnum Key { get; set; }

        public WingThirdFingerCharacteristic(WingThirdFingerEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is WingThirdFingerCharacteristic)
                {
                    if (Key == ((WingThirdFingerCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static WingThirdFingerCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (WingThirdFingerEnum)promptIndex;
            return new WingThirdFingerCharacteristic(key);
        }
    }

    public class TragusCharacteristic : CharacteristicEnumBase
    {

        public enum TragusEnum
        {
            Undefined, Absent, Present, AlmostAbsent
        }
        public static List<string> Prompts { get; set; } = new List<string>() {
            "",
            "Absent",
            "Present, deeply bifurcate",
            "Absent or if present, entire (no deep divisions)"
        };

        public TragusEnum Key { get; set; }

        public TragusCharacteristic(TragusEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacteristicBase> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic is TragusCharacteristic)
                {
                    if (Key == ((TragusCharacteristic)characteristic).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TragusCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TragusEnum)promptIndex;
            return new TragusCharacteristic(key);
        }
    }
    #endregion
}
