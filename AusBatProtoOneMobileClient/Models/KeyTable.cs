using AusBatProtoOneMobileClient.Helpers;
using Mobile.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTable
    {
        public string NodeId { get; set; }

        public List<string> KeyIds { get; set; } = new List<string>();

        public List<Picker> Pickers { get; set; } = new List<Picker>();

        public List<NodeRow> NodeRows { get; set; } = new List<NodeRow>();


        public static KeyTable Load(string filename)
        {
            try
            {
                using (Stream stream = FileHelper.GetStreamFromFile($"Data.KeyTables.{filename.ToLower()}_keytable.json"))
                {
                    if (stream == null)
                    {
                        Debug.WriteLine($"KeyTable for [{filename}] does not exist");
                        return new KeyTable();
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string keyTableJson = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(keyTableJson))
                        {
                            throw new BusinessException($"No data inside keytable file for [{filename}]");
                        }
                        try
                        {
                            return JsonConvert.DeserializeObject<KeyTable>(keyTableJson);
                        }
                        catch (System.Exception ex)
                        {
                            throw new BusinessException($"Problem parsing keytable file for [{filename}]. {ex.Message}");
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem reading keytable file for [{filename}]. {ex.Message}");
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
