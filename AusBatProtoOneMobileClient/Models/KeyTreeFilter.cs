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

        public static KeyTreeFilter Current 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new KeyTreeFilter();
                }
                return instance;
            }        
        }

        internal KeyTree.KeyTreeNodeBase GetFilterResetNode()
        {
            return App.dbase.KeyTree.RootNode;
        }

        internal List<KeyTreeNodeBase> GetKeyTreeNodesInRegions(List<KeyTreeNodeBase> currentKeyTreeNodes, List<int> regionIds)
        {
            var result = new List<KeyTreeNodeBase>();
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
