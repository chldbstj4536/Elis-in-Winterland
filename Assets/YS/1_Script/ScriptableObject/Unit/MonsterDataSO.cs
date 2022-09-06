using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Unit/Monster")]
    public class MonsterDataSO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@SetMonsterDataList()")]
        private List<MonsterDataPair> monsterData = new List<MonsterDataPair>();

        public Monster.MonsterData this[MONSTER_INDEX index]
        {
            get
            {
                monsterData[(int)index].data.index = index;
                return monsterData[(int)index].data;
            }
        }

        private void SetMonsterDataList()
        {
            if (monsterData == null)
                monsterData = new List<MonsterDataPair>();

            if (monsterData.Count != (int)MONSTER_INDEX.MAX)
                for (int i = monsterData.Count; i < (int)MONSTER_INDEX.MAX; ++i)
                    monsterData.Add(new MonsterDataPair((MONSTER_INDEX)i, null));
        }

        [System.Serializable]
        public struct MonsterDataPair
        {
            [Space(5), HideLabel, DisableIf("@true")]
            public MONSTER_INDEX index;
            [HideLabel, BoxGroup, SerializeReference]
            public Monster.MonsterData data;

            public MonsterDataPair(MONSTER_INDEX index, Monster.MonsterData data)
            {
                this.index = index;
                this.data = data;
            }
        }
    }
}