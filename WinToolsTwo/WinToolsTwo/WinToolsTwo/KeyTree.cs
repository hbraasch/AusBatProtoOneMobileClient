using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinToolsTwo
{
    public class KeyTree
    {
        public TreeNode RootNode { get; set; }

        public List<PickerCharacterDefinition> PickerCharacterDefinitions = new List<PickerCharacterDefinition>();

        public class TreeNode
        {
            public string NodeId { get; set; }
            public TreeNode Parent { get; set; }
            public List<TreeNode> Children { get; set; } = new List<TreeNode>();

            public List<CharacterBase> Characters = new List<CharacterBase>();

        }

        public void LoadTree()
        {
            RootNode = LoadTree(null, null, "family");

            TreeNode LoadTree(TreeNode parentTreeNode, KeyTable parentKeyTable, string nodeId)
            {
                var treeNode = new TreeNode { NodeId = nodeId, Parent = parentTreeNode };
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
                    characters.Add(new PickerCharacter(parentKeyTable.NodeId, keyId, nodeRow.Values[rowColumnIndex]));
                    if (!PickerCharacterDefinitions.Exists(o=>o.keyTableNodeId == parentKeyTable.NodeId && o.keyId == keyId))
                    {                       
                        var newPickerCharacterDefinition = new PickerCharacterDefinition { 
                            keyTableNodeId = parentKeyTable.NodeId, 
                            keyId = keyId,
                            Options = GenerateOptions(picker.OptionIds, picker.OptionPrompts) };
                        PickerCharacterDefinitions.Add(newPickerCharacterDefinition);
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

            List<PickerCharacterDefinition.Option> GenerateOptions(List<string> optionIds, List<string> optionPrompts)
            {
                var result = new List<PickerCharacterDefinition.Option>();
                for (int i = 0; i < optionIds.Count; i++)
                {
                    result.Add(new PickerCharacterDefinition.Option { OptionId = optionIds[i], OptionPrompt = optionPrompts[i]});
                }
                return result;
            }
        }

        public class PickerCharacter : CharacterBase
        {
            private string keyTableNodeId;
            private string keyId;
            private string optionId;

            public PickerCharacter(string keyTableNodeId, string keyId, string optionId)
            {
                this.keyTableNodeId = keyTableNodeId;
                this.keyId = keyId;
                this.optionId = optionId;
            }
        }

        public class PickerCharacterDefinition
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
            private string keyTableNodeId;
            private string keyId;
            private float minValue;
            private float maxValue;

            public NumericCharacter(string keyTableNodeId, string keyId, float minValue, float maxValue)
            {
                this.keyTableNodeId = keyTableNodeId;
                this.keyId = keyId;
                this.minValue = minValue;
                this.maxValue = maxValue;
            }
        }

        #region *// Helpers

        public TreeNode GetKeyNode(string nodeId)
        {
            TreeNode node = null;
            var traverser = new KeyTreeTraverser(RootNode, (parent, current, level) => 
            {
                Debug.WriteLine($"Parent: {parent?.NodeId} Current: {current.NodeId} Level: {level}");
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

        public class KeyTreeTraverser
        {
            TreeNode startNode;
            Func<TreeNode, TreeNode, int, ExitAction> processNode;
            public enum ExitAction { ExitImmediately, Continue };
            public KeyTreeTraverser(TreeNode startNode, Func<TreeNode, TreeNode, int, ExitAction> processNode)
            {
                this.startNode = startNode;
                this.processNode = processNode;
            }

            int level = 0;
            public void Execute()
            {
                RecursiveExecute(startNode);

                bool RecursiveExecute(TreeNode node)
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
        #endregion
    }
}
