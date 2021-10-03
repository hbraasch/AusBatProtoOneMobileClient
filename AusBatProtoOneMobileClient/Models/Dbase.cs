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
        public List<Bat> Bats = new List<Bat>();
        public List<MapRegion> MapRegions = new List<MapRegion>();

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

        public void Init()
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
                    Characteristics = new List<CharacteristicBase> {  
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Absent), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.Absent)} });
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
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentNotAttached) } });
                dbase.Classifications.Add(new Classification { Id = "Macroderma", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae(Dobsonia)" });
                dbase.Classifications.Add(new Classification { Id = "Magna", Type = Classification.ClassificationType.Species, Parent = "Macroderma" });

                dbase.Classifications.Add(new Classification { Id = "Megadermatidae", Type = Classification.ClassificationType.Family , 
                    Characteristics = new List<CharacteristicBase> {  
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.Absent) } }); 
                dbase.Classifications.Add(new Classification { Id = "Dobsonia", Type = Classification.ClassificationType.Genus, Parent = "Megadermatidae" });
                dbase.Classifications.Add(new Classification { Id = "Gigas", Type = Classification.ClassificationType.Species, Parent = "Dobsonia" });


                dbase.Classifications.Add(new Classification { Id = "Rhinolophidae", Type = Classification.ClassificationType.Family, 
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed) } });
                dbase.Classifications.Add(new Classification { Id = "Rhinolophus", Type = Classification.ClassificationType.Genus, Parent = "Rhinolophidae" });
                dbase.Classifications.Add(new Classification { Id = "Megaphyllus", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Robertsi", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Sp 'intermediate'", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });



                dbase.Classifications.Add(new Classification { Id = "Hipposideridae", Type = Classification.ClassificationType.Family, ImageTag = "Hipp_family",
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed) }}); 
                dbase.Classifications.Add(new Classification { Id = "Hipposideros", Type = Classification.ClassificationType.Genus, Parent = "Hipposideridae" });
                dbase.Classifications.Add(new Classification { Id = "Ater", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Cervinus", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Diadema", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Inornatus", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Semoni", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });
                dbase.Classifications.Add(new Classification { Id = "Stenotis", Type = Classification.ClassificationType.Species, Parent = "Hipposideros" });



                dbase.Classifications.Add(new Classification { Id = "Minioperidae", Type = Classification.ClassificationType.Family,
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed) }});
                dbase.Classifications.Add(new Classification { Id = "Miniopterus", Type = Classification.ClassificationType.Genus, Parent = "Minioperidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae bassanii", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae oceanensis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae orianae", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });



                dbase.Classifications.Add(new Classification { Id = "Emballonuridae", Type = Classification.ClassificationType.Family, ImageTag = "Embal_family",
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present) , 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentProjectingThrough)  }});
                dbase.Classifications.Add(new Classification { Id = "Saccolaimus", Type = Classification.ClassificationType.Genus, Parent = "Emballonuridae" });
                dbase.Classifications.Add(new Classification { Id = "Flaviventris", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus" });
                dbase.Classifications.Add(new Classification { Id = "Mixtus", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus" });
                dbase.Classifications.Add(new Classification { Id = "Saccolaimus", Type = Classification.ClassificationType.Species, Parent = "Saccolaimus" });
                dbase.Classifications.Add(new Classification { Id = "Taphozous", Type = Classification.ClassificationType.Genus, Parent = "Emballonuridae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Taphozous" });
                dbase.Classifications.Add(new Classification { Id = "Georgianus", Type = Classification.ClassificationType.Species, Parent = "Taphozous" });
                dbase.Classifications.Add(new Classification { Id = "Hilli", Type = Classification.ClassificationType.Species, Parent = "Taphozous" });
                dbase.Classifications.Add(new Classification { Id = "Kapalgensis", Type = Classification.ClassificationType.Species, Parent = "Taphozous" });
                dbase.Classifications.Add(new Classification { Id = "Troughtoni", Type = Classification.ClassificationType.Species, Parent = "Taphozous" });



                dbase.Classifications.Add(new Classification { Id = "Molossidae", Type = Classification.ClassificationType.Family,
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentProjectingFree) }});
                dbase.Classifications.Add(new Classification { Id = "Austonomus", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Austonomus" });
                dbase.Classifications.Add(new Classification { Id = "Chaerephon", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Jobensis", Type = Classification.ClassificationType.Species, Parent = "Chaerephon" });
                dbase.Classifications.Add(new Classification { Id = "Micronomus", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Norfolkensis", Type = Classification.ClassificationType.Species, Parent = "Micronomus" });
                dbase.Classifications.Add(new Classification { Id = "Ozimops", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Cobourgianus", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Halli", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Kitcheneri", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Lumsdenae", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Petersi", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Planiceps", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Ridei", Type = Classification.ClassificationType.Species, Parent = "Ozimops" });
                dbase.Classifications.Add(new Classification { Id = "Setirostris", Type = Classification.ClassificationType.Genus, Parent = "Molossidae" });
                dbase.Classifications.Add(new Classification { Id = "Eleryi", Type = Classification.ClassificationType.Species, Parent = "Setirostris" });



                dbase.Classifications.Add(new Classification { Id = "Vespertilionidae", Type = Classification.ClassificationType.Family, ImageTag = "Vesp_family",
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed) }});
                dbase.Classifications.Add(new Classification { Id = "Chalinolobus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Dwyeri", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                dbase.Classifications.Add(new Classification { Id = "Gouldii", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                dbase.Classifications.Add(new Classification { Id = "Morio", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                dbase.Classifications.Add(new Classification { Id = "Nigrogriseus", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });
                dbase.Classifications.Add(new Classification { Id = "Picatus", Type = Classification.ClassificationType.Species, Parent = "Chalinolobus" });

                dbase.Classifications.Add(new Classification { Id = "Falsistrellus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Mackenziei", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });
                dbase.Classifications.Add(new Classification { Id = "Tasmaniensis", Type = Classification.ClassificationType.Species, Parent = "Falsistrellus" });

                dbase.Classifications.Add(new Classification { Id = "Murina", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Florium", Type = Classification.ClassificationType.Species, Parent = "Murina" });

                dbase.Classifications.Add(new Classification { Id = "Myotis", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Macropus", Type = Classification.ClassificationType.Species, Parent = "Myotis" });

                dbase.Classifications.Add(new Classification { Id = "Nyctophilus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
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

                dbase.Classifications.Add(new Classification { Id = "Phoniscus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Papuensis", Type = Classification.ClassificationType.Species, Parent = "Phoniscus" });

                dbase.Classifications.Add(new Classification { Id = "Pipistrellus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Adamsi", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus" });
                dbase.Classifications.Add(new Classification { Id = "Murrayi", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus" });
                dbase.Classifications.Add(new Classification { Id = "Westralis", Type = Classification.ClassificationType.Species, Parent = "Pipistrellus" });

                dbase.Classifications.Add(new Classification { Id = "Scoteanax", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Rueppellii", Type = Classification.ClassificationType.Species, Parent = "Scoteanax" });

                dbase.Classifications.Add(new Classification { Id = "Scotorepens", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Balstoni", Type = Classification.ClassificationType.Species, Parent = "Scotorepens" });
                dbase.Classifications.Add(new Classification { Id = "Greyii", Type = Classification.ClassificationType.Species, Parent = "Scotorepens" });
                dbase.Classifications.Add(new Classification { Id = "Orion", Type = Classification.ClassificationType.Species, Parent = "Scotorepens" });
                dbase.Classifications.Add(new Classification { Id = "Sanborni", Type = Classification.ClassificationType.Species, Parent = "Scotorepens" });
                dbase.Classifications.Add(new Classification { Id = "Sp. ", Type = Classification.ClassificationType.Species, Parent = "Scotorepens" });

                dbase.Classifications.Add(new Classification { Id = "Vespadalus", Type = Classification.ClassificationType.Genus, Parent = "Vespertilionidae" });
                dbase.Classifications.Add(new Classification { Id = "Caurinus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Douglasorum", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Finlaysoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Regulus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Baverstocki", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Darlingtoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Pumilus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Troughtoni", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                dbase.Classifications.Add(new Classification { Id = "Vulturnus", Type = Classification.ClassificationType.Species, Parent = "Vespadalus" });
                #endregion


                #region *// Bats
                dbase.Bats.Add(new Bat { GenusId = "Austonomus", SpeciesId = "australis", Name = "White-striped Freetail Bat", DataTag = "Aust_aust" });
                dbase.Bats.Add(new Bat { GenusId = "Chaerephon", SpeciesId = "jobensis", Name = "Northern Wrinkle-lipped Bat", DataTag = "Chae_job" });
                dbase.Bats.Add(new Bat { GenusId = "Chalinolobus", SpeciesId = "dwyeri", Name = "Large-eared Pied Bat", DataTag = "Chal_dwye" });
                dbase.Bats.Add(new Bat { GenusId = "Chalinolobus", SpeciesId = "gouldii", Name = "Gould's Wattled Bat", DataTag = "Chal_gould" });
                dbase.Bats.Add(new Bat { GenusId = "Chalinolobus", SpeciesId = "morio", Name = "Chocolate Wattled Bat", DataTag = "Chal_morio" });
                dbase.Bats.Add(new Bat { GenusId = "Chalinolobus", SpeciesId = "nigrogriseus", Name = "Hoary Wattled Bat", DataTag = "Chal_nigro" });
                dbase.Bats.Add(new Bat { GenusId = "Chalinolobus", SpeciesId = "picatus", Name = "Little Pied Bat", DataTag = "Chal_pic" });
                dbase.Bats.Add(new Bat { GenusId = "Dobsonia", SpeciesId = "magna", Name = "Bare-backed Fruit Bat", DataTag = "Dob_mag" });
                dbase.Bats.Add(new Bat { GenusId = "Falsistrellus", SpeciesId = "mackenziei", Name = "Western False Pipistrelle", DataTag = "Falsi_mack_" });
                dbase.Bats.Add(new Bat { GenusId = "Falsistrellus", SpeciesId = "tasmaniensis", Name = "Eastern False Pipistrelle", DataTag = "Falsi_tasman" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "ater", Name = "Dusky Leaf-nosed Bat", DataTag = "Hipp_ater" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "cervinus", Name = "Fawn Leaf-nosed Bat", DataTag = "Hipp_cerv" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "diadema", Name = "Diadem Leaf-nosed Bat", DataTag = "Hipp_diadem" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "inornatus", Name = "Arnhem Leaf-nosed Bat", DataTag = "Hipp_inorn" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "semoni", Name = "Semon's Leaf-nosed Bat", DataTag = "AAAAAAAAAAAAAAAAAAAAA" });
                dbase.Bats.Add(new Bat { GenusId = "Hipposideros", SpeciesId = "stenotis", Name = "Northern Leaf-nosed Bat", DataTag = "AAAAAAAAAAAAAAAAAAAAA" });
                dbase.Bats.Add(new Bat { GenusId = "Macroderma", SpeciesId = "gigas", Name = "Ghost Bat", DataTag = "Macro_gigas" });
                dbase.Bats.Add(new Bat { GenusId = "Macroglossus", SpeciesId = "minimus", Name = "Northern Blossom Bat", DataTag = "Macro_minimus" });
                dbase.Bats.Add(new Bat { GenusId = "Micronomus", SpeciesId = "norfolkensis", Name = "East Coast Free-tailed Bat", DataTag = "Micro_norfolk_" });
                dbase.Bats.Add(new Bat { GenusId = "Miniopterus", SpeciesId = "australis", Name = "Little Bent-winged Bat", DataTag = "Mini_aust_" });
                dbase.Bats.Add(new Bat { GenusId = "Miniopterus", SpeciesId = "orianae bassanii", Name = "Southern Bent-winged Bat", DataTag = "Mini_oc_bassani" });
                dbase.Bats.Add(new Bat { GenusId = "Miniopterus", SpeciesId = "orianae oceanensi", Name = "Eastern Bent-winged bat", DataTag = "Mini_or_ocean" });
                dbase.Bats.Add(new Bat { GenusId = "Miniopterus", SpeciesId = "orianae orianae", Name = "Northern Bent-winged Bat", DataTag = "Mini_or_orianae" });
                dbase.Bats.Add(new Bat { GenusId = "Murina", SpeciesId = "florium", Name = "Flute-nosed Bat", DataTag = "Murin_florium" });
                dbase.Bats.Add(new Bat { GenusId = "Myotis", SpeciesId = "macropus", Name = "Large-footed Myotis", DataTag = "Myot_mac" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctimene", SpeciesId = "robinsoni", Name = "Eastern Tube-nosed Bat", DataTag = "Nyct_robin" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "arnhemensis", Name = "Northern Long-eared Bat", DataTag = "Nyct_arnhem" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "bifax", Name = "Eastern Long-eared Bat", DataTag = "Nyct_bifax" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "corbeni", Name = "South-eastern Long-eared Bat", DataTag = "Nyct_corben" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "daedalus", Name = "Pallid Long-eared Bat", DataTag = "Nyct_daed" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "geoffroyi", Name = "Lesser Long-eared Bat", DataTag = "Nyct_geoff" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "gouldi", Name = "Gould's Long-eared Bat", DataTag = "Nyct_gould" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "major major", Name = "Western Long-eared Bat", DataTag = "Nyct_major" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "major tor", Name = "Central Long-eared Bat", DataTag = "Nyct_major_tor" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "sherrini", Name = "Tasmanian Long-eared Bat", DataTag = "Nyctophilus-sherrini" });
                dbase.Bats.Add(new Bat { GenusId = "Nyctophilus", SpeciesId = "walkeri", Name = "Pygmy Long-eared Bat", DataTag = "Nyct_walkeri" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "cobourgianus", Name = "Western Little Free-tailed Bat", DataTag = "Ozi_coburg" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "halli", Name = "Cape York Free-tailed Bat", DataTag = "Ozie_halli" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "kitcheneri", Name = "South-western Free-tailed Bat", DataTag = "Ozi_kitchen" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "lumsdenae", Name = "Northern Free-tail Bat", DataTag = "Ozi_lumsd" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "petersi", Name = "Inland Free-tailed Bat", DataTag = "Ozi_peters" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "planiceps", Name = "South-eastern Free-tailed Bat", DataTag = "Ozi_planic" });
                dbase.Bats.Add(new Bat { GenusId = "Ozimops", SpeciesId = "ridei", Name = "Eastern Free-tailed Bat", DataTag = "Ozie_ridei" });
                dbase.Bats.Add(new Bat { GenusId = "Phoniscus", SpeciesId = "papuensis", Name = "Golden-tipped Bat", DataTag = "Phoni_pap" });
                dbase.Bats.Add(new Bat { GenusId = "Pipistrellus", SpeciesId = "adamsi", Name = "Forest Pipistrelle", DataTag = "Pip_adams" });
                dbase.Bats.Add(new Bat { GenusId = "Pipistrellus", SpeciesId = "murrayi", Name = "Christmas Island Pipistrelle", DataTag = "AAAAAAAAAAAAAAAAAAAAA" });
                dbase.Bats.Add(new Bat { GenusId = "Pipistrellus", SpeciesId = "westralis", Name = "Northern Pipistrelle", DataTag = "Pip_west" });
                dbase.Bats.Add(new Bat { GenusId = "Pteropus", SpeciesId = "alecto", Name = "Black Flying-fox", DataTag = "Pterop_alect" });
                dbase.Bats.Add(new Bat { GenusId = "Pteropus", SpeciesId = "conspicillatus", Name = "Spectacled Flying-fox", DataTag = "Pterop_conspic" });
                dbase.Bats.Add(new Bat { GenusId = "Pteropus", SpeciesId = "natalis", Name = "Christmas Island Flying-fox", DataTag = "Pterop_natalis" });
                dbase.Bats.Add(new Bat { GenusId = "Pteropus", SpeciesId = "poliocephalus", Name = "Grey-headed Flying-fox", DataTag = "Pterop_polioc" });
                dbase.Bats.Add(new Bat { GenusId = "Pteropus", SpeciesId = "scapulatus", Name = "Little Red Flying-fox", DataTag = "Pterop_scap" });
                dbase.Bats.Add(new Bat { GenusId = "Rhinolophus", SpeciesId = "megaphyllus", Name = "Eastern Horseshoe-bat", DataTag = "Rhino_mega" });
                dbase.Bats.Add(new Bat { GenusId = "Rhinolophus", SpeciesId = "robertsi", Name = "Greater Large-eared Horseshoe-bat", DataTag = "Rhino_robert_" });
                dbase.Bats.Add(new Bat { GenusId = "Rhinolophus", SpeciesId = "sp 'intermediate'", Name = "Lesser Large-eared Horseshoe-bat", DataTag = "Rhino_sp" });
                dbase.Bats.Add(new Bat { GenusId = "Rhinonicteris", SpeciesId = "aurantia", Name = "Orange Leaf-nosed Bat", DataTag = "Rhinon_aurant" });
                dbase.Bats.Add(new Bat { GenusId = "Saccolaimus", SpeciesId = "flaviventris", Name = "Yellow-bellied Sheath-tailed Bat", DataTag = "Sacc_flav" });
                dbase.Bats.Add(new Bat { GenusId = "Saccolaimus", SpeciesId = "mixtus", Name = "Cape York Sheath-tailed Bat", DataTag = "Sacc_mixtus" });
                dbase.Bats.Add(new Bat { GenusId = "Saccolaimus", SpeciesId = "saccolaimus", Name = "Bare-rumped Sheath-tailed Bat", DataTag = "Sacc_sacc" });
                dbase.Bats.Add(new Bat { GenusId = "Scoteanax", SpeciesId = "rueppellii", Name = "Greater Broad-nosed Bat", DataTag = "Scot_ruepp" });
                dbase.Bats.Add(new Bat { GenusId = "Scotorepens", SpeciesId = "balstoni", Name = "Inland Broad-nosed Bat", DataTag = "Scot_balst" });
                dbase.Bats.Add(new Bat { GenusId = "Scotorepens", SpeciesId = "greyii", Name = "Little Broad-nosed Bat", DataTag = "Scot_greyi" });
                dbase.Bats.Add(new Bat { GenusId = "Scotorepens", SpeciesId = "orion", Name = "Eastern Broad-nosed Bat", DataTag = "Scot_orion" });
                dbase.Bats.Add(new Bat { GenusId = "Scotorepens", SpeciesId = "sanborni", Name = "Northern Broad-nosed Bat", DataTag = "Scot_sanborn" });
                dbase.Bats.Add(new Bat { GenusId = "Scotorepens", SpeciesId = "sp.", Name = "Central-eastern Broad-nosed Bat", DataTag = "AAAAAAAAAAAAAAAAAAAAA" });
                dbase.Bats.Add(new Bat { GenusId = "Setirostris", SpeciesId = "eleryi", Name = "Bristle-faced Free-tailed Bat", DataTag = "Seti_eleryi" });
                dbase.Bats.Add(new Bat { GenusId = "Syconycteris", SpeciesId = "australis", Name = "Eastern Blossom Bat", DataTag = "Sycon_aust" });
                dbase.Bats.Add(new Bat { GenusId = "Taphozous", SpeciesId = "australis", Name = "Coastal Sheath-tailed Bat", DataTag = "Taph_aust" });
                dbase.Bats.Add(new Bat { GenusId = "Taphozous", SpeciesId = "georgianus", Name = "Common Sheath-tailed Bat", DataTag = "Taph_georg" });
                dbase.Bats.Add(new Bat { GenusId = "Taphozous", SpeciesId = "hilli", Name = "Hill's Sheath-tailed Bat", DataTag = "Taph_hilli_" });
                dbase.Bats.Add(new Bat { GenusId = "Taphozous", SpeciesId = "kapalgensis", Name = "Arnhem Sheath-tailed Bat", DataTag = "Taph_kapalg" });
                dbase.Bats.Add(new Bat { GenusId = "Taphozous", SpeciesId = "troughtoni", Name = "Troughton's Sheath-tailed Bat", DataTag = "Taph_trought" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadalus", SpeciesId = "caurinus", Name = "Northern Cave Bat", DataTag = "Vesp_caur" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadalus", SpeciesId = "douglasorum", Name = "Yellow-lipped Cave Bat", DataTag = "Vesp_douglas" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadalus", SpeciesId = "finlaysoni", Name = "Finlayson's Cave Bat, Inland Cave Bat", DataTag = "Vesp_finlay" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadalus", SpeciesId = "regulus", Name = "Southern Forest Bat", DataTag = "Vesp_regulus" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadelus", SpeciesId = "baverstocki", Name = "Inland Forest Bat", DataTag = "Vesp_baver_" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadelus", SpeciesId = "darlingtoni", Name = "Large Forest Bat", DataTag = "Vesp_darling" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadelus", SpeciesId = "pumilus", Name = "Eastern Forest Bat", DataTag = "Vesp_pumilus" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadelus", SpeciesId = "troug;htoni", Name = "Eastern Cave Bat", DataTag = "Vesp_trought" });
                dbase.Bats.Add(new Bat { GenusId = "Vespadelus", SpeciesId = "vulturnus", Name = "Little Forest Bat", DataTag = "Vesp_vult_" });
                #endregion

                foreach (var bat in dbase.Bats)
                {
                    bat.LoadRegions(dbase);
                    bat.LoadDetails(dbase);
                    bat.LoadImages(dbase);
                    bat.LoadCalls(dbase);

                }


                var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Debug.WriteLine($"Writing files to path: {folderPath}");

#if false
                var classifJson = JsonConvert.SerializeObject(dbase.Classifications, Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, "classification.json"), classifJson);
                var batsJson = JsonConvert.SerializeObject(dbase.Bats, Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, "bats.json"), batsJson); 
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



        public static List<Bat> Filter(List<Bat> bats, List<MapRegion> selectedRegions)
        {
            if (selectedRegions.IsEmpty()) return bats;
            return bats.Where(o => o.MapRegions.Intersect(selectedRegions, new RegionComparer()).Count() > 0).ToList();
        }

        public static List<Bat> Order(List<Bat> bats)
        {
            return bats.OrderBy(bat => $"{bat.GenusId} {bat.SpeciesId}").ToList();
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

        internal List<Bat> GetAllSpecies()
        {
            return Bats.OrderBy(bat => $"{bat.GenusId} {bat.SpeciesId}").ToList();
        }

        internal List<Bat> GetAllSpeciesInFamily(Classification family)
        {
            var genusesInFamily = App.dbase.Classifications.Where(o => o.Parent == family.Id).ToList();
            return GetAllSpecies(genusesInFamily);
        }

        internal List<Bat> GetAllSpecies(List<Classification> genuses)
        {
            List<Bat> result = new List<Bat>();
            foreach (var genus in genuses)
            {
                result.AddRange(GetAllSpeciesInGenus(genus));
            }
            return result.OrderBy(bat => $"{bat.GenusId} {bat.SpeciesId}").ToList();
        }
        internal List<Bat> GetAllSpeciesInGenus(Classification genus)
        {
            return Bats.Where(o => o.GenusId == genus.Id).OrderBy(bat => $"{bat.GenusId} {bat.SpeciesId}").ToList();
        }

        public List<Bat> GetAllSpecies(List<MapRegion> selectedRegions)
        {
            var bats = GetAllSpecies();
            return Filter(bats, selectedRegions);
        }


        internal List<Bat> GetAllSpecies(Classification genus, List<MapRegion> selectedRegions)
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
}
