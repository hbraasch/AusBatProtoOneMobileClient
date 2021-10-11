using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTable
    {
        static string pathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\KeyTables";

        public string NodeId { get; set; }

        public List<string> KeyIds { get; set; } = new List<string>();

        public List<Picker> Pickers { get; set; } = new List<Picker>();

        public List<NodeRow> NodeRows { get; set; } = new List<NodeRow>();


        public static KeyTable Load(string filename)
        {
            try
            {
                if (!KeyTableExists(filename)) return null;
                var androidFilename = FormatToAndroidFilename(filename);
                var fullFilename = Path.Combine(pathFullname, $"{androidFilename}_keytable.json");


                using (StreamReader reader = new StreamReader(Path.Combine(pathFullname, $"{androidFilename}_keytable.json")))
                {
                    string keyTableJson = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(keyTableJson))
                    {
                        throw new ApplicationException($"No data inside keytable file for [{filename}]");
                    }
                    try
                    {
                        return JsonConvert.DeserializeObject<KeyTable>(keyTableJson);
                    }
                    catch (System.Exception ex)
                    {
                        throw new ApplicationException($"Problem parsing keytable file for [{filename}]. {ex.Message}");
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Problem reading keytable file for [{filename}]. {ex.Message}");
            }

            string FormatToAndroidFilename(string text)
            {
                var result = text.Replace("-", "_");
                result = result.Replace(" ", "_");
                return result.ToLower();

            }

            bool KeyTableExists(string filename)
            {
                var androidFilename = FormatToAndroidFilename(filename);
                var fullFilename = Path.Combine(pathFullname, $"{androidFilename}_keytable.json");
                return File.Exists(fullFilename);
            }
        }


    }

    public class NodeRow
    {
        public string NodeId { get; set; }
        public List<string> Values { get; set; } = new List<string>();
    }

    public class Picker
    {
        public string Id { get; set; }

        public List<string> OptionIds { get; set; } = new List<string>();
        public List<string> OptionPrompts { get; set; } = new List<string>();
    }

}
