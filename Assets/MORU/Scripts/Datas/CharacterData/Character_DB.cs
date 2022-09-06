using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;


namespace Moru
{
    public partial class UserSaveData
    {
        /////현재 선택한 캐릭터 정보/////
        [Tooltip("현재 선택된 캐릭터 인덱스.")]
        [SerializeField]
        private PLAYABLE_UNIT_INDEX current_PLAYABLE_UNIT_INDEX;
        public PLAYABLE_UNIT_INDEX Current_PLAYABLE_UNIT_INDEX { get { return current_PLAYABLE_UNIT_INDEX; } set { current_PLAYABLE_UNIT_INDEX = value; } }

        [ShowInInspector, LabelText("현재 선택한 캐릭터 데이터")]
        public Character_DB GetCurrentCharacterDB => characters_Info[Current_PLAYABLE_UNIT_INDEX];

        [Tooltip("현재 캐릭터 정보창에 오를 캐릭터 데이터 그릇입니다.")]
        [SerializeField]
        private PLAYABLE_UNIT_INDEX load_PLAYABLE_UNIT_INDEX;
        public PLAYABLE_UNIT_INDEX Load_Characters_Index { get { return load_PLAYABLE_UNIT_INDEX; } set { load_PLAYABLE_UNIT_INDEX = value; } }

        public Character_DB GetLoadCharacter => characters_Info[Load_Characters_Index];

        [Tooltip("캐릭터 데이터 그릇입니다.")]
        [SerializeField, ShowInInspector]
        public Dictionary<PLAYABLE_UNIT_INDEX, Character_DB> characters_Info;


        /// <summary>
        /// 캐릭터 리스트 정보를 초기화합니다.
        /// </summary>
        public void Init_Character()
        {
            characters_Info = new Dictionary<PLAYABLE_UNIT_INDEX, Character_DB>();
            for (int i = 0; i < (int)PLAYABLE_UNIT_INDEX.MAX; i++)
            {
                var CHARACTER_ENUM = (PLAYABLE_UNIT_INDEX)i;
                //////생성자에서 초기화해주기
                characters_Info.Add(CHARACTER_ENUM, new Character_DB());
                characters_Info.TryGetValue(CHARACTER_ENUM, out Character_DB DB_Value);
                DB_Value.Index = CHARACTER_ENUM;
                DB_Value.Initialized();
                //////
            }
        }


        

        public partial class Character_DB
        {
            #region 캐릭터 기본정보
            [SerializeField, LabelText("보유 여부"), ReadOnly] private bool isHaving;
            public bool IsHaving => isHaving;

            [SerializeField, LabelText("캐릭터 인덱스")] PLAYABLE_UNIT_INDEX index;
            public PLAYABLE_UNIT_INDEX Index { get { return index; } set { index = value; } }

            [SerializeField, LabelText("유닛 데이터")] YS.PlayableUnit.PlayableUnitData data;
            public YS.PlayableUnit.PlayableUnitData Data
            {
                get
                {
                    if (data == null)
                    { data = UserSaveData.ChararcterSO[index]; }
                    return data;
                }
            }

            [SerializeField, LabelText("유닛 스켈레톤 데이터")] private SkeletonDataAsset skeletonDataAsset;
            public SkeletonDataAsset skeleton
            {
                get
                {
                    if (skeletonDataAsset == null)
                    { skeletonDataAsset = UserSaveData.SkeletonSO.character_Skeleton[(int)index]; }
                    return skeletonDataAsset;
                }
            }

            public Character_DB(bool _isHaving = true)
            {
                isHaving = _isHaving;
            }

            /// <summary>
            /// 해당 캐릭터에 대한 모든 초기화작업을 진행합니다.
            /// </summary>
            public void Initialized()
            {
                INIT_Skill();
                Init_Skin();
            }

            #endregion
        }


    }
}