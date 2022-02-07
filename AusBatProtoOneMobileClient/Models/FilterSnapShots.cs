using System.Linq;
using System.Collections.Generic;
using static AusBatProtoOneMobileClient.Models.KeyTree;
using static DocGenOneMobileClient.Views.FilterSnapShots;
using static DocGenOneMobileClient.Views.KeyPageViewModel;

namespace DocGenOneMobileClient.Views
{
    public class FilterSnapShots : List<KeyFilterSnapShot>
    {

        public class KeyFilterSnapShot
        {
            public KeyTreeNodeBase BeforePromptKeyTreeNode;
            public bool BeforeHasRegionFilterBeenUsed;
            public List<int> BeforeRegionIds;
            public List<KeyTreeNodeBase> BeforeTriggeredKeyTreeNodes;
            public List<CharacterPromptBase> BeforeUsedPromptCharacters;
            public FilterState BeforeFilterState;
        }

        public class KeyResultSnapShot
        {
            public List<KeyTreeNodeBase> selectedKeyTreeNodes;
        }


        public void Push(KeyFilterSnapShot snapshot)
        {
            Add(snapshot);
        }

        public KeyFilterSnapShot Pop()
        {
            if (Count > 0)
            {
                var snapShot = this.Last();
                this.Remove(snapShot);
                return snapShot;
            }
            return null;
        }
    }
}


