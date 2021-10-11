using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AusBatProtoOneMobileClient.Models.KeyTree;

namespace AusBatProtoOneMobileClient.Models.Touch
{
    public class KeyTreeFilter
    {
        private static KeyTreeFilter instance;

        public static KeyTreeFilter Current => (instance == null) ? new KeyTreeFilter(): instance;

        internal List<KeyTree.KeyTreeNode> GetFilterResetNodes()
        {
            return KeyTree.RootNode.Children.Select(o => o).ToList();
        }

        internal List<KeyTreeNode> GetKeyTreeNodesInRegions(List<KeyTreeNode> currentKeyTreeNodes, List<int> regionIds)
        {
            var result = new List<KeyTreeNode>();
            foreach (var currentKeyTreeNode in currentKeyTreeNodes)
            {
                var intersect = currentKeyTreeNode.RegionIds.Intersect(regionIds);
                if (intersect == null) continue;
                result.Add(currentKeyTreeNode);
            }
            return result;
        }

        internal object GenerateNextLevelKeyTreeNodes(List<KeyTreeNode> currentKeyTreeNodes)
        {
            throw new NotImplementedException();
        }
    }
}
