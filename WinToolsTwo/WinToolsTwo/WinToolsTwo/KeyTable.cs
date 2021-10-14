using Mobile.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTable
    {
        static string sourceDatasetsPathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesDataSets";
        static string destDatasetsPathFullname = @"C:\P\AustBatsOriginal\KeyTables";
        public string NodeId { get; set; }

        public List<string> KeyIds { get; set; } = new List<string>();

        public List<Picker> Pickers { get; set; } = new List<Picker>();

        public List<NodeRow> NodeRows { get; set; } = new List<NodeRow>();

        private static string filename;

        public static KeyTable Load(string parentKeyTableId, string filename)
        {
            KeyTable.filename = filename;
            string keyTableJson = File.ReadAllText(Path.Combine(sourceDatasetsPathFullname, filename));
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

        internal void PrintData()
        {
            Debug.Write($"KeyTable: {KeyTable.filename} references from [{NodeId}] to [");
            foreach (var nodeRow in NodeRows)
            {
                Debug.Write($"{nodeRow.NodeId},");
            }
            Debug.WriteLine("]");
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
        public List<string> OptionImages { get; set; } = new List<string>();
    }

}
