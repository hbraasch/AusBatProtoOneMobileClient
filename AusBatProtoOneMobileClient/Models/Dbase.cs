using AusBatProtoOneMobileClient.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TreeApp.Helpers;
using Xamarin.Essentials;

namespace AusBatProtoOneMobileClient.Models
{
    public class Dbase
    {
        const string DBASE_FILENAME = "dbase.json";

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
            return JsonConvert.DeserializeObject<Dbase>(dbaseJson);
        }
        public static void Save(Dbase dbase)
        {
            var folderPath = Path.Combine(FileSystem.AppDataDirectory, "Library");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, DBASE_FILENAME);
            var dbaseJson = JsonConvert.SerializeObject(dbase);
            File.WriteAllText(filePath, dbaseJson);
        }

        public static void Clear()
        {
            Save(new Dbase());
        }
    }
}
