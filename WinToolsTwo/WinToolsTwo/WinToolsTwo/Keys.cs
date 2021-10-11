using Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WinToolsTwo.KeyTree;

namespace AusBatProtoOneMobileClient.Models
{
    public class Keys
    {
        private static Keys instance;
        public static Keys Current 
        { 
            get 
            {
                if (instance == null)
                {
                    instance = new Keys();
                    instance.LoadAllKeyTables();
                }
                return instance; 
            }
        }

        private List<KeyTable> KeyTables { get; set; } = new List<KeyTable>();

        private void LoadAllKeyTables()
        {
            KeyTables = GetKeyTables("Family");

            List<KeyTable> GetKeyTables(string nodeId)
            {
                var keyTables = new List<KeyTable>();
                var keyTable = KeyTable.Load(nodeId);
                var subNodeIds = keyTable.NodeRows.Select(o => o.NodeId);
                foreach (var subNodeId in subNodeIds)
                {
                    keyTables.AddRange(GetKeyTables(subNodeId));
                }
                keyTables.AddRange(keyTables);
                return keyTables;
            }
        }




        internal List<CharacterBase> GenerateCharacters(string nodeId)
        {
            List<CharacterBase> characters = new List<CharacterBase>();
            var keyTable = KeyTables.FirstOrDefault(o => o.NodeRows.Exists(row => row.NodeId == nodeId));
            if (keyTable == null)
            {
                throw new BusinessException($"Sub node id [{nodeId}] cannot be found in any keytable");
            }

            var nodeRow = keyTable.NodeRows.FirstOrDefault(o => o.NodeId == nodeId);
            foreach (var keyId in keyTable.KeyIds)
            {
                var rowColumnIndex = keyTable.KeyIds.IndexOf(keyId);
                if (keyTable.Pickers.Exists(o=>o.Id == keyId))
                {
                    // RowItem is a picker
                    characters.Add(new PickerCharacter(keyTable.NodeId, keyId, nodeRow.Values[rowColumnIndex]));
                }
                else
                {
                    // RowItem is a numeric
                    (float minValue, float maxValue) values = ExtractValues(nodeRow.Values[rowColumnIndex]);
                    characters.Add(new NumericCharacter(keyTable.NodeId, keyId, values.minValue, values.maxValue));
                }

                (float minValue, float maxValue) ExtractValues(string value)
                {
                    var numbers = value.Split('-');
                    float minValue;
                    float maxValue;
                    if (float.TryParse(numbers[0], out minValue))
                    {
                        throw new BusinessException($"Unable to extract float value from [{value}]");
                    }
                    if (float.TryParse(numbers[0], out maxValue))
                    {
                        throw new BusinessException($"Unable to extract float value from [{value}]");
                    }
                    return (minValue, maxValue);
                }
            }
            return characters;
        }


    }




}
