﻿using AusBatProtoOneMobileClient.Data;
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
                    Characteristics = new List<CharacteristicBase> {  
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Absent), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.Absent),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Present),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.Absent)
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
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentNotAttached),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.Absent) } });
                dbase.Classifications.Add(new Classification { Id = "Macroderma", Type = Classification.ClassificationType.Genus, Parent = "Pteropodidae(Dobsonia)" });
                dbase.Classifications.Add(new Classification { Id = "Magna", Type = Classification.ClassificationType.Species, Parent = "Macroderma" });

                dbase.Classifications.Add(new Classification { Id = "Megadermatidae", Type = Classification.ClassificationType.Family , 
                    Characteristics = new List<CharacteristicBase> {  
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.Absent),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.LargeHorshoe),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.Present) } }); 
                dbase.Classifications.Add(new Classification { Id = "Dobsonia", Type = Classification.ClassificationType.Genus, Parent = "Megadermatidae" });
                dbase.Classifications.Add(new Classification { Id = "Gigas", Type = Classification.ClassificationType.Species, Parent = "Dobsonia" });


                dbase.Classifications.Add(new Classification { Id = "Rhinolophidae", Type = Classification.ClassificationType.Family, 
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.LargeHorshoe),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent) } });
                dbase.Classifications.Add(new Classification { Id = "Rhinolophus", Type = Classification.ClassificationType.Genus, Parent = "Rhinolophidae" });
                dbase.Classifications.Add(new Classification { Id = "Megaphyllus", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Robertsi", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });
                dbase.Classifications.Add(new Classification { Id = "Sp 'intermediate'", Type = Classification.ClassificationType.Species, Parent = "Rhinolophus" });



                dbase.Classifications.Add(new Classification { Id = "Hipposideridae", Type = Classification.ClassificationType.Family, ImageTag = "Hipp_family",
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present), 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.LargeFlattenned),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent) }
                }); 
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
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Long),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent) }
                });
                dbase.Classifications.Add(new Classification { Id = "Miniopterus", Type = Classification.ClassificationType.Genus, Parent = "Minioperidae" });
                dbase.Classifications.Add(new Classification { Id = "Australis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae bassanii", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae oceanensis", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });
                dbase.Classifications.Add(new Classification { Id = "Orianae orianae", Type = Classification.ClassificationType.Species, Parent = "Miniopterus" });



                dbase.Classifications.Add(new Classification { Id = "Emballonuridae", Type = Classification.ClassificationType.Family, ImageTag = "Embal_family",
                    Characteristics = new List<CharacteristicBase> { 
                        new TailPresentCharacteristic(TailPresentCharacteristic.TailPresentEnum.Present) , 
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentProjectingThrough),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent)  }
                });
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
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentProjectingFree),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.None),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent) }
                });
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
                        new TailMembraneStructureCharacteristic(TailMembraneStructureCharacteristic.TailMembraneStructureEnum.PresentFullyEnclosed),
                        new SecondFingerClawCharacteristic(SecondFingerClawCharacteristic.SecondFingerClawEnum.Absent),
                        new FaceStructureNoseLeafCharacteristic(FaceStructureNoseLeafCharacteristic.FaceStructureNoseLeafEnum.SmallTransverseLeaf),
                        new WingThirdFingerCharacteristic(WingThirdFingerCharacteristic.WingThirdFingerEnum.Short),
                        new TragusCharacteristic(TragusCharacteristic.TragusEnum.AlmostAbsent) }
                });
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
                dbase.Species.Add(LoadSpecies( "Scotorepens", "sp."));
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

    #region *// Family Key Characteristics
    public class TailPresentCharacteristic : CharacteristicEnumBase
    {
        public enum TailPresentEnum
        {
            Undefined, Absent, Present
        }
        public static List<string> Prompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ShortPrompts { get; set; } = new List<string>() { "", "Is absent", "Is present" };
        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "ic_bat.jpg",  "ic_bat.jpg"
        };

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

        public override string GetShortPrompt()
        {
            return ShortPrompts[(int)Key];
        }

        internal static TailPresentCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TailPresentEnum)promptIndex;
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

        public override string GetShortPrompt()
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

        public static List<string> ShortPrompts { get; set; } = new List<string>() { "", "Present", "Not present" };

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

        public override string GetShortPrompt()
        {
            return ShortPrompts[(int)Key];
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

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "None, face simple",
            "Large complex horse-shoe",
            "Large complex flattened",
            "Small transverse leaf"
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
        public override string GetShortPrompt()
        {
            return ShortPrompts[(int)Key];
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

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "Phalage < 3x",
            "Phalage > 3x"
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

        public override string GetShortPrompt()
        {
            return ShortPrompts[(int)Key];
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
            "Bifurcate (divided into two)"
        };

        public static List<string> ImageSources { get; set; } = new List<string>()
        {
            "", "tragus-absent.jpg",  "tragus_entire.jpg", "tragus_bifurcate.jpg"
        };

        public static List<string> ShortPrompts { get; set; } = new List<string>() {
            "",
            "Absent",
            "Deeply bifurcate",
            "Absent or entire"
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

        public override string GetShortPrompt()
        {
            return ShortPrompts[(int)Key];
        }

        internal static TragusCharacteristic CreateFromPrompt(string prompt)
        {
            var promptIndex = Prompts.IndexOf(prompt);
            var key = (TragusEnum)promptIndex;
            return new TragusCharacteristic(key);
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
