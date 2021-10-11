using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApp.Helpers;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTree
    {
        public static KeyTreeNode RootNode { get; set; }

        public List<PickerCharacter> PickerCharacters = new List<PickerCharacter>();

        public class KeyTreeNode
        {
            public string NodeId { get; set; }
            public KeyTreeNode Parent { get; set; }
            public List<KeyTreeNode> Children { get; set; } = new List<KeyTreeNode>();

            public List<CharacterBase> Characters = new List<CharacterBase>();

            public List<int> RegionIds = new List<int>();

        }

        public void LoadTreeFromKeyTables()
        {
            RootNode = LoadTree(null, null, "family");

            KeyTreeNode LoadTree(KeyTreeNode parentTreeNode, KeyTable parentKeyTable, string nodeId)
            {
                var treeNode = new KeyTreeNode { NodeId = nodeId, Parent = parentTreeNode };
                treeNode.Characters = GenerateCharacters(parentKeyTable, nodeId);
                var treeNodeKeyTable = KeyTable.Load(nodeId);
                if (treeNodeKeyTable == null) return treeNode;
                var subNodeIds = treeNodeKeyTable.NodeRows.Select(o => o.NodeId);
                foreach (var subNodeId in subNodeIds)
                {
                    var childNode = LoadTree(treeNode, treeNodeKeyTable, subNodeId);
                    treeNode.Children.Add(childNode);
                }
                return treeNode;
            }
        }

        public class CharacterBase { }

        private List<CharacterBase> GenerateCharacters(KeyTable parentKeyTable, string nodeId)
        {
            List<CharacterBase> characters = new List<CharacterBase>();
            if (parentKeyTable == null) return characters;

            var nodeRow = parentKeyTable.NodeRows.FirstOrDefault(o => o.NodeId == nodeId);
            foreach (var keyId in parentKeyTable.KeyIds)
            {
                var rowColumnIndex = parentKeyTable.KeyIds.IndexOf(keyId);
                var picker = parentKeyTable.Pickers.FirstOrDefault(o => o.Id == keyId);
                if (picker != null)
                {
                    // RowItem is a picker
                    characters.Add(new PickerCharacterValue(parentKeyTable.NodeId, keyId, nodeRow.Values[rowColumnIndex]));
                    if (!PickerCharacters.Exists(o=>o.keyTableNodeId == parentKeyTable.NodeId && o.keyId == keyId))
                    {                       
                        var newPickerCharacterDefinition = new PickerCharacter { 
                            keyTableNodeId = parentKeyTable.NodeId, 
                            keyId = keyId,
                            Options = GenerateOptions(picker.OptionIds, picker.OptionPrompts) };
                        PickerCharacters.Add(newPickerCharacterDefinition);
                    }
                }
                else
                {
                    // RowItem is a numeric
                    (float minValue, float maxValue) values = ExtractValues(nodeRow.Values[rowColumnIndex]);
                    characters.Add(new NumericCharacter(parentKeyTable.NodeId, keyId, values.minValue, values.maxValue));
                }

                (float minValue, float maxValue) ExtractValues(string value)
                {
                    var numbers = value.Split('-');
                    float minValue;
                    float maxValue;
                    if (!float.TryParse(numbers[0], out minValue))
                    {
                        throw new BusinessException($"Unable to extract float value from [{value}]");
                    }
                    if (!float.TryParse(numbers[1], out maxValue))
                    {
                        throw new BusinessException($"Unable to extract float value from [{value}]");
                    }
                    return (minValue, maxValue);
                }
            }
            return characters;

            List<PickerCharacter.Option> GenerateOptions(List<string> optionIds, List<string> optionPrompts)
            {
                var result = new List<PickerCharacter.Option>();
                for (int i = 0; i < optionIds.Count; i++)
                {
                    result.Add(new PickerCharacter.Option { OptionId = optionIds[i], OptionPrompt = optionPrompts[i]});
                }
                return result;
            }
        }

        public class PickerCharacterValue : CharacterBase
        {
            public string keyTableNodeId;
            public string keyId;
            public string optionId;

            public PickerCharacterValue(string keyTableNodeId, string keyId, string optionId)
            {
                this.keyTableNodeId = keyTableNodeId;
                this.keyId = keyId;
                this.optionId = optionId;
            }
        }

        public class PickerCharacter
        {
            public string keyTableNodeId;
            public string keyId;
            public List<Option> Options = new List<Option>();

            public class Option
            {
                public string OptionId { get; set; }
                public string OptionPrompt { get; set; }
            }

        }

        public class NumericCharacter : CharacterBase
        {
            public string keyTableNodeId;
            public string keyId;
            public float minValue;
            public float maxValue;

            public NumericCharacter(string keyTableNodeId, string keyId, float minValue, float maxValue)
            {
                this.keyTableNodeId = keyTableNodeId;
                this.keyId = keyId;
                this.minValue = minValue;
                this.maxValue = maxValue;
            }
        }

        #region *// Helpers

        public KeyTreeNode GetKeyNode(string nodeId)
        {
            KeyTreeNode node = null;
            var traverser = new KeyTreeTraverser((parent, current, level) =>
            {
                Debug.WriteLine($"Parent: {parent?.NodeId} Current: {current.NodeId} Level: {level}");
                if (current.NodeId == nodeId)
                {
                    node = current;
                    return KeyTreeTraverser.ExitAction.ExitImmediately;
                }

                return KeyTreeTraverser.ExitAction.Continue;
            }, RootNode);
            traverser.Execute();
            return node;
        }

        public List<PickerCharacter.Option> GetPickerOptions(string keyTableNodeId, string keyId)
        {
            var picker = PickerCharacters.FirstOrDefault(o => o.keyTableNodeId == keyTableNodeId && o.keyId == keyId);
            if (picker == null) return new List<PickerCharacter.Option>();
            return picker.Options;
        }
        public class KeyTreeTraverser
        {
            KeyTreeNode startNode;
            Func<KeyTreeNode, KeyTreeNode, int, ExitAction> processNode;
            public enum ExitAction { ExitImmediately, Continue };
            public KeyTreeTraverser(Func<KeyTreeNode, KeyTreeNode, int, ExitAction> processNode, KeyTreeNode startNode = null)
            {
                if (startNode == null) { this.startNode = KeyTree.RootNode; } else { this.startNode = startNode; }
                this.startNode = startNode;
                this.processNode = processNode;
            }

            int level = 0;
            public void Execute()
            {
                RecursiveExecute(startNode);

                bool RecursiveExecute(KeyTreeNode node)
                {
                    level++;
                    if (processNode.Invoke(node.Parent, node, level) == ExitAction.ExitImmediately) return true;
                    foreach (var child in node.Children)
                    {
                        if (RecursiveExecute(child)) return true; ;
                    }
                    level--;
                    return false;
                }
            }
        }

        internal void EnhanceTree(List<Species> specieses)
        {
            #region *// Enter leave node data
            foreach (var species in specieses)
            {
                var nodeId = $"{species.GenusId.UppercaseFirstChar()} {species.SpeciesId.ToLower()}";
                var node = GetKeyNode(nodeId);
                if (node == null) throw new BusinessException($"Could not find species [{nodeId}] in key tree");

                #region *// RegionIds
                node.RegionIds = species.RegionIds;
                #region *// Ripple regionIds to root node
                var nodeParent = node.Parent;
                while (nodeParent != null)
                {
                    nodeParent.RegionIds.AddRangeUnique<int>(node.RegionIds);
                    nodeParent = nodeParent.Parent;
                }
                #endregion 
                #endregion
            }
            #endregion


        }
        #endregion
    }
}
