using AusBatProtoOneMobileClient.Data;
using Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TreeApp.Helpers;
using static AusBatProtoOneMobileClient.Models.KeyTree;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTree
    {
        public const string DONT_CARE = "DontCare";
        public KeyTreeNodeBase RootNode { get; set; }

        public List<PickerCharacterPrompt> PickerCharacters = new List<PickerCharacterPrompt>();

        public class KeyTreeNodeBase
        {
            // Node(A) => Node(B)

            public string NodeId { get; set; }
            public Guid NodeGuid { get; set; }
            public KeyTreeNode Parent { get; set; }
            public List<KeyTreeNodeBase> Children { get; set; } = new List<KeyTreeNodeBase>();

            #region *// Prompt => Trigger relation
            public List<CharacterPromptBase> PromptCharactersForNextLevel = new List<CharacterPromptBase>();    // In Node(A)

            public List<CharacterTriggerBase> TriggerCharactersForSelf = new List<CharacterTriggerBase>();  // In Node(B)
            #endregion

            public List<int> RegionIds = new List<int>();

            internal List<KeyTreeNodeBase> GetTriggeredNodesUsingRegions(List<KeyTreeNodeBase> currentTriggeredNodes, List<int> regionIds)
            {
                List<KeyTreeNodeBase> triggeredNodes = new List<KeyTreeNodeBase>();
                foreach (var childNode in Children)
                {
                    if (!currentTriggeredNodes.Exists(o => o.NodeId == childNode.NodeId)) continue; // Only test on nodes already triggerred
                    var sharedRegionIds = RegionIds.Intersect(regionIds).ToList();
                    if (!sharedRegionIds.IsEmpty())
                    {
                        triggeredNodes.Add(childNode);
                    }
                }
                return triggeredNodes;
            }

            internal List<KeyTreeNodeBase> GetTriggeredNodesUsingEntries(List<KeyTreeNodeBase> currentTriggeredNodes, CharacterPromptBase characterEntry)
            {
                List<KeyTreeNodeBase> triggeredNodes = new List<KeyTreeNodeBase>();
                // Test one level down

                foreach (var childNode in Children)
                {
                    #region *// Only filter on already triggered nodes, if there are triggerred nodes 
                    if (currentTriggeredNodes.Count > 0)
                    {
                        if (!currentTriggeredNodes.Exists(o => o.NodeId == childNode.NodeId)) continue; // Only test on nodes already triggerred
                    }
                    #endregion
                    if (characterEntry is PickerCharacterPrompt pcp)
                    {
                        var evaluateCharacters = childNode.TriggerCharactersForSelf.Where(o => o is PickerCharacterTrigger).Select(o => o as PickerCharacterTrigger).ToList();
                        var evaluateCharacter = evaluateCharacters.FirstOrDefault(o => o.KeyId == pcp.KeyId);
                        if (evaluateCharacter.OptionId == pcp.EntryOptionId || evaluateCharacter.OptionId == DONT_CARE)
                        {
                            triggeredNodes.Add(childNode);
                        }
                    }
                    else if (characterEntry is NumericCharacterPrompt ncp)
                    {
                        var evaluateCharacters = childNode.TriggerCharactersForSelf.Where(o => o is NumericCharacterTrigger).Select(o => o as NumericCharacterTrigger).ToList();
                        var evaluateCharacter = evaluateCharacters.FirstOrDefault(o => o.KeyId == ncp.KeyId);
                        if (evaluateCharacter.MinValue <= ncp.Entry && ncp.Entry <= evaluateCharacter.MaxValue)
                        {
                            triggeredNodes.Add(childNode);

                        }
                    }
                    else throw new ApplicationException("Unidentified character encountered");
                }

                return triggeredNodes;
            }
        }

        public class KeyTreeNode : KeyTreeNodeBase { }

        public class LeafKeyTreeNode : KeyTreeNodeBase
        {
            public string GenusId { get; set; }
            public string SpeciesId { get; set; }

            public LeafKeyTreeNode(KeyTreeNode parent, string nodeId, Guid nodeGuid)
            {
                Parent = parent;
                NodeId = nodeId;
                NodeGuid = nodeGuid;
                var ids = nodeId.Split(' ');
                GenusId = ids[0];
                for (int i = 1; i < ids.Length; i++)
                {
                    SpeciesId += ids[i] + " ";
                }
                SpeciesId = SpeciesId.Trim();
            }
        }


        int level = 0;
        public void LoadTreeFromKeyTables()
        {
            RootNode = LoadTree(null, null, "Family", Guid.NewGuid(), level);
            KeyTreeTraverser.rootNode = RootNode;

            PrintKeyTree();

            KeyTreeNode LoadTree(KeyTreeNode parentTreeNode, KeyTable parentKeyTable, string nodeId, Guid nodeGuid, int level)
            {
                level++;
                var treeNode = new KeyTreeNode { NodeId = nodeId, NodeGuid = nodeGuid, Parent = parentTreeNode };
                var treeNodeKeyTable = KeyTable.Load(parentKeyTable?.NodeId??"Family", nodeId);
                treeNodeKeyTable.PrintData(level);
                if (treeNodeKeyTable == null) return treeNode;
                var subNodeIds = treeNodeKeyTable.NodeRows.Select(o => o.NodeId).ToList();
                for(int rowIndex = 0; rowIndex < subNodeIds.Count; rowIndex++)
                {
                    KeyTreeNodeBase childNode;
                    var subNodeId = subNodeIds[rowIndex];

                    if (IsLeafNode(subNodeId))
                    {
                        childNode = new LeafKeyTreeNode(treeNode, subNodeId, Guid.NewGuid());
                    }
                    else
                    {
                        childNode = LoadTree(treeNode, treeNodeKeyTable, subNodeId, Guid.NewGuid(), level);
                    }
                    
                    treeNode.Children.Add(childNode);
                    treeNodeKeyTable.NodeRows[rowIndex].NodeGuid = childNode.NodeGuid; // Mark each one uniquely because there can be duplicates (OR operation)
                }

                #region *// Extract characters
                treeNode.PromptCharactersForNextLevel = GeneratePromptCharacters(treeNodeKeyTable);
                foreach (var childNode in treeNode.Children)
                {
                    childNode.TriggerCharactersForSelf = GenerateTriggerCharacters(treeNodeKeyTable, childNode.NodeId, childNode.NodeGuid);
                } 
                #endregion
                return treeNode;
            }

            bool IsLeafNode(string subNodeId)
            {
                // Leaf node consists of <Genus><Space><SpeciesName>..<SpeciesName> 
                return (subNodeId.Split(' ').Length > 1);
            }
        }

        internal static List<CharacterPromptBase> GetIneffectivePromptCharacters(KeyTreeNodeBase currentPromptKeyTreeNode, List<KeyTreeNodeBase> currentTriggeredKeyTreeNodes, List<CharacterPromptBase> usedPromptCharacters)
        {
            var ineffectivePromptCharacters = new List<CharacterPromptBase>();
            foreach (var promptCharacterForNextLevel in currentPromptKeyTreeNode.PromptCharactersForNextLevel)
            {
                if (promptCharacterForNextLevel is NumericCharacterPrompt)
                {
                    if (IsAllNodesCharacterRangeTheSame(promptCharacterForNextLevel.KeyId, currentTriggeredKeyTreeNodes))
                    {
                        ineffectivePromptCharacters.Add(promptCharacterForNextLevel);
                    }
                }
                else if (promptCharacterForNextLevel is PickerCharacterPrompt)
                {
                    if (IsAllNodesPickerOptionsTheSame(promptCharacterForNextLevel.KeyId, currentTriggeredKeyTreeNodes))
                    {
                        ineffectivePromptCharacters.Add(promptCharacterForNextLevel);
                    }
                }
                else throw new ApplicationException("Undefined character type");
            }
            return ineffectivePromptCharacters;
        }

        private static bool IsAllNodesPickerOptionsTheSame(string keyId, List<KeyTreeNodeBase> currentTriggeredKeyTreeNodes)
        {
            if (currentTriggeredKeyTreeNodes.Count == 0) return false;
            string previousOptionId = "";
            foreach (var currentTriggeredKeyTreeNode in currentTriggeredKeyTreeNodes)
            {
                var character = currentTriggeredKeyTreeNode.TriggerCharactersForSelf.FirstOrDefault(o => o.KeyId == keyId) as PickerCharacterTrigger;
                if (previousOptionId == "")
                {
                    previousOptionId = character.OptionId;
                    continue;
                }
                if (character.OptionId != previousOptionId) return false;
            }
            return true;
        }

        private static bool IsAllNodesCharacterRangeTheSame(string keyId, List<KeyTreeNodeBase> currentTriggeredKeyTreeNodes)
        {
            if (currentTriggeredKeyTreeNodes.Count == 0) return false;
            (float, float) previousRange = (0, 0);
            foreach (var currentTriggeredKeyTreeNode in currentTriggeredKeyTreeNodes)
            {
                var character = currentTriggeredKeyTreeNode.TriggerCharactersForSelf.FirstOrDefault(o => o.KeyId == keyId) as NumericCharacterTrigger;
                if (previousRange == (0,0))
                {
                    previousRange = (character.MinValue, character.MaxValue);
                    continue;
                }
                (float, float) nextRange = (character.MinValue, character.MaxValue);
                if (nextRange != previousRange) return false;
            }
            return true;
        }

        private List<CharacterPromptBase> GeneratePromptCharacters(KeyTable keyTable)
        {
            try
            {
                List<CharacterPromptBase> characters = new List<CharacterPromptBase>();
                foreach (var keyId in keyTable.KeyIds)
                {
                    var keyIdsIndex = keyTable.KeyIds.IndexOf(keyId);
                    var picker = keyTable.Pickers.FirstOrDefault(o => o.Id == keyId);
                    if (picker != null)
                    {
                        // RowItem is a picker
                        var prompt = (keyTable.KeyPrompts.Count == 0) ? keyId : keyTable.KeyPrompts[keyIdsIndex];
                        var imageSource = (keyTable.KeyImages.Count == 0) ? keyId : keyTable.KeyImages[keyIdsIndex];
                        var displayOrder = (keyTable.KeyDisplayOrders.Count == 0) ? 0 : keyTable.KeyDisplayOrders[keyIdsIndex];
                        var newPickerCharacterPrompt = new PickerCharacterPrompt
                        {
                            KeyId = keyId,
                            Prompt = prompt,
                            ImageSource = imageSource,
                            DisplayOrder = displayOrder,
                            Options = GenerateOptions(picker.OptionIds, picker.OptionPrompts, picker.OptionImages)
                        };
                        characters.Add(newPickerCharacterPrompt);
                    }
                    else
                    {
                        // RowItem is a numeric
                        var prompt = (keyTable.KeyPrompts.Count == 0) ? keyId : keyTable.KeyPrompts[keyIdsIndex];
                        var imageSource = (keyTable.KeyImages.Count == 0) ? keyId : keyTable.KeyImages[keyIdsIndex];
                        var displayOrder = (keyTable.KeyDisplayOrders.Count == 0) ? 0 : keyTable.KeyDisplayOrders[keyIdsIndex];
                        characters.Add(new NumericCharacterPrompt
                        {
                            KeyId = keyId,
                            Prompt = prompt,
                            ImageSource = imageSource,
                            DisplayOrder = displayOrder
                        });
                    }
                }
                return characters;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Problem generating characters for keytable [{keyTable.NodeId}]. {ex.Message}");
            }
        }

        private List<CharacterTriggerBase> GenerateTriggerCharacters(KeyTable keyTable, string nodeId, Guid nodeGuid)
        {
            List<CharacterTriggerBase> characters = new List<CharacterTriggerBase>();
            if (keyTable == null) return characters;
            var nodeRow = keyTable.NodeRows.FirstOrDefault(o => o.NodeGuid == nodeGuid);
            foreach (var keyId in keyTable.KeyIds)
            {
                var rowColumnIndex = keyTable.KeyIds.IndexOf(keyId);
                var picker = keyTable.Pickers.FirstOrDefault(o => o.Id == keyId);
                if (picker != null)
                {
                    // RowItem is a picker
                    string optionId = (nodeRow.Values[rowColumnIndex] == "") ? DONT_CARE : nodeRow.Values[rowColumnIndex];
                    characters.Add(new PickerCharacterTrigger { KeyId = keyId, OptionId = optionId });
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

        private List<PickerCharacterPrompt.Option> GenerateOptions(List<string> optionIds, List<string> optionPrompts, List<string> optionImages)
        {
            var result = new List<PickerCharacterPrompt.Option>();
            for (int i = 0; i < optionIds.Count; i++)
            {
                var image = (optionImages.Count > 0) ? optionImages[i] : "";
                result.Add(new PickerCharacterPrompt.Option { OptionId = optionIds[i], OptionPrompt = optionPrompts[i], OptionImageSource = image});
            }
            return result;
        }

        private (float minValue, float maxValue) ExtractValues(string value)
        {
            if (value == "")
            {
                // Don't care value
                return (float.MinValue, float.MaxValue);
            }
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




        public class CharacterPromptBase {
            public string KeyId;
            public string Prompt { get; set; }
            public string ImageSource { get; set; }
            public int DisplayOrder { get; set; }
        }
        public class CharacterTriggerBase
        {
            public string KeyId;
        }

        public class PickerCharacterPrompt: CharacterPromptBase
        {
            public List<Option> Options = new List<Option>();
            public string EntryOptionId;

            public class Option
            {
                public string OptionId { get; set; }
                public string OptionPrompt { get; set; }
                public string OptionImageSource { get; set; }
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

        public static List<KeyTreeNodeBase> AddNodesRangeUnique(List<KeyTreeNodeBase> allTriggeredKeyTreeNodes, List<KeyTreeNodeBase> triggeredKeyTreeNodes)
        {
            allTriggeredKeyTreeNodes.AddRange(triggeredKeyTreeNodes);
            allTriggeredKeyTreeNodes = allTriggeredKeyTreeNodes.Distinct(new TreeNodeComparer()).ToList();
            return allTriggeredKeyTreeNodes;
        }

        internal static List<CharacterPromptBase> AddCharacterUnique(List<CharacterPromptBase> currentList, CharacterPromptBase newItem)
        {
            if (currentList.Exists(o=>o.KeyId == newItem.KeyId))
            {
                return currentList;
            }
            currentList.Add(newItem);
            return currentList;
        }

        internal void EnhanceTree(List<Species> specieses)
        {
            #region *// Enter leave node data
            foreach (var species in specieses)
            {
                var nodeId = $"{species.GenusId.ToUpperFirstChar()} {species.SpeciesId.ToLower()}";
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
