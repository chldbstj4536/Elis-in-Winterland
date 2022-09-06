using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Moru
{
    [CreateAssetMenu(fileName = "MasterySO", menuName = "Moru/MasterySO")]
    public class MasterySO : ScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 1, DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = false), DisableContextMenu]
        [Searchable, SerializeField, OnInspectorInit("@MasteryInit()")]
        private List<Vertical_MasteryData> masteryDatas = new List<Vertical_MasteryData>();
        public Vertical_MasteryData this[YS.PLAYABLE_UNIT_INDEX index] => masteryDatas[(int)index];




        public void MasteryInit()
        {
            if (masteryDatas == null)
                masteryDatas = new List<Vertical_MasteryData>();
            if (masteryDatas.Count != (int)YS.PLAYABLE_UNIT_INDEX.MAX)
            {
                for (int i = masteryDatas.Count; i < (int)YS.PLAYABLE_UNIT_INDEX.MAX; i++)
                {
                    masteryDatas.Add(new Vertical_MasteryData((YS.PLAYABLE_UNIT_INDEX)i));
                    Debug.Log($"마스터리 이니셜라이징");
                }
            }
        }

        [System.Serializable]
        public struct Vertical_MasteryData
        {
            public YS.PLAYABLE_UNIT_INDEX character;

            [OnValueChanged("AutoSetPlayerLevelList", true), SerializeField]
            public int[] NeedPlayerLevelList;

            [SerializeField, LabelText("캐릭터타입 레벨에 따른 특성리스트")]
            public List<Horizontal_MasteryData> Vertical_Masterys_TypeAttack;

            [SerializeField, LabelText("유틸리티타입 레벨에 따른 특성리스트")]
            public List<Horizontal_MasteryData> Vertical_Masterys_TypeUtility;

            /// <summary>
            /// 마스터리 데이터를 레벨과 해당레벨의 인덱스를 참조하여 받아옵니다.
            /// </summary>
            /// <param name="UnitIndex"></param>
            /// <param name="level"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            static public UserSaveData.Character_DB.Mastery_DB GetMastery(YS.PLAYABLE_UNIT_INDEX UnitIndex,Define.MasteryType type, Define.MasteryLevel level, int index)
            {
                var MasteryData = Moru.UserSaveData.MasterySO.masteryDatas[(int)UnitIndex];
                var refList = MasteryData.Vertical_Masterys_TypeAttack[(int)level];
                return refList.Masterys[index];
            }

            public Vertical_MasteryData(YS.PLAYABLE_UNIT_INDEX index)
            {
                character = index;
                NeedPlayerLevelList = new int[((int)Define.MasteryLevel.MAX)];
                if(NeedPlayerLevelList.Length != (int)Define.MasteryLevel.MAX)
                {

                }


                Vertical_Masterys_TypeAttack = new List<Horizontal_MasteryData>((int)Define.MasteryLevel.MAX);
                if (Vertical_Masterys_TypeAttack.Count != (int)Define.MasteryLevel.MAX)
                {
                    for (int i = 0; i < (int)Define.MasteryLevel.MAX; i++)
                    {
                        Vertical_Masterys_TypeAttack.Add(new Horizontal_MasteryData((Define.MasteryLevel)i, Define.MasteryType.Character));
                    }
                }
                Vertical_Masterys_TypeUtility = new List<Horizontal_MasteryData>((int)Define.MasteryLevel.MAX);
                if (Vertical_Masterys_TypeUtility.Count != (int)Define.MasteryLevel.MAX)
                {
                    for (int i = 0; i < (int)Define.MasteryLevel.MAX; i++)
                    {
                        Vertical_Masterys_TypeUtility.Add(new Horizontal_MasteryData((Define.MasteryLevel)i, Define.MasteryType.Utillity));
                    }
                }

                AutoSetPlayerLevelList();
            }


            private void AutoSetPlayerLevelList()
            {
                for (int i = 0; i < NeedPlayerLevelList.Length; i++)
                {
                    if (i == 0) continue;
                    if (NeedPlayerLevelList[i] < NeedPlayerLevelList[i - 1])
                    {
                        NeedPlayerLevelList[i] = NeedPlayerLevelList[i - 1] + 1;
                    }
                }
            }
        }


        [System.Serializable]
        public struct Horizontal_MasteryData
        {
            [SerializeField, ReadOnly]
            public Moru.Define.MasteryLevel LevelType;

            [SerializeField, LabelText("가로줄 특성리스트")]
            public List<UserSaveData.Character_DB.Mastery_DB> Masterys;

            public Horizontal_MasteryData(Moru.Define.MasteryLevel _lvType, Moru.Define.MasteryType type)
            {
                LevelType = _lvType;
                Masterys = new List<UserSaveData.Character_DB.Mastery_DB>();
                Debug.Log($"실행확인");
                if (Masterys.Count != 4)
                {
                    for (int i = 0; i <4; i++)
                    {
                        Masterys.Add(new UserSaveData.Character_DB.Mastery_DB((int)_lvType * 4 + i, _lvType, type));
                    }
                }
            }
        }
    }
}