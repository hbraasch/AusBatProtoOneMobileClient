using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Helpers;
using Mobile.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TreeApp.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Models
{
    public class Dbase
    {
        const string DBASE_FILENAME = "dbase.json";
        static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public string IntroductionHtml;
        public string AboutHtml;
        public List<Classification> Classifications = new List<Classification>();
        public List<Species> Species = new List<Species>();
        public List<MapRegion> MapRegions = new List<MapRegion>();
        public List<Sighting> Sightings = new List<Sighting>();

        public static Dbase Load()
        {
            var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, DBASE_FILENAME);
            if (!File.Exists(filePath))
            {
                return new Dbase();
            }
            var dbaseJson = File.ReadAllText(filePath);
            if (dbaseJson.IsEmpty()) return new Dbase();
            return JsonConvert.DeserializeObject<Dbase>(dbaseJson, settings);
        }
        public static void Save(Dbase dbase)
        {
            var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, DBASE_FILENAME);
            var dbaseJson = JsonConvert.SerializeObject(dbase, settings);
            File.WriteAllText(filePath, dbaseJson);
        }

        public static void Clear()
        {
            Save(new Dbase());
        }

        public async Task Init()
        {
            try
            {
                var dbase = new Dbase();

                dbase.IntroductionHtml = LoadIntroduction();
                dbase.AboutHtml = LoadAbout();

                Debug.WriteLine(JsonConvert.SerializeObject(new List<int> { 101, 102 }).ToString());

                #region *// Provinces map
                var hotspotRadius = 0.05f;
                dbase.MapRegions.Add(new Models.MapRegion { Id = 102, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.79, 0.21), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 103, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.81, 0.32), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 104, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.87, 0.42), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 105, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.93, 0.59), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 106, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.87, 0.70), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 107, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.81, 0.80), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 108, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.78, 0.94), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 201, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.73, 0.23), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 202, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.70, 0.50), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 203, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.73, 0.72), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 301, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.46, 0.13), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 302, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.31, 0.23), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 304, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.50, 0.29), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 306, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.28, 0.40), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 307, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.12, 0.41), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 308, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.07, 0.50), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 309, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.19, 0.59), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 310, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.37, 0.61), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 311, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.56, 0.67), Radius = hotspotRadius } } });
                dbase.MapRegions.Add(new Models.MapRegion { Id = 312, Hotspots = new List<Models.MapRegion.HotSpotItem> { new Models.MapRegion.HotSpotItem { Center = new Point(0.63, 0.66), Radius = hotspotRadius } } });
                #endregion

                #region *// Family/Genus/Species
                dbase.Classifications.Add(new Classification { Id = "Pteropodidae", Type = Classification.ClassificationType.Family, 
                    Characters = new List<CharacterBase> {  
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Absent), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.Absent),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Present),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.Absent)
                    },
                });
                dbase.Classifications.Add(new Classification { Id = "Macroglossus", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae" });
                dbase.Classifications.Add(new Classification { Id = "Minimus", Type = Classification.ClassificationType.Species, Parent = "Macroglossus" });
                dbase.Classifications.Add(new Classification { Id = "Nyctimene", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae" });
                dbase.Classifications.Add(new Classification { Id = "Robinsoni", Type = Classification.ClassificationType.Species, Parent = "Nyctimene" });
                dbase.Classifications.Add(new Classification { Id = "Pteropus", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae" });
                dbase.Classifications.Add(new Classification { Id = "Alecto", Type = Classification.ClassificationType.Species, Parent = "Pteropus" });
                dbase.Classifications.Add(new Classification { Id = "Conspicillatus", Type = Classification.ClassificationType.Species, Parent = "Pteropus" });
                dbase.Classifications.Add(new Classification { Id = "Natalis", Type = Classification.ClassificationType.Species, Parent = "Pteropus" });
                dbase.Classifications.Add(new Classification { Id = "Poliocephalus", Type = Classification.ClassificationType.Species, Parent = "Pteropus" });
                dbase.Classifications.Add(new Classification { Id = "Scapulatus", Type = Classification.ClassificationType.Species, Parent = "Pteropus" });
                dbase.Classifications.Add(new Classification { Id = "Syconycteris", Type = Classification.ClassificationType.Species, Parent = "Pteropodidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Syconycteris" });

                dbase.Classifications.Add(new Classification { Id = "Pteropodidae(Dobsonia)", Type = Classification.ClassificationType.Family, 
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentNotAttached),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.Absent) } });
                dbase.Classifications.Add(new Classification { Id = "Macroderma", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae(Dobsonia)" });
                dbase.Classifications.Add(new Classification { Id = "Magna", Type = Classification.ClassificationType.Species, Parent = "Macroderma" });

                dbase.Classifications.Add(new Classification { Id = "Megadermatidae", Type = Classification.ClassificationType.Family , 
                    Characters = new List<CharacterBase> {  
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.Absent),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.LargeHorshoe),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.Present) } }); 
                dbase.Classifications.Add(new Classification { Id = "Dobsonia", Type = Classification.ClassificationType.Genus, Parent = "Megadermatidae" });
                dbase.Classifications.Add(new Classification { Id = "Gigas", Type = Classification.ClassificationType.Species, Parent = "Dobsonia" });


                dbase.Classifications.Add(new Classification { Id = "Rhinolophidae", Type = Classification.ClassificationType.Family, 
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.LargeHorshoe),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent) } });
                dbase.Classifications.Add(new Classification { Id = "Rhinolophus", Type = Classification.ClassificationType.Genus, Parent = "Rhinolophidae" });
                dbase.Classifications.Add(new Classification { Id = "Megaphyllus", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Robertsi", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Sp 'intermediate'", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });



                dbase.Classifications.Add(new Classification { Id = "Hipposideridae", Type = Classification.ClassificationType.Family, ImageTag = "Hipp_family",
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.LargeFlattenned),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent) }
                }); 
                dbase.Classifications.Add(new Classification { Id = "Hipposideros", Type = Classification.ClassificationType.Genus, Parent = "Hipposideridae" });
                dbase.Classifications.Add(new Classification { Id = "Ater", Type = Classification.ClassificationType.Species, Parent = "Hipposideros",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(34.0f, 41.0f),
                        new FiveMetCharacter(26.0f, 32.0f),
                        new ThreeMetCharacter(25.0f, 31.0f),
                        new WingspanCharacter(220.0f, 275.0f),
                        new HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesCharacter.HiposideridaeFacialFeaturesEnum.Dory_semoni),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Cervinus", Type = Classification.ClassificationType.Species, Parent = "Hipposideros",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(45.0f, 48.0f),
                        new FiveMetCharacter(29.0f, 33.0f),
                        new ThreeMetCharacter(31.0f, 39.0f),
                        new WingspanCharacter(280.0f, 300.0f),
                        new HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesCharacter.HiposideridaeFacialFeaturesEnum.Hipp_cerv),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Diadema", Type = Classification.ClassificationType.Species, Parent = "Hipposideros",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(77.0f, 86.0f),
                        new FiveMetCharacter(53.0f, 59.0f),
                        new ThreeMetCharacter(59.0f, 66.0f),
                        new WingspanCharacter(470.0f, 520.0f),
                        new HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesCharacter.HiposideridaeFacialFeaturesEnum.Hipp_diad),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Inornatus", Type = Classification.ClassificationType.Species, Parent = "Hipposideros",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(68.0f, 74.0f),
                        new FiveMetCharacter(48.0f, 51.0f),
                        new ThreeMetCharacter(53.0f, 57.0f),
                        new WingspanCharacter(380.0f, 448.0f),
                        new HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesCharacter.HiposideridaeFacialFeaturesEnum.Hipp_inornat),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Semoni", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Stenotis", Type = Classification.ClassificationType.Species, Parent = "Hipposideros",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(41.0f, 46.0f),
                        new FiveMetCharacter(30.0f, 32.0f),
                        new ThreeMetCharacter(30.0f, 33.0f),
                        new WingspanCharacter(235.0f, 280.0f),
                        new HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesCharacter.HiposideridaeFacialFeaturesEnum.Dory_stenotis),
                    }
                });



                dbase.Classifications.Add(new Classification { Id = "Minioperidae", Type = Classification.ClassificationType.Family,
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Long),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent) }
                });
                dbase.Classifications.Add(new Classification { Id = "Miniopterus", Type = Classification.ClassificationType.Genus, Parent = "Minioperidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae bassanii", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae oceanensis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae orianae", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });



                dbase.Classifications.Add(new Classification { Id = "Emballonuridae", Type = Classification.ClassificationType.Family, ImageTag = "Embal_family",
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present) , 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentProjectingThrough),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent)  }
                });
                dbase.Classifications.Add(new Classification { Id = "Saccolaimus", Type = Classification.ClassificationType.Genus, Parent = "Emballonuridae" });
                dbase.Classifications.Add(new Classification { Id = "Flaviventris", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(5.8f, 5.7f),
                        new TibiaCharacter(26.0f, 33.0f),
                        new ThreeMetCharacter(72.0f, 84.5f),
                        new ForeArmCharacter(65.0f, 83.0f),
                        new TailLengthCharacter(21.0f, 33.5f),
                        new EarLengthCharacter(16.0f, 23.0f),
                        new SnoutToVentLengthCharacter(72.0f, 92.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.AbsentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Mixtus", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(4.4f, 4.9f),
                        new TibiaCharacter(22.0f, 26.0f),
                        new ThreeMetCharacter(61.0f, 65.0f),
                        new ForeArmCharacter(61.0f, 68.0f),
                        new TailLengthCharacter(21.0f, 27.0f),
                        new EarLengthCharacter(16.5f, 21.5f),
                        new SnoutToVentLengthCharacter(70.0f, 79.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.PresentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Saccolaimus", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(5.0f, 5.7f),
                        new TibiaCharacter(28.0f, 32.0f),
                        new ThreeMetCharacter(73.0f, 83.0f),
                        new ForeArmCharacter(72.0f, 81.0f),
                        new TailLengthCharacter(22.0f, 41.0f),
                        new EarLengthCharacter(16.0f, 23.0f),
                        new SnoutToVentLengthCharacter(81.0f, 110.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.PresentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Taphozous", Type = Classification.ClassificationType.Genus, Parent = "Emballonuridae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Taphozous",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(3.6f, 4.2f),
                        new TibiaCharacter(23.5f, 27.5f),
                        new ThreeMetCharacter(57.0f, 61.0f),
                        new ForeArmCharacter(61.0f, 68.0f),
                        new TailLengthCharacter(22.0f, 31.0f),
                        new EarLengthCharacter(18.0f, 25.0f),
                        new SnoutToVentLengthCharacter(61.0f, 75.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.AbsentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Georgianus", Type = Classification.ClassificationType.Species, Parent = "Taphozous",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(3.7f, 4.5f),
                        new TibiaCharacter(26.0f, 31.0f),
                        new ThreeMetCharacter(55.0f, 61.0f),
                        new ForeArmCharacter(61.0f, 74.0f),
                        new TailLengthCharacter(27.0f, 34.5f),
                        new EarLengthCharacter(16.0f, 25.5f),
                        new SnoutToVentLengthCharacter(60.0f, 82.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.Absent),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Absent),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Hilli", Type = Classification.ClassificationType.Species, Parent = "Taphozous",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(3.0f, 4.9f),
                        new TibiaCharacter(25.0f, 32.0f),
                        new ThreeMetCharacter(54.0f, 65.0f),
                        new ForeArmCharacter(60.0f, 72.0f),
                        new TailLengthCharacter(23.0f, 38.0f),
                        new EarLengthCharacter(18.0f, 24.0f),
                        new SnoutToVentLengthCharacter(64.0f, 82.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.AbsentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Kapalgensis", Type = Classification.ClassificationType.Species, Parent = "Taphozous",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(4.0f, 4.5f),
                        new TibiaCharacter(22.0f, 26.0f),
                        new ThreeMetCharacter(57.0f, 61.0f),
                        new ForeArmCharacter(58.0f, 65.0f),
                        new TailLengthCharacter(17.0f, 25.0f),
                        new EarLengthCharacter(16.0f, 20.0f),
                        new SnoutToVentLengthCharacter(68.0f, 82.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.AbsentOrRudimentary),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Present),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Troughtoni", Type = Classification.ClassificationType.Species, Parent = "Taphozous",
                    Characters = new List<CharacterBase>
                    {
                        new OuterCanineWidthCharacter(3.9f, 4.7f),
                        new TibiaCharacter(29.0f, 33.5f),
                        new ThreeMetCharacter(57.0f, 65.0f),
                        new ForeArmCharacter(67.0f, 76.0f),
                        new TailLengthCharacter(28.0f, 38.0f),
                        new EarLengthCharacter(19.0f, 27.5f),
                        new SnoutToVentLengthCharacter(79.0f, 95.0f),
                        new GularPouchFemaleCharacter(GularPouchFemaleCharacter.GularPouchFemaleEnum.Absent),
                        new GularPouchMaleCharacter(GularPouchMaleCharacter.GularPouchMaleEnum.Absent),
                        new MetacarpalWingPouchCharacter(MetacarpalWingPouchCharacter.MetacarpalWingPouchEnum.Present),
                    }
                });



                dbase.Classifications.Add(new Classification { Id = "Molossidae", Type = Classification.ClassificationType.Family,
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentProjectingFree),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent) }
                });
                dbase.Classifications.Add(new Classification { Id = "Austonomus", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Austonomus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(57.0f, 65.0f),
                        new OuterCanineWidthCharacter(5.8f, 6.4f),
                        new TailLengthCharacter(39.0f, 53.0f),
                        new FootCharacter(10.0f, 12.5f),
                        new TibiaCharacter(17.0f, 24.0f),
                        new PenisLengthCharacter(2.8f, 8.0f),
                        new ThreeMetCharacter(54.0f, 59.0f),
                        new FiveMetCharacter(29.0f, 33.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Present),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });      
                dbase.Classifications.Add(new Classification { Id = "Chaerephon", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Jobensis", Type = Classification.ClassificationType.Species, Parent = "Chaerephon",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(46.0f, 52.0f),
                        new OuterCanineWidthCharacter(5.6f, 6.2f),
                        new TailLengthCharacter(32.0f, 38.0f),
                        new FootCharacter(8.0f, 10.0f),
                        new TibiaCharacter(14.0f, 18.0f),
                        new PenisLengthCharacter(3.0f, 8.0f),
                        new ThreeMetCharacter(42.0f, 52.0f),
                        new FiveMetCharacter(27.0f, 32.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Micronomus", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Norfolkensis", Type = Classification.ClassificationType.Species, Parent = "Micronomus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(35.0f, 39.0f),
                        new OuterCanineWidthCharacter(3.7f, 4.2f),
                        new TailLengthCharacter(30.0f, 40.0f),
                        new FootCharacter(5.9f, 8.1f),
                        new TibiaCharacter(11.0f, 12.2f),
                        new PenisLengthCharacter(2.0f, 3.0f),
                        new ThreeMetCharacter(27.0f, 29.0f),
                        new FiveMetCharacter(30.0f, 37.2f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Present),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Ozimops", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Cobourgianus", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 36.1f),
                        new OuterCanineWidthCharacter(3.7f, 4.4f),
                        new TailLengthCharacter(28.0f, 36.0f),
                        new FootCharacter(3.8f, 6.0f),
                        new TibiaCharacter(9.9f, 13.2f),
                        new PenisLengthCharacter(3.0f, 5.0f),
                        new ThreeMetCharacter(34.0f, 38.0f),
                        new FiveMetCharacter(25.0f, 28.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Halli", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.1f, 36.0f),
                        new OuterCanineWidthCharacter(4.4f, 4.9f),
                        new TailLengthCharacter(23.0f, 28.0f),
                        new FootCharacter(4.5f, 7.0f),
                        new TibiaCharacter(10.4f, 13.0f),
                        new PenisLengthCharacter(4.0f, 6.0f),
                        new ThreeMetCharacter(33.5f, 36.6f),
                        new FiveMetCharacter(25.0f, 28.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Kitcheneri", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 36.0f),
                        new OuterCanineWidthCharacter(4.2f, 4.9f),
                        new TailLengthCharacter(28.0f, 36.0f),
                        new FootCharacter(4.3f, 6.9f),
                        new TibiaCharacter(10.6f, 11.8f),
                        new PenisLengthCharacter(4.0f, 6.0f),
                        new ThreeMetCharacter(34.0f, 38.0f),
                        new FiveMetCharacter(24.5f, 27.5f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Lumsdenae", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(35.2f, 40.4f),
                        new OuterCanineWidthCharacter(4.4f, 5.6f),
                        new TailLengthCharacter(24.0f, 37.0f),
                        new FootCharacter(4.6f, 7.4f),
                        new TibiaCharacter(9.3f, 14.0f),
                        new PenisLengthCharacter(3.5f, 4.5f),
                        new ThreeMetCharacter(36.6f, 42.5f),
                        new FiveMetCharacter(24.0f, 30.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Petersi", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 39.0f),
                        new OuterCanineWidthCharacter(4.0f, 4.9f),
                        new TailLengthCharacter(24.0f, 35.0f),
                        new FootCharacter(4.1f, 6.8f),
                        new TibiaCharacter(9.3f, 12.2f),
                        new PenisLengthCharacter(2.0f, 5.0f),
                        new ThreeMetCharacter(33.0f, 41.2f),
                        new FiveMetCharacter(33.0f, 39.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Planiceps", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                    new ForeArmCharacter(32.2f, 36.2f),
                    new OuterCanineWidthCharacter(4.0f, 4.8f),
                    new TailLengthCharacter(22.0f, 33.0f),
                    new FootCharacter(4.2f, 6.6f),
                    new TibiaCharacter(9.9f, 12.0f),
                    new PenisLengthCharacter(7.8f, 10.0f),
                    new ThreeMetCharacter(33.0f, 38.0f),
                    new FiveMetCharacter(24.0f, 29.0f),
                    new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                    new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Ridei", Type = Classification.ClassificationType.Species, Parent = "Ozimops",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(30.0f, 35.3f),
                        new OuterCanineWidthCharacter(3.6f, 4.6f),
                        new TailLengthCharacter(18.0f, 31.0f),
                        new FootCharacter(4.0f, 7.0f),
                        new TibiaCharacter(9.0f, 13.0f),
                        new PenisLengthCharacter(2.6f, 5.0f),
                        new ThreeMetCharacter(31.0f, 38.0f),
                        new FiveMetCharacter(22.0f, 29.0f),
                        new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                        new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Absent),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Setirostris", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Eleryi", Type = Classification.ClassificationType.Species, Parent = "Setirostris",
                    Characters = new List<CharacterBase>
                    {
                    new ForeArmCharacter(31.0f, 36.0f),
                    new OuterCanineWidthCharacter(2.9f, 3.8f),
                    new TailLengthCharacter(27.0f, 37.0f),
                    new FootCharacter(5.0f, 7.0f),
                    new TibiaCharacter(9.0f, 12.0f),
                    new PenisLengthCharacter(2.8f, 4.1f),
                    new ThreeMetCharacter(32.0f, 35.0f),
                    new FiveMetCharacter(22.6f, 30.0f),
                    new GularPouchCharacter(GularPouchCharacter.GularPouchEnum.Absent),
                    new BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsCharacter.BothSexesWithFleshyGenitalProjectionsEnum.Present),
                    }
                });



                dbase.Classifications.Add(new Classification { Id = "Vespertilionidae", Type = Classification.ClassificationType.Family, ImageTag = "Vesp_family",
                    Characters = new List<CharacterBase> { 
                        new TailPresentCharacter(TailPresentCharacter.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacter(TailMembraneStructureCharacter.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacter(SecondFingerClawCharacter.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacter(FaceStructureNoseLeafCharacter.FaceStructureNoseLeafEnum.SmallTransverseLeaf),
                        new WingThirdFingerCharacter(WingThirdFingerCharacter.WingThirdFingerEnum.Short),
                        new TragusCharacter(TragusCharacter.TragusEnum.AlmostAbsent) }
                });
                dbase.Classifications.Add(new Classification { Id = "Chalinolobus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 48.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsInequalLargerToothBifid),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.Orbicular),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoPair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.SmallLobe),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Dwyeri", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(36.0f, 45.0f),
                        new ThreeMetCharacter(36.0f, 40.0f),
                        new ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourCharacter.ChalinolobusFurPatternAndColourEnum.BlackWithLaterStripesWingStructure),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Gouldii", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(35.0f, 47.0f),
                        new ThreeMetCharacter(31.0f, 45.0f),
                        new ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourCharacter.ChalinolobusFurPatternAndColourEnum.Brown),
                        }
                });
                dbase.Classifications.Add(new Classification { Id = "Morio", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(33.0f, 44.0f),
                        new ThreeMetCharacter(33.0f, 38.0f),
                        new ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourCharacter.ChalinolobusFurPatternAndColourEnum.BrownOrGrey),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Nigrogriseus", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus",
                    Characters = new List<CharacterBase>
                    {
                    new ForeArmCharacter(31.0f, 38.0f),
                    new ThreeMetCharacter(25.0f, 37.0f),
                    new ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourCharacter.ChalinolobusFurPatternAndColourEnum.BlackOrGrey),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Picatus", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 38.0f),
                        new ThreeMetCharacter(30.0f, 33.0f),
                        new ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourCharacter.ChalinolobusFurPatternAndColourEnum.BlackWithLaterStripes),
                    }
                });

                dbase.Classifications.Add(new Classification { Id = "Falsistrellus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(44.0f, 58.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsInequalNeitherBifid),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoPair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Mackenziei", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });
                dbase.Classifications.Add(new Classification { Id = "Tasmaniensis", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });

                dbase.Classifications.Add(new Classification { Id = "Murina", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 36.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsEqualNeitherBifid),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoPair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.Hairy),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Extended),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Florium", Type = Classification.ClassificationType.Species, Parent = "Murina" });

                dbase.Classifications.Add(new Classification { Id = "Myotis", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(36.0f, 43.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsEqualBothBifid),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoTriplets),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Macropus", Type = Classification.ClassificationType.Species, Parent = "Myotis" });

                dbase.Classifications.Add(new Classification { Id = "Nyctophilus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(30.0f, 50.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.OnePair),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.OnePair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.NotSmooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Arnhemensis", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Bifax", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Corbeni", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Daedalus", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Geoffroyi", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Gouldi", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Major major", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Major tor", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Sherrini", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });
                dbase.Classifications.Add(new Classification { Id = "Walkeri", Type = Classification.ClassificationType.Species, Parent = "Nyctophilus" });

                dbase.Classifications.Add(new Classification { Id = "Phoniscus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(34.0f, 41.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.OnePair),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.Linear),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.OnePair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Groove),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.Hairy),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Papuensis", Type = Classification.ClassificationType.Species, Parent = "Phoniscus" });

                dbase.Classifications.Add(new Classification { Id = "Pipistrellus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                    new ForeArmCharacter(27.0f, 33.0f),
                    new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsInequalLargerToothBifid),
                    new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsInequalNeitherBifid),
                    new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                    new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoPair),
                    new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                    new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                    new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                    new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                    new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Adamsi", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 41.0f),
                        new OuterCanineWidthCharacter(3.9f, 4.4f),
                        new FaTibiaRatioCharacter(2.3f, 2.7f),
                        new TibiaCharacter(12.0f, 18.0f),
                        new PipistrellusTragusCharacter(PipistrellusTragusCharacter.PipistrellusTragusEnum.BroadestInMiddle),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Murrayi", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus" });
                dbase.Classifications.Add(new Classification { Id = "Westralis", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(27.0f, 35.0f),
                        new OuterCanineWidthCharacter(3.3f, 3.8f),
                        new FaTibiaRatioCharacter(2.2f, 2.4f),
                        new TibiaCharacter(11.0f, 16.0f),
                        new PipistrellusTragusCharacter(PipistrellusTragusCharacter.PipistrellusTragusEnum.BroadestAtBase),
                    }
                });

                dbase.Classifications.Add(new Classification { Id = "Scoteanax", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(50.0f, 57.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.OnePair),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.OnePair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Rueppellii", Type = Classification.ClassificationType.Species, Parent = "Scoteanax" });

                dbase.Classifications.Add(new Classification { Id = "Scotorepens", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(27.0f, 41.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.OnePair),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.TwoPair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Balstoni", Type = Classification.ClassificationType.Species, Parent = "Scotorepens",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 41.0f),
                        new OuterCanineWidthCharacter(4.3f, 5.6f),
                        new FaTibiaRatioCharacter(2.3f, 2.7f),
                        new TibiaCharacter(12.0f, 18.0f),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Greyii", Type = Classification.ClassificationType.Species, Parent = "Scotorepens",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(27.0f, 34.5f),
                        new OuterCanineWidthCharacter(3.9f, 5.1f),
                        new FaTibiaRatioCharacter(2.2f, 2.4f),
                        new TibiaCharacter(11.0f, 16.0f),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Orion", Type = Classification.ClassificationType.Species, Parent = "Scotorepens",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 39.0f),
                        new OuterCanineWidthCharacter(4.9f, 5.3f),
                        new FaTibiaRatioCharacter(2.5f, 2.7f),
                        new TibiaCharacter(12.5f, 15.0f),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Sanborni", Type = Classification.ClassificationType.Species, Parent = "Scotorepens",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(27.0f, 37.0f),
                        new OuterCanineWidthCharacter(3.9f, 5.0f),
                        new FaTibiaRatioCharacter(2.1f, 2.4f),
                        new TibiaCharacter(11.1f, 15.8f),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "(Parnaby)", Type = Classification.ClassificationType.Species, Parent = "Scotorepens",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(31.0f, 34.0f),
                        new OuterCanineWidthCharacter(4.0f, 4.9f),
                        new FaTibiaRatioCharacter(2.3f, 2.6f),
                        new TibiaCharacter(12.1f, 14.9f),
                    }
                });

                dbase.Classifications.Add(new Classification { Id = "Vespadalus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(22.0f, 38.0f),
                        new VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsCharacter.VespertilionidaeUpperIncisorsEnum.TwoPairsInequalLargerToothBifid),
                        new VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeCharacter.VespertilionidaeTragusShapeEnum.OblongLanceolate),
                        new VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsCharacter.VespertilionidaeUpperPreMolarsEnum.OnePair),
                        new VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesCharacter.VespertilionidaeUpperCannesEnum.Smooth),
                        new VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneCharacter.VespertilionidaeTailMembraneEnum.NoFur),
                        new VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureCharacter.VespertilionidaeMuzzleStructureEnum.Smooth),
                        new VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeCharacter.VespertilionidaeNostrilShapeEnum.Flat),
                        new VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginCharacter.VespertilionidaeLowerEarMarginEnum.NoLobes),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Caurinus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(26.0f, 32.0f),
                        new ThreeMetCharacter(25.0f, 32.0f),
                        new TibiaCharacter(10.5f, 12.5f),
                        new ThreeP1Character(9.5f, 12.0f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.BrownToBlack),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Douglasorum", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(34.0f, 38.0f),
                        new ThreeMetCharacter(32.0f, 37.0f),
                        new TibiaCharacter(13.5f, 15.8f),
                        new ThreeP1Character(9.8f, 12.0f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.3f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.BrownToBlack),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Finlaysoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(29.0f, 38.0f),
                        new ThreeMetCharacter(29.0f, 37.0f),
                        new TibiaCharacter(11.0f, 16.5f),
                        new ThreeP1Character(10.8f, 14.0f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.BrownToBlack),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Regulus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(28.0f, 35.0f),
                        new ThreeMetCharacter(27.0f, 33.0f),
                        new TibiaCharacter(11.2f, 14.0f),
                        new ThreeP1Character(9.5f, 12.5f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.NoneToLightBrown),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Baverstocki", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(26.0f, 32.0f),
                        new ThreeMetCharacter(26.0f, 32.0f),
                        new TibiaCharacter(10.5f, 13.2f),
                        new ThreeP1Character(9.3f, 11.1f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.NoneToLightBrown),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Darlingtoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 38.0f),
                        new ThreeMetCharacter(30.0f, 36.0f),
                        new TibiaCharacter(12.1f, 14.5f),
                        new ThreeP1Character(11.5f, 14.5f),
                        new ThreeP1TheeMetRatioCharacter(0.4f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.PinkToYellow),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Pumilus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(28.0f, 34.0f),
                        new ThreeMetCharacter(27.0f, 33.0f),
                        new TibiaCharacter(11.5f, 14.0f),
                        new ThreeP1Character(10.2f, 12.5f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.BrownToBlack),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Troughtoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(32.0f, 37.0f),
                        new ThreeMetCharacter(32.0f, 36.0f),
                        new TibiaCharacter(12.5f, 14.5f),
                        new ThreeP1Character(11.3f, 13.8f),
                        new ThreeP1TheeMetRatioCharacter(0.3f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.NoneToBrown),
                    }
                });
                dbase.Classifications.Add(new Classification { Id = "Vulturnus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus",
                    Characters = new List<CharacterBase>
                    {
                        new ForeArmCharacter(23.0f, 33.0f),
                        new ThreeMetCharacter(24.0f, 32.0f),
                        new TibiaCharacter(10.5f, 13.0f),
                        new ThreeP1Character(10.3f, 12.5f),
                        new ThreeP1TheeMetRatioCharacter(0.4f, 0.4f),
                        new VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationCharacter.VespadelusFacialSkinPigmentationEnum.NoneToLightBrown),
                    }
                });
                #endregion  

                #region *// Species
                dbase.Species.Add(LoadSpecies( "Austonomus", "australis")); 
                dbase.Species.Add(LoadSpecies( "Chaerephon", "jobensis"));
                dbase.Species.Add(LoadSpecies( "Chalinolobus", "dwyeri"));
                dbase.Species.Add(LoadSpecies( "Chalinolobus", "gouldii"));
                dbase.Species.Add(LoadSpecies( "Chalinolobus", "morio"));
                dbase.Species.Add(LoadSpecies( "Chalinolobus", "nigrogriseus"));
                dbase.Species.Add(LoadSpecies( "Chalinolobus", "picatus"));
                dbase.Species.Add(LoadSpecies( "Dobsonia", "magna"));
                dbase.Species.Add(LoadSpecies( "Falsistrellus", "mackenziei"));
                dbase.Species.Add(LoadSpecies( "Falsistrellus", "tasmaniensis"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "ater"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "cervinus"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "diadema"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "inornatus"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "semoni"));
                dbase.Species.Add(LoadSpecies( "Hipposideros", "stenotis"));
                dbase.Species.Add(LoadSpecies( "Macroderma", "gigas"));
                dbase.Species.Add(LoadSpecies( "Macroglossus", "minimus"));
                dbase.Species.Add(LoadSpecies( "Micronomus", "norfolkensis"));
                dbase.Species.Add(LoadSpecies( "Miniopterus", "australis"));
                dbase.Species.Add(LoadSpecies( "Miniopterus", "orianae bassanii"));
                dbase.Species.Add(LoadSpecies( "Miniopterus", "orianae oceanensi"));
                dbase.Species.Add(LoadSpecies( "Miniopterus", "orianae orianae"));
                dbase.Species.Add(LoadSpecies( "Murina", "florium"));
                dbase.Species.Add(LoadSpecies( "Myotis", "macropus"));
                dbase.Species.Add(LoadSpecies( "Nyctimene", "robinsoni"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "arnhemensis"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "bifax"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "corbeni"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "daedalus"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "geoffroyi"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "gouldi"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "major major"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "major tor"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "sherrini"));
                dbase.Species.Add(LoadSpecies( "Nyctophilus", "walkeri"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "cobourgianus"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "halli"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "kitcheneri"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "lumsdenae"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "petersi"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "planiceps"));
                dbase.Species.Add(LoadSpecies( "Ozimops", "ridei"));
                dbase.Species.Add(LoadSpecies( "Phoniscus", "papuensis"));
                dbase.Species.Add(LoadSpecies( "Pipistrellus", "adamsi"));
                dbase.Species.Add(LoadSpecies( "Pipistrellus", "murrayi"));
                dbase.Species.Add(LoadSpecies( "Pipistrellus", "westralis"));
                dbase.Species.Add(LoadSpecies( "Pteropus", "alecto"));
                dbase.Species.Add(LoadSpecies( "Pteropus", "conspicillatus"));
                dbase.Species.Add(LoadSpecies( "Pteropus", "natalis"));
                dbase.Species.Add(LoadSpecies( "Pteropus", "poliocephalus"));
                dbase.Species.Add(LoadSpecies( "Pteropus", "scapulatus"));
                dbase.Species.Add(LoadSpecies( "Rhinolophus", "megaphyllus"));
                dbase.Species.Add(LoadSpecies( "Rhinolophus", "robertsi"));
                dbase.Species.Add(LoadSpecies( "Rhinolophus", "sp 'intermediate'"));
                dbase.Species.Add(LoadSpecies( "Rhinonicteris", "aurantia"));
                dbase.Species.Add(LoadSpecies( "Saccolaimus", "flaviventris"));
                dbase.Species.Add(LoadSpecies( "Saccolaimus", "mixtus"));
                dbase.Species.Add(LoadSpecies( "Saccolaimus", "saccolaimus"));
                dbase.Species.Add(LoadSpecies( "Scoteanax", "rueppellii"));
                dbase.Species.Add(LoadSpecies( "Scotorepens", "balstoni"));
                dbase.Species.Add(LoadSpecies( "Scotorepens", "greyii"));
                dbase.Species.Add(LoadSpecies( "Scotorepens", "orion"));
                dbase.Species.Add(LoadSpecies( "Scotorepens", "sanborni"));
                dbase.Species.Add(LoadSpecies( "Scotorepens", "(Parnaby)"));
                dbase.Species.Add(LoadSpecies( "Setirostris", "eleryi"));
                dbase.Species.Add(LoadSpecies( "Syconycteris", "australis"));
                dbase.Species.Add(LoadSpecies( "Taphozous", "australis"));
                dbase.Species.Add(LoadSpecies( "Taphozous", "georgianus"));
                dbase.Species.Add(LoadSpecies( "Taphozous", "hilli"));
                dbase.Species.Add(LoadSpecies( "Taphozous", "kapalgensis"));
                dbase.Species.Add(LoadSpecies( "Taphozous", "troughtoni"));
                dbase.Species.Add(LoadSpecies( "Vespadalus", "caurinus"));
                dbase.Species.Add(LoadSpecies( "Vespadalus", "douglasorum"));
                dbase.Species.Add(LoadSpecies( "Vespadalus", "finlaysoni"));
                dbase.Species.Add(LoadSpecies( "Vespadalus", "regulus"));
                dbase.Species.Add(LoadSpecies( "Vespadelus", "baverstocki"));
                dbase.Species.Add(LoadSpecies( "Vespadelus", "darlingtoni"));
                dbase.Species.Add(LoadSpecies( "Vespadelus", "pumilus"));
                dbase.Species.Add(LoadSpecies( "Vespadelus", "troughtoni"));
                dbase.Species.Add(LoadSpecies( "Vespadelus", "vulturnus"));
                #endregion

                foreach (var species in dbase.Species)
                {
                    species.LoadRegions(dbase);
                    species.LoadDetails();
                    await species.LoadImages();
                    await species.LoadCalls();
                    species.SetDistributionMapFilename();
                }


                var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Debug.WriteLine($"Writing files to path: {folderPath}");

