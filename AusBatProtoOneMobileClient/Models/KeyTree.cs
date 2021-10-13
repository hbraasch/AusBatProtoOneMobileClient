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
using static AusBatProtoOneMobileClient.Models.KeyTree;
using static AusBatProtoOneMobileClient.Models.KeyTree.PickerCharacterPrompt;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTree
    {
        public KeyTreeNodeBase RootNode { get; set; }

        public List<PickerCharacterPrompt> PickerCharacters = new List<PickerCharacterPrompt>();

        public class KeyTreeNodeBase
        {
            // Node(A) => Node(B)

            public string NodeId { get; set; }
            public KeyTreeNode Parent { get; set; }
            public List<KeyTreeNodeBase> Children { get; set; } = new List<KeyTreeNodeBase>();

            #region *// Prompt => Trigger relation
            public List<CharacterPromptBase> PromptCharactersForNextLevel = new List<CharacterPromptBase>();    // In Node(A)

            public List<CharacterTriggerBase> TriggerCharactersForSelf = new List<CharacterTriggerBase>();  // In Node(B)
            #endregion

            public List<int> RegionIds = new List<int>();

            internal List<KeyTreeNodeBase> GetRegionTriggeredNodes(List<int> regionIds)
            {
                List<KeyTreeNodeBase> triggeredNodes = new List<KeyTreeNodeBase>();
                foreach (var childNode in Children)
                {
                    var sharedRegionIds = RegionIds.Intersect(regionIds).ToList();
                    if (!sharedRegionIds.IsEmpty())
                    {
                        triggeredNodes.Add(childNode);
                    }
                }
                return triggeredNodes;
            }

            internal List<KeyTreeNodeBase> GetEntryTriggeredNodes(CharacterPromptBase characterEntry)
            {
                List<KeyTreeNodeBase> triggeredNodes = new List<KeyTreeNodeBase>();
                // Test one level down
                if (characterEntry is PickerCharacterPrompt pcp)
                {
                    foreach (var childNode in Children)
                    {
                        var evaluateCharacters = childNode.TriggerCharactersForSelf.Where(o => o is PickerCharacterTrigger).Select(o=>o as PickerCharacterTrigger).ToList() ;
                        var evaluateCharacter = evaluateCharacters.FirstOrDefault(o => o.KeyId == pcp.KeyId);
                        if (evaluateCharacter.OptionId == pcp.EntryOptionId)
                        {
                            triggeredNodes.Add(childNode);
                        }
                    }
                }
                else
                {
                    var ncp = characterEntry as NumericCharacterPrompt;
                    foreach (var childNode in Children)
                    {
                        var evaluateCharacters = childNode.TriggerCharactersForSelf.Where(o => o is NumericCharacterTrigger).Select(o => o as NumericCharacterTrigger).ToList();
                        var evaluateCharacter = evaluateCharacters.FirstOrDefault(o => o.KeyId == ncp.KeyId);
                        if (evaluateCharacter.MinValue <= ncp.Entry && ncp.Entry <= evaluateCharacter.MaxValue)
                        {
                            triggeredNodes.Add(childNode);

                        }
                    }
                }
                return triggeredNodes;
            }
        }

        internal static KeyTreeNodeBase Clone(KeyTreeNodeBase rootNode)
        {
            throw new NotImplementedException();
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
                        KeyId = keyId,
                        Prompt = keyId,
                        Options = GenerateOptions(picker.OptionIds, picker.OptionPrompts)
                    };
                    characters.Add(newPickerCharacterPrompt);                   
                }
                else
                {
                    // RowItem is a numeric
                    characters.Add(new NumericCharacterPrompt { KeyId = keyId, Prompt = keyId });
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
                    characters.Add(new PickerCharacterTrigger { KeyId = keyId, OptionId = nodeRow.Values[rowColumnIndex] });
                }
                else
                {
                    // RowItem is a numeric
                    (float minValue, float maxValue) values = ExtractValues(nodeRow.Values[rowColumnIndex]);
                    characters.Add(new NumericCharacterTrigger { KeyId = keyId, MinValue = values.minValue, MaxValue = values.maxValue });
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

        public class CharacterTriggerBase { public string KeyId; }
        public class CharacterPromptBase {
            public string KeyId;
            public string Prompt { get; set; }
        }




        public class PickerCharacterPrompt: CharacterPromptBase
        {
            public List<Option> Options = new List<Option>();
            public string EntryOptionId;

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
            public float Entry;
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

        public static List<KeyTreeNodeBase> AddRangeUnique(List<KeyTreeNodeBase> allTriggeredKeyTreeNodes, List<KeyTreeNodeBase> triggeredKeyTreeNodes)
        {
            allTriggeredKeyTreeNodes.AddRange(triggeredKeyTreeNodes);
            allTriggeredKeyTreeNodes = allTriggeredKeyTreeNodes.Distinct(new TreeNodeComparer()).ToList();
            return allTriggeredKeyTreeNodes;
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
                    nodeParent.RegionIds.AddRange(node.RegionIds);
                    var distinctList = nodeParent.RegionIds.Distinct(new IntComparer());
                    nodeParent.RegionIds = distinctList.ToList();
                    nodeParent = nodeParent.Parent;
                }
                #endregion 
                #endregion
            }
            #endregion


        }

        private class IntComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj;
            }
        }
        #endregion
    }

    internal class TreeNodeComparer : IEqualityComparer<KeyTreeNodeBase>
    {
        public bool Equals(KeyTreeNodeBase x, KeyTreeNodeBase y)
        {
            return x.NodeId == y.NodeId;
        }

        public int GetHashCode(KeyTreeNodeBase obj)
        {
            return obj.NodeId.GetHashCode() + obj.GetHashCode();
        }
    }
}
