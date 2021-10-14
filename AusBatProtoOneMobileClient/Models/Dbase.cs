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
using static AusBatProtoOneMobileClient.Models.KeyTree;

namespace AusBatProtoOneMobileClient.Models
{
    public class Dbase
    {
        const string DBASE_FILENAME = " json";
        static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };


        public string IntroductionHtml;
        public string AboutHtml;
        public KeyTree KeyTree;
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
                dbase.MapRegions.Clear();
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

                #region *// KeyTree
                dbase.KeyTree = new KeyTree();
                dbase.KeyTree.LoadTreeFromKeyTables();
                #endregion

                dbase.Classifications.Clear();
                dbase.Classifications =  GenerateClassifications(dbase.KeyTree);
                #endregion


                #region *// Species details
                dbase.Species.Clear();
                foreach (var item in dbase.Classifications.Where(o=>o.Type == Classification.ClassificationType.Species))
                {
                    dbase.Species.Add(LoadSpecies(item.Parent, item.Id));
                }
                dbase.KeyTree.EnhanceTree(dbase.Species);


                foreach (var species in  dbase.Species)
                {
                    species.LoadDetails();
                    await species.LoadImages();
                    await species.LoadCalls();
                }

                #endregion




#if false
                var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Debug.WriteLine($"Writing files to path: {folderPath}");
                var batsCharsJson = JsonConvert.SerializeObject( Bats[0], Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, "batChars.json"), batsCharsJson); 
#endif

                Save(dbase);
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

        private List<Classification> GenerateClassifications(KeyTree keyTree)
        {
            List<Classification> classifications = new List<Classification>();
            var traverser = new KeyTreeTraverser((parent, current, level) =>
            {
                if (level == 2)
                {
                    if (!classifications.Exists(o => o.Id == current.NodeId && o.Parent == ""))
                    {
                        // Extract family
                        classifications.Add(new Classification { Id = current.NodeId, Parent = "", Type = Classification.ClassificationType.Family });
                    }           
                }
                else if (current is LeafKeyTreeNode leafNode)
                {
                    // Its a leaf node (extract genus and species)
                    #region *// Extract genus details by rippling upwards till family node found
                    if (!(classifications.Exists(o=>o.Type == Classification.ClassificationType.Genus && o.Id == leafNode.GenusId)))
                    {

                        KeyTreeNodeBase node = current;
                        KeyTreeNodeBase familyNode = null;
                        while (node != null)
                        {
                            if (node.Parent.NodeId == "Family")
                            {
                                familyNode = node;
                                break;
                            }
                            node = node.Parent;
                        }
                        if (familyNode == null) throw new BusinessException($"Could not find family node for {leafNode.GenusId}");
                        if (!classifications.Exists(o => o.Id == leafNode.GenusId && o.Parent == familyNode.NodeId))
                        {
                            classifications.Add(new Classification { Id = leafNode.GenusId, Parent = familyNode.NodeId, Type = Classification.ClassificationType.Genus }); 
                        }
                    }
                    #endregion
                    #region *// Extract species details and only add one if copies
                    if (!classifications.Exists(o=>o.Id == leafNode.SpeciesId && o.Parent == leafNode.GenusId))
                    {
                        classifications.Add(new Classification { Id = leafNode.SpeciesId, Parent = leafNode.GenusId, Type = Classification.ClassificationType.Species });
                    }
                    #endregion

                }
                return KeyTreeTraverser.ExitAction.Continue;
            });
            traverser.Execute();
            return classifications;
        }

        internal Species FindSpecies(string genusId, string speciesId)
        {
            return Species.FirstOrDefault(o => o.GenusId == genusId.ToLower() && o.SpeciesId == speciesId.ToLower());
        }

        public static List<Species> Filter(List<Species> specieses, List<MapRegion> selectedRegions)
        {
            var selectedRegionIds = selectedRegions.Select(o => o.Id);
            if (selectedRegions.IsEmpty()) return specieses;
            return specieses.Where(o => o.RegionIds.Intersect(selectedRegionIds).Count() > 0).ToList();
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
            var genusesInFamily = Classifications.Where(o => o.Parent == family.Id).ToList();
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
            return Species.Where(o => o.GenusId.ToLower() == genus.Id.ToLower()).OrderBy(species => $"{species.GenusId} {species.SpeciesId}").ToList();
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
                            Debug.WriteLine($"Filename: {datasetFilename} Genus: {species.GenusId}, Species: {species.SpeciesId} Name: {species.Name}");
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
