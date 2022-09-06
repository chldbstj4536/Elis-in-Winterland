using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "PlayableUnitData", menuName = "ScriptableObjects/Unit/PlayableUnit")]
    public class PlayableUnitDataSO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@SetPlayableUnitDataList()")]
        private List<PlayableUnitDataPair> unitData = new List<PlayableUnitDataPair>();

        public PlayableUnit.PlayableUnitData this[PLAYABLE_UNIT_INDEX index]
        {
            get
            {
                unitData[(int)index].data.index = index;
                return unitData[(int)index].data;
            }
        }
        private void SetPlayableUnitDataList()
        {
            if (unitData == null)
                unitData = new List<PlayableUnitDataPair>();

            if (unitData.Count != (int)PLAYABLE_UNIT_INDEX.MAX)
                for (int i = unitData.Count; i < (int)PLAYABLE_UNIT_INDEX.MAX; ++i)
                    unitData.Add(new PlayableUnitDataPair((PLAYABLE_UNIT_INDEX)i, null));
        }

        [System.Serializable]
        public struct PlayableUnitDataPair
        {
            [Space(5), HideLabel, DisableIf("@true")]
            public PLAYABLE_UNIT_INDEX index;
            [Space(10), HideLabel, SerializeReference]
            public PlayableUnit.PlayableUnitData data;

            public PlayableUnitDataPair(PLAYABLE_UNIT_INDEX index, PlayableUnit.PlayableUnitData data)
            {
                this.index = index;
                this.data = data;
            }
        }
    }
}