#if false
                var batsCharsJson = JsonConvert.SerializeObject(dbase.Bats[0], Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, "batChars.json"), batsCharsJson); 
#endif

                Dbase.Save(dbase);
                App.dbase = dbase;
            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                throw new BusinessException("Cancelled by user");
            }
            catch
            {
                throw;
            }
        }



        public static List<Species> Filter(List<Species> bats, List<MapRegion> selectedRegions)
        {
            if (selectedRegions.IsEmpty()) return bats;
            return bats.Where(o => o.MapRegions.Intersect(selectedRegions, new RegionComparer()).Count() > 0).ToList();
        }

        public static List<Species> Order(List<Species> specieses)
        {
            return specieses.OrderBy(species => $"{species.GenusId} {species.SpeciesId}").ToList();
        }

        public class RegionComparer : IEqualityComparer<MapRegion>
        {
            public bool Equals(MapRegion x, MapRegion y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(MapRegion obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        internal List<Species> GetAllSpecies()
        {
            return Species.OrderBy(species => $"{species.GenusId} {species.SpeciesId}").ToList();
        }

        internal List<Species> GetAllSpeciesInFamily(Classification family)
        {
            var genusesInFamily = App.dbase.Classifications.Where(o => o.Parent == family.Id).ToList();
            return GetAllSpecies(genusesInFamily);
        }

        internal List<Species> GetAllSpecies(List<Classification> genuses)
        {
            List<Species> result = new List<Species>();
            foreach (var genus in genuses)
            {
                result.AddRange(GetAllSpeciesInGenus(genus));
            }
            return result.OrderBy(species => $"{species.GenusId} {species.SpeciesId}").ToList();
        }
        internal List<Species> GetAllSpeciesInGenus(Classification genus)
        {
            return Species.Where(o => o.GenusId == genus.Id).OrderBy(species => $"{species.GenusId} {species.SpeciesId}").ToList();
        }

        public List<Species> GetAllSpecies(List<MapRegion> selectedRegions)
        {
            var bats = GetAllSpecies();
            return Filter(bats, selectedRegions);
        }


        internal List<Species> GetAllSpecies(Classification genus, List<MapRegion> selectedRegions)
        {
            var bats = GetAllSpeciesInGenus(genus);
            return Filter(bats, selectedRegions);
        }
        private string LoadIntroduction()
        {
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile("Data.Introduction.introduction.html"))
                {
                    if (stream == null)
                    {
                        throw new BusinessException("Introduction file does not exist");
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(result))
                        {
                            throw new BusinessException($"No data inside Introduction file");
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading Introduction file. {ex.Message}");
            }
        }

        private Species LoadSpecies(string genusId, string speciesId)
        {
            var datasetFilename = $"{genusId.ToLower()}_{speciesId.ToLower()}_dataset.json";
            try
            {                
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.SpeciesDataSets.{datasetFilename}"))
                {
                    if (stream == null)
                    {
                        throw new BusinessException($"Dataset file [{datasetFilename}] does not exist");
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string speciesDatasetJson = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(speciesDatasetJson))
                        {
                            throw new BusinessException($"No data inside dataset file [{datasetFilename}]");
                        }
                        try
                        {
                            var species = JsonConvert.DeserializeObject<Species>(speciesDatasetJson);
                            return species;
                        }
                        catch (Exception ex)
                        {
                            throw new BusinessException($"JSON paring error in [{datasetFilename}] file. {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading dataset file [{datasetFilename}]. {ex.Message}");
            }
        }

        private string LoadAbout()
        {
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile("Data.About.about.html"))
                {
                    if (stream == null)
                    {
                        throw new BusinessException("About file does not exist");
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(result))
                        {
                            throw new BusinessException($"No data inside About file");
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading About file. {ex.Message}");
            }
        }
    }

    #region *// Famaly To species Characters
    #region *// Numerics
    public class ForeArmCharacter : CharacterNumericBase
    {
        public ForeArmCharacter(float min, float max) : base("Fore arm", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class ThreeP1TheeMetRatioCharacter : CharacterNumericBase
    {
        public ThreeP1TheeMetRatioCharacter(float min, float max) : base("3 P1/3-MET ratio", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ThreeP1TheeMetRatioCharacter);
        }
    }

    public class ThreeP1Character : CharacterNumericBase
    {
        public ThreeP1Character(float min, float max) : base("3 P1", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ThreeP1Character);
        }
    }

    public class OuterCanineWidthCharacter : CharacterNumericBase
    {
        public OuterCanineWidthCharacter(float min, float max) : base("Outer canine width", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class TailLengthCharacter : CharacterNumericBase
    {
        public TailLengthCharacter(float min, float max) : base("Tail length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class SnoutToVentLengthCharacter : CharacterNumericBase
    {
        public SnoutToVentLengthCharacter(float min, float max) : base("SnoutToVent length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is SnoutToVentLengthCharacter);
        }
    }

    public class EarLengthCharacter : CharacterNumericBase
    {
        public EarLengthCharacter(float min, float max) : base("Ear length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is EarLengthCharacter);
        }
    }

    public class FootCharacter : CharacterNumericBase
    {
        public FootCharacter(float min, float max) : base("Foot length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class TibiaCharacter : CharacterNumericBase
    {
        public TibiaCharacter(float min, float max) : base("Tibia length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class FaTibiaRatioCharacter : CharacterNumericBase
    {
        public FaTibiaRatioCharacter(float min, float max) : base("Fa/Tibia ratio", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class PenisLengthCharacter : CharacterNumericBase
    {
        public PenisLengthCharacter(float min, float max) : base("Penis length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class ThreeMetCharacter : CharacterNumericBase
    {
        public ThreeMetCharacter(float min, float max) : base("3-MET length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class FiveMetCharacter : CharacterNumericBase
    {
        public FiveMetCharacter(float min, float max) : base("5-MET length", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is ForeArmCharacter);
        }
    }

    public class WingspanCharacter : CharacterNumericBase
    {
        public WingspanCharacter(float min, float max) : base("Wing span", min, max) { }

        public override bool ExistsIn(List<CharacterBase> characters)
        {
            return characters.Exists(o => o is WingspanCharacter);
        }
    }

    #endregion

    #region *// Options
    public class GularPouchCharacter : CharacterEnumBase
    {
        public enum GularPouchEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public GularPouchEnum Key { get; set; }

        public GularPouchCharacter(GularPouchEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is GularPouchCharacter)
                {
                    if (Key == ((GularPouchCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static GularPouchCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (GularPouchEnum)promptIndex;
            return new GularPouchCharacter(key);
        }
    }

    public class VespadelusFacialSkinPigmentationCharacter : CharacterEnumBase
    {
        public enum VespadelusFacialSkinPigmentationEnum
        {
            Undefined, NoneToLightBrown, BrownToBlack, PinkToYellow, NoneToBrown
        }
        public static List<string> Prompts { get; set; } = new List<string>() { 
            "",
            "none (pink) to light brown",
            "brown to black",
            "pink to yellow",
            "none (pink) to brown"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "", "", "", ""
        };

        public VespadelusFacialSkinPigmentationEnum Key { get; set; }

        public VespadelusFacialSkinPigmentationCharacter(VespadelusFacialSkinPigmentationEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespadelusFacialSkinPigmentationCharacter)
                {
                    if (Key == ((VespadelusFacialSkinPigmentationCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespadelusFacialSkinPigmentationCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespadelusFacialSkinPigmentationEnum)promptIndex;
            return new VespadelusFacialSkinPigmentationCharacter(key);
        }
    }

    public class PipistrellusTragusCharacter : CharacterEnumBase
    {
        public enum PipistrellusTragusEnum
        {
            Undefined, BroadestAtBase, BroadestInMiddle
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Broadest in middle", "Broadest at base" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "",  ""
        };

        public PipistrellusTragusEnum Key { get; set; }

        public PipistrellusTragusCharacter(PipistrellusTragusEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is PipistrellusTragusCharacter)
                {
                    if (Key == ((PipistrellusTragusCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static PipistrellusTragusCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (PipistrellusTragusEnum)promptIndex;
            return new PipistrellusTragusCharacter(key);
        }
    }
    public class ChalinolobusFurPatternAndColourCharacter : CharacterEnumBase
    {
        public enum ChalinolobusFurPatternAndColourEnum
        {
            Undefined, 
            Brown, 
            BlackWithLaterStripesWingStructure,
            BrownOrGrey,
            BlackOrGrey,
            BlackWithLaterStripes
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", 
            "Brown body, merging to darker fur over the head",
            "Black, white lateral stripes on ventral surface along wing juncture",
            "Brown or grey-brown",
            "Black to grey, often with light frosting. No white lateral stripes",
            "Black, white lateral stripes on ventral surface"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "","","","","",""
        };

        public ChalinolobusFurPatternAndColourEnum Key { get; set; }

        public ChalinolobusFurPatternAndColourCharacter(ChalinolobusFurPatternAndColourEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is ChalinolobusFurPatternAndColourCharacter)
                {
                    if (Key == ((ChalinolobusFurPatternAndColourCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static ChalinolobusFurPatternAndColourCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (ChalinolobusFurPatternAndColourEnum)promptIndex;
            return new ChalinolobusFurPatternAndColourCharacter(key);
        }
    }
    public class VespertilionidaeUpperCannesCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeUpperCannesEnum
        {
            Undefined,
            Smooth,
            Groove
        }
        public static List<string> Prompts { get; set; } = new List<string>() {
            "",
            "Anterior surface smooth",
            "Anterior surface with longitudinal groove"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "",
            "",
            ""
        };

        public VespertilionidaeUpperCannesEnum Key { get; set; }

        public VespertilionidaeUpperCannesCharacter(VespertilionidaeUpperCannesEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeUpperCannesCharacter)
                {
                    if (Key == ((VespertilionidaeUpperCannesCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeUpperCannesCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeUpperCannesEnum)promptIndex;
            return new VespertilionidaeUpperCannesCharacter(key);
        }
    }
    public class VespertilionidaeUpperPreMolarsCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeUpperPreMolarsEnum
        {
            Undefined,
            OnePair,
            TwoPair,
            TwoTriplets
        }
        public static List<string> Prompts { get; set; } = new List<string>() {
            "",
            "1 pair",
            "2 pairs",
            "2 triplets"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "",
            "pre_molars_one_pair.jpg",
            "upper_pre_molars_two.jpg",
            ""
        };

        public VespertilionidaeUpperPreMolarsEnum Key { get; set; }

        public VespertilionidaeUpperPreMolarsCharacter(VespertilionidaeUpperPreMolarsEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeUpperPreMolarsCharacter)
                {
                    if (Key == ((VespertilionidaeUpperPreMolarsCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeUpperPreMolarsCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeUpperPreMolarsEnum)promptIndex;
            return new VespertilionidaeUpperPreMolarsCharacter(key);
        }
    }
    public class VespertilionidaeTragusShapeCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeTragusShapeEnum
        {
            Undefined,
            Orbicular,
            OblongLanceolate,
            Linear
        }
        public static List<string> Prompts { get; set; } = new List<string>() { 
            "",
            "orbicular",
            "oblong lanceolate",
            "linear"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "",
            "tragus_orbicular.jpg",
            "tragus_oblong_lanceolate.jpg",
            "tragus_linear.jpg"
        };

        public VespertilionidaeTragusShapeEnum Key { get; set; }

        public VespertilionidaeTragusShapeCharacter(VespertilionidaeTragusShapeEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeTragusShapeCharacter)
                {
                    if (Key == ((VespertilionidaeTragusShapeCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeTragusShapeCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeTragusShapeEnum)promptIndex;
            return new VespertilionidaeTragusShapeCharacter(key);
        }
    }
    public class VespertilionidaeUpperIncisorsCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeUpperIncisorsEnum
        {
            Undefined, 
            TwoPairsInequalLargerToothBifid,
            TwoPairsInequalNeitherBifid,
            TwoPairsEqualNeitherBifid,
            TwoPairsEqualBothBifid,
            OnePair
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "",
            "2 pairs, inequal size, larger tooth bifid",
            "2 pairs, inequal size, neither bifid",
            "2 pairs, ± equal size, neither bifid",
            "2 pairs, ± equal size, both bifid",
            "1 pair"
        };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "",
            "2_pairs_larger_bifid.jpg",
            "2_pairs_inequal_size_neither_bifid.jpg",
            "",
            "2_pairs_equal_size_both_bifurcate.jpg",
            "1_pair.jpg"
        };

        public VespertilionidaeUpperIncisorsEnum Key { get; set; }

        public VespertilionidaeUpperIncisorsCharacter(VespertilionidaeUpperIncisorsEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeUpperIncisorsCharacter)
                {
                    if (Key == ((VespertilionidaeUpperIncisorsCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeUpperIncisorsCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeUpperIncisorsEnum)promptIndex;
            return new VespertilionidaeUpperIncisorsCharacter(key);
        }
    }
    public class VespertilionidaeLowerEarMarginCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeLowerEarMarginEnum
        {
            Undefined, NoLobes, SmallLobe
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "No lobes or other appendages", "Small lobe on lower margin and lobe at the corner of the mouth"};
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "lower_ear_margin_without_lobes.jpg",  "lobe-on-lower-margin-and-cnr-mouth.jpg"
        };

        public VespertilionidaeLowerEarMarginEnum Key { get; set; }

        public VespertilionidaeLowerEarMarginCharacter(VespertilionidaeLowerEarMarginEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeLowerEarMarginCharacter)
                {
                    if (Key == ((VespertilionidaeLowerEarMarginCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeLowerEarMarginCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeLowerEarMarginEnum)promptIndex;
            return new VespertilionidaeLowerEarMarginCharacter(key);
        }
    }
    public class VespertilionidaeNostrilShapeCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeNostrilShapeEnum
        {
            Undefined, Flat, Extended
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Flat or with slight rim", "Extended into distinct tubes"};
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "nostrils_flat_slight_rim.jpg",  "nostrils_distinct_tubes.jpg"
        };

        public VespertilionidaeNostrilShapeEnum Key { get; set; }

        public VespertilionidaeNostrilShapeCharacter(VespertilionidaeNostrilShapeEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeNostrilShapeCharacter)
                {
                    if (Key == ((VespertilionidaeNostrilShapeCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeNostrilShapeCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeNostrilShapeEnum)promptIndex;
            return new VespertilionidaeNostrilShapeCharacter(key);
        }
    }
    public class VespertilionidaeTailMembraneCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeTailMembraneEnum
        {
            Undefined, NoFur, Hairy
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Upper surface with sparse fur or naked", "Upper surface with obvious fur" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "tail_membrane_no_fur.jpg",  "tail_membrane_hairy.jpg"
        };

        public VespertilionidaeTailMembraneEnum Key { get; set; }

        public VespertilionidaeTailMembraneCharacter(VespertilionidaeTailMembraneEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeTailMembraneCharacter)
                {
                    if (Key == ((VespertilionidaeTailMembraneCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeTailMembraneCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeTailMembraneEnum)promptIndex;
            return new VespertilionidaeTailMembraneCharacter(key);
        }
    }
    public class VespertilionidaeMuzzleStructureCharacter : CharacterEnumBase
    {
        public enum VespertilionidaeMuzzleStructureEnum
        {
            Undefined, Smooth, NotSmooth
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Smooth, without bare skin folds ( furred ridge may be present)", "Small transverse, projecting leaf immediately above nostrils" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "muzzle_smooth.jpg",  "muzzle_projecting_leaf_above_nostrils.jpg"
        };

        public VespertilionidaeMuzzleStructureEnum Key { get; set; }

        public VespertilionidaeMuzzleStructureCharacter(VespertilionidaeMuzzleStructureEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is VespertilionidaeMuzzleStructureCharacter)
                {
                    if (Key == ((VespertilionidaeMuzzleStructureCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static VespertilionidaeMuzzleStructureCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (VespertilionidaeMuzzleStructureEnum)promptIndex;
            return new VespertilionidaeMuzzleStructureCharacter(key);
        }
    }
    public class HiposideridaeFacialFeaturesCharacter : CharacterEnumBase
    {
        public enum HiposideridaeFacialFeaturesEnum
        {
            Undefined, Rhino_aur, Dory_semoni, Dory_stenotis, Hipp_ater, Hipp_cerv, Hipp_diad, Hipp_inornat
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Rhino_aur", "Dory_semoni", "Dory_stenotis", "Hipp_ater", "Hipp_cerv", "Hipp_diad", "Hipp_inornat" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "rhino_aur_facial.jpg", "dory_semoni_facial.jpg", "dory_stenotis_facial.jpg", "hipp_ater_facial.jpg", "hipp_cerv_facial.jpg", "hipp_diad_facial.jpg", "hipp_inornat_facial.jpg"
        };

        public HiposideridaeFacialFeaturesEnum Key { get; set; }

        public HiposideridaeFacialFeaturesCharacter(HiposideridaeFacialFeaturesEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is HiposideridaeFacialFeaturesCharacter)
                {
                    if (Key == ((HiposideridaeFacialFeaturesCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static HiposideridaeFacialFeaturesCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (HiposideridaeFacialFeaturesEnum)promptIndex;
            return new HiposideridaeFacialFeaturesCharacter(key);
        }
    }
    public class GularPouchFemaleCharacter : CharacterEnumBase
    {

        public enum GularPouchFemaleEnum
        {
            Undefined, Absent, AbsentOrRudimentary, PresentOrRudimentary
        }

        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is absent or rudimentary", "Is present or rudimentary" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public GularPouchFemaleEnum Key { get; set; }

        public GularPouchFemaleCharacter(GularPouchFemaleEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is GularPouchFemaleCharacter)
                {
                    if (Key == ((GularPouchFemaleCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static GularPouchFemaleCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (GularPouchFemaleEnum)promptIndex;
            return new GularPouchFemaleCharacter(key);
        }
    }
    public class GularPouchMaleCharacter : CharacterEnumBase
    {

        public enum GularPouchMaleEnum
        {
            Undefined, Absent, Present
        }

        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public GularPouchMaleEnum Key { get; set; }

        public GularPouchMaleCharacter(GularPouchMaleEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is GularPouchMaleCharacter)
                {
                    if (Key == ((GularPouchMaleCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static GularPouchMaleCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (GularPouchMaleEnum)promptIndex;
            return new GularPouchMaleCharacter(key);
        }
    }
    public class MetacarpalWingPouchCharacter : CharacterEnumBase
    {
        public enum MetacarpalWingPouchEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public MetacarpalWingPouchEnum Key { get; set; }

        public MetacarpalWingPouchCharacter(MetacarpalWingPouchEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is MetacarpalWingPouchCharacter)
                {
                    if (Key == ((MetacarpalWingPouchCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static MetacarpalWingPouchCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (MetacarpalWingPouchEnum)promptIndex;
            return new MetacarpalWingPouchCharacter(key);
        }
    }
    public class BothSexesWithFleshyGenitalProjectionsCharacter : CharacterEnumBase
    {
        public enum BothSexesWithFleshyGenitalProjectionsEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public BothSexesWithFleshyGenitalProjectionsEnum Key { get; set; }

        public BothSexesWithFleshyGenitalProjectionsCharacter(BothSexesWithFleshyGenitalProjectionsEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is BothSexesWithFleshyGenitalProjectionsCharacter)
                {
                    if (Key == ((BothSexesWithFleshyGenitalProjectionsCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static BothSexesWithFleshyGenitalProjectionsCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (BothSexesWithFleshyGenitalProjectionsEnum)promptIndex;
            return new BothSexesWithFleshyGenitalProjectionsCharacter(key);
        }
    } 
    #endregion

    #endregion

    #region *// Root to Family Key Characters
    public class TailPresentCharacter : CharacterEnumBase
    {
        public enum TailPresentEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

        public TailPresentEnum Key { get; set; }

        public TailPresentCharacter(TailPresentEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is TailPresentCharacter)
                {
                    if (Key == ((TailPresentCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TailPresentCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TailPresentEnum)promptIndex;
            return new TailPresentCharacter(key);
        }
    }

    public class TailMembraneStructureCharacter : CharacterEnumBase
    {

        public enum TailMembraneStructureEnum
        {
            Undefined, Absent, PresentNotAttached, PresentFullyEnclosed, PresentProjectingThrough, PresentProjectingFree
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Present, not attached to tail membrane", "Present, fully enclosed in membrane", "Present, projecting through upper surface of tail membrane", "Present, projecting free foe > 8mm past tail membrane" };

        public TailMembraneStructureEnum Key { get; set; }

        public TailMembraneStructureCharacter(TailMembraneStructureEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is TailMembraneStructureCharacter)
                {
                    if (Key == ((TailMembraneStructureCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TailMembraneStructureCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TailMembraneStructureEnum)promptIndex;
            return new TailMembraneStructureCharacter(key);
        }
    }

    public class SecondFingerClawCharacter : CharacterEnumBase
    {

        public enum SecondFingerClawEnum
        {
            Undefined, Present, Absent
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Present", "Not present" };

        public static List<string> ShortPrompts { get; set; } = new List<string>() { "", "Present", "Not present" };

        public SecondFingerClawEnum Key { get; set; }

        public SecondFingerClawCharacter(SecondFingerClawEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is SecondFingerClawCharacter)
                {
                    if (Key == ((SecondFingerClawCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static SecondFingerClawCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (SecondFingerClawEnum)promptIndex;
            return new SecondFingerClawCharacter(key);
        }
    }

    public class FaceStructureNoseLeafCharacter : CharacterEnumBase
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

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "None, face simple",
            "Large complex horse-shoe",
            "Large complex flattened",
            "Small transverse leaf"
        };

        public FaceStructureNoseLeafEnum Key { get; set; }

        public FaceStructureNoseLeafCharacter(FaceStructureNoseLeafEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is FaceStructureNoseLeafCharacter)
                {
                    if (Key == ((FaceStructureNoseLeafCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }
        internal static FaceStructureNoseLeafCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (FaceStructureNoseLeafEnum)promptIndex;
            return new FaceStructureNoseLeafCharacter(key);
        }
    }

    public class WingThirdFingerCharacter : CharacterEnumBase
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

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "Phalage < 3x",
            "Phalage > 3x"
        };

        public WingThirdFingerEnum Key { get; set; }

        public WingThirdFingerCharacter(WingThirdFingerEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is WingThirdFingerCharacter)
                {
                    if (Key == ((WingThirdFingerCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static WingThirdFingerCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (WingThirdFingerEnum)promptIndex;
            return new WingThirdFingerCharacter(key);
        }
    }

    public class TragusCharacter : CharacterEnumBase
    {

        public enum TragusEnum
        {
            Undefined, Absent, Present, AlmostAbsent
        }
        public static List<string> Prompts { get; set; } = new List<string>() {
            "",
            "Absent",
            "Present, deeply bifurcate",
            "Bifurcate (divided into two)"
        };

        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "tragus_absent.jpg",  "tragus_entire.jpg", "tragus_bifurcate.jpg"
        };

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "Absent",
            "Deeply bifurcate",
            "Absent or entire"
        };

        public TragusEnum Key { get; set; }

        public TragusCharacter(TragusEnum key)
        {
            Key = key;
        }

        public override bool ExistsIn(List<CharacterBase> Characters)
        {
            foreach (var Character in Characters)
            {
                if (Character is TragusCharacter)
                {
                    if (Key == ((TragusCharacter)Character).Key) return true;
                }
            }
            return false;
        }

        public override string GetPrompt()
        {
            return Prompts[(int)Key];
        }

        internal static TragusCharacter CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TragusEnum)promptIndex;
            return new TragusCharacter(key);
        }
    }
    #endregion

    #region *// Sightings
    public class Sighting
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int MapRegionId { get; set; }
        public string GenusId { get; set; }
        public string SpeciesId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
    #endregion
}
