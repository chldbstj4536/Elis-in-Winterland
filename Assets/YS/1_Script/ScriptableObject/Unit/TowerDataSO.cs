using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/Unit/Tower")]
    public class TowerDataSO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@SetTowerDataList()")]
        private List<TowerDataPair> towerData = new List<TowerDataPair>();

        public Tower.TowerData this[TOWER_INDEX index]
        {
            get
            {
                towerData[(int)index].data.index = index;
                return towerData[(int)index].data;
            }
        }

        private void SetTowerDataList()
        {
            if (towerData == null)
                towerData = new List<TowerDataPair>();

            if (towerData.Count != (int)TOWER_INDEX.MAX)
                for (int i = towerData.Count; i < (int)TOWER_INDEX.MAX; ++i)
                    towerData.Add(new TowerDataPair((TOWER_INDEX)i, null));
        }

        [System.Serializable]
        public struct TowerDataPair
        {
            [Space(5), HideLabel, DisableIf("@true")]
            public TOWER_INDEX index;
            [Space(10), HideLabel, SerializeReference]
            public Tower.TowerData data;

            public TowerDataPair(TOWER_INDEX index, Tower.TowerData data)
            {
                this.index = index;
                this.data = data;
            }
        }
    }
}