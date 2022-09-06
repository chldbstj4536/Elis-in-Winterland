using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "SoulCardData", menuName = "ScriptableObjects/SoulCardData", order = 1)]
    public class SoulCardSO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@SetSoulCardList()")]
        private List<SoulCardPair> soulCardDatas = new List<SoulCardPair>();

        public SoulCardData this[SOULCARD_INDEX index] { get { return soulCardDatas[(int)index].data; } }

        private void SetSoulCardList()
        {
            if (soulCardDatas == null)
                soulCardDatas = new List<SoulCardPair>();

            if (soulCardDatas.Count != (int)SOULCARD_INDEX.MAX)
                for (int i = soulCardDatas.Count; i < (int)SOULCARD_INDEX.MAX; ++i)
                    soulCardDatas.Add(new SoulCardPair((SOULCARD_INDEX)i, new SoulCardData(true)));
        }
        [System.Serializable]
        public struct SoulCardPair
        {
            [Space(5), HideLabel, DisableIf("@true")]
            public SOULCARD_INDEX index;
            [Space(10), HideLabel]
            public SoulCardData data;

            public SoulCardPair(SOULCARD_INDEX index, SoulCardData data)
            {
                this.index = index;
                this.data = data;
            }
        }
    }
}