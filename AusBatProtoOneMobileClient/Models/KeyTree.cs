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
using static AusBatProtoOneMobileClient.Models.KeyTree.PickerCharacterPrompt;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTree
    {
        public KeyTreeNodeBase RootNode { get; set; }

        public List<PickerCharacterPrompt> PickerCharacters = new List<PickerCharacterPrompt>();

        public class KeyTreeNodeBase
        {
            public string NodeId { get; set; }
            public KeyTreeNode Parent { get; set; }
            public List<KeyTreeNodeBase> Children { get; set; } = new List<KeyTreeNodeBase>();

            public List<CharacterPromptBase> PromptCharactersForNextLevel = new List<CharacterPromptBase>();
            public List<CharacterTriggerBase> TriggerCharactersForSelf = new List<CharacterTriggerBase>();
            
            public List<int> RegionIds = new List<int>();
        }
        public class KeyTreeNode : KeyTreeNodeBase { }

        public class LeafKeyTreeNode : KeyTreeNodeBase
        {
            public string GenusId { get; set; }
            public string SpeciesId { get; set; }

            public LeafKeyTreeNode(KeyTreeNode parent, string nodeId)
            {
                Parent = parent;
                NodeId = nodeId;
                var ids = nodeId.Split(' ');
                GenusId = ids[0];
                for (int i = 1; i < ids.Length; i++)
                {
                    SpeciesId += ids[i] + " ";
                }
                SpeciesId = SpeciesId.Trim();
            }
        }

        public void LoadTreeFromKeyTables()
        {
            RootNode = LoadTree(null, null, "Family");
            KeyTreeTraverser.rootNode = RootNode;

            PrintKeyTree();

            KeyTreeNode LoadTree(KeyTreeNode parentTreeNode, KeyTable parentKeyTable, string nodeId)
            {
                var treeNode = new KeyTreeNode { NodeId = nodeId, Parent = parentTreeNode };
                var treeNodeKeyTable = KeyTable.Load(parentKeyTable?.NodeId??"Family", nodeId);
                treeNode.PromptCharactersForNextLevel = GeneratePromptCharacters(treeNodeKeyTable);
                treeNodeKeyTable.PrintData();
                if (treeNodeKeyTable == null) return treeNode;
                var subNodeIds = treeNodeKeyTable.NodeRows.Select(o => o.NodeId);
                foreach (var subNodeId in subNodeIds)
                {
                    KeyTreeNodeBase childNode;
                    if (IsLeafNode(subNodeId))
                    {
                        childNode = new LeafKeyTreeNode(treeNode, subNodeId);
                    }
                    else
                    {
                        childNode = LoadTree(treeNode, treeNodeKeyTable, subNodeId);
                    }
                    childNode.TriggerCharactersForSelf = GenerateTriggerCharacters(treeNodeKeyTable, childNode.NodeId);
                    treeNode.Children.Add(childNode);
                }
                return treeNode;
            }

            bool IsLeafNode(string subNodeId)
            {
                // Leaf node consists of <Genus><Space><SpeciesName>..<SpeciesName> 
                return (subNodeId.Split(' ').Length > 1);
            }
        }

        private List<CharacterPromptBase> GeneratePromptCharacters(KeyTable keyTable)
        {
            List<CharacterPromptBase> characters = new List<CharacterPromptBase>();
            foreach (var keyId in keyTable.KeyIds)
            {
                var keyIdsIndex = keyTable.KeyIds.IndexOf(keyId);
                var picker = keyTable.Pickers.FirstOrDefault(o => o.Id == keyId);
                if (picker != null)
                {
                    // RowItem is a picker
                    var newPickerCharacterPrompt = new PickerCharacterPrompt
                    {                      
                        Prompt = keyId,
                        Options = GenerateOptions(picker.OptionIds, picker.OptionPrompts)
                    };
                    characters.Add(newPickerCharacterPrompt);                   
                }
                else
                {
                    // RowItem is a numeric
                    characters.Add(new NumericCharacterPrompt { Prompt = keyId });
                }
            }
            return characters;
        }

        private List<CharacterTriggerBase> GenerateTriggerCharacters(KeyTable keyTable, string nodeId)
        {
            List<CharacterTriggerBase> characters = new List<CharacterTriggerBase>();
            if (keyTable == null) return characters;
            var nodeRow = keyTable.NodeRows.FirstOrDefault(o => o.NodeId == nodeId);
            foreach (var keyId in keyTable.KeyIds)
            {
                var rowColumnIndex = keyTable.KeyIds.IndexOf(keyId);
                var picker = keyTable.Pickers.FirstOrDefault(o => o.Id == keyId);
                if (picker != null)
                {
                    // RowItem is a picker
                    characters.Add(new PickerCharacterTrigger { OptionId = nodeRow.Values[rowColumnIndex] });
                }
                else
                {
                    // RowItem is a numeric
                    (float minValue, float maxValue) values = ExtractValues(nodeRow.Values[rowColumnIndex]);
                    characters.Add(new NumericCharacterTrigger { MinValue = values.minValue, MaxValue = values.maxValue });
                }
            }
            return characters;
        }

        private List<PickerCharacterPrompt.Option> GenerateOptions(List<string> optionIds, List<string> optionPrompts)
        {
            var result = new List<PickerCharacterPrompt.Option>();
            for (int i = 0; i < optionIds.Count; i++)
            {
                result.Add(new PickerCharacterPrompt.Option { OptionId = optionIds[i], OptionPrompt = optionPrompts[i] });
            }
            return result;
        }

        private (float minValue, float maxValue) ExtractValues(string value)
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

        public class CharacterTriggerBase { }
        public class CharacterPromptBase { 
            public string Prompt { get; set; }
        }




        public class PickerCharacterPrompt: CharacterPromptBase
        {
            public List<Option> Options = new List<Option>();

            public class Option
            {
                public string OptionId { get; set; }
                public string OptionPrompt { get; set; }
            }

        }
        public class PickerCharacterTrigger : CharacterTriggerBase
        {
            public string OptionId;

        }
        public class NumericCharacterPrompt : CharacterPromptBase
        {

        }
        public class NumericCharacterTrigger : CharacterTriggerBase
        {
            public float MinValue;
            public float MaxValue;
        }

        #region *// Helpers

        public KeyTreeNodeBase GetKeyNode(string nodeId)
        {
            KeyTreeNodeBase node = null;
            var traverser = new KeyTreeTraverser((parent, current, level) =>
            {
                if (current.NodeId == nodeId)
                {
                    node = current;
                    return KeyTreeTraverser.ExitAction.ExitImmediately;
                }

                return KeyTreeTraverser.ExitAction.Continue;
            });
            traverser.Execute();
            return node;
        }

        public void PrintKeyTree()
        {
            var traverser = new KeyTreeTraverser((parent, current, level) =>
            {
                Debug.WriteLine($"{Indent(level)}Parent = {parent?.NodeId} Current = {current.NodeId} Level: {level}");
                return KeyTreeTraverser.ExitAction.Continue;
            });
            traverser.Execute();

            string Indent(int totalLength)
            {
                string result = string.Empty;
                for (int i = 0; i < totalLength * 5; i++)
                {
                    result += " ";
                }
                return result;
            }
        }

        public class KeyTreeTraverser
        {
            public static KeyTreeNodeBase rootNode;
            Func<KeyTreeNodeBase, KeyTreeNodeBase, int, ExitAction> processNode;
            public enum ExitAction { ExitImmediately, Continue };

            public KeyTreeTraverser(Func<KeyTreeNodeBase, KeyTreeNodeBase, int, ExitAction> processNode)
            {
                if (rootNode == null) throw new ApplicationException("No start keytree node supplied");
                this.processNode = processNode;
            }

            int level = 0;
            public void Execute()
            {
                RecursiveExecute(rootNode);

                bool RecursiveExecute(KeyTreeNodeBase node)
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
