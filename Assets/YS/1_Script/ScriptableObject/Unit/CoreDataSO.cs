using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "CoreData", menuName = "ScriptableObjects/Unit/Core")]
    public class CoreDataSO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@SetCoreDataList()")]
        private List<CoreDataPair> coreData = new List<CoreDataPair>();

        public Core.CoreData this[CORE_INDEX index]
        {
            get
            {
                coreData[(int)index].data.index = index;
                return coreData[(int)index].data;
            }
        }

        private void SetCoreDataList()
        {
            if (coreData == null)
                coreData = new List<CoreDataPair>();

            if (coreData.Count != (int)CORE_INDEX.MAX)
                for (int i = coreData.Count; i < (int)CORE_INDEX.MAX; ++i)
                    coreData.Add(new CoreDataPair((CORE_INDEX)i, null));
        }

        [System.Serializable]
        public struct CoreDataPair
        {
            [Space(5), HideLabel, DisableIf("@true")]
            public CORE_INDEX index;
            [Space(10), HideLabel, SerializeReference]
            public Core.CoreData data;

            public CoreDataPair(CORE_INDEX index, Core.CoreData data)
            {
                this.index = index;
                this.data = data;
            }
        }
    }
}