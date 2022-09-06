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
        /////���� ������ ĳ���� ����/////
        [Tooltip("���� ���õ� ĳ���� �ε���.")]
        [SerializeField]
        private PLAYABLE_UNIT_INDEX current_PLAYABLE_UNIT_INDEX;
        public PLAYABLE_UNIT_INDEX Current_PLAYABLE_UNIT_INDEX { get { return current_PLAYABLE_UNIT_INDEX; } set { current_PLAYABLE_UNIT_INDEX = value; } }

        [ShowInInspector, LabelText("���� ������ ĳ���� ������")]
        public Character_DB GetCurrentCharacterDB => characters_Info[Current_PLAYABLE_UNIT_INDEX];

        [Tooltip("���� ĳ���� ����â�� ���� ĳ���� ������ �׸��Դϴ�.")]
        [SerializeField]
        private PLAYABLE_UNIT_INDEX load_PLAYABLE_UNIT_INDEX;
        public PLAYABLE_UNIT_INDEX Load_Characters_Index { get { return load_PLAYABLE_UNIT_INDEX; } set { load_PLAYABLE_UNIT_INDEX = value; } }

        public Character_DB GetLoadCharacter => characters_Info[Load_Characters_Index];

        [Tooltip("ĳ���� ������ �׸��Դϴ�.")]
        [SerializeField, ShowInInspector]
        public Dictionary<PLAYABLE_UNIT_INDEX, Character_DB> characters_Info;


        /// <summary>
        /// ĳ���� ����Ʈ ������ �ʱ�ȭ�մϴ�.
        /// </summary>
        public void Init_Character()
        {
            characters_Info = new Dictionary<PLAYABLE_UNIT_INDEX, Character_DB>();
            for (int i = 0; i < (int)PLAYABLE_UNIT_INDEX.MAX; i++)
            {
                var CHARACTER_ENUM = (PLAYABLE_UNIT_INDEX)i;
                //////�����ڿ��� �ʱ�ȭ���ֱ�
                characters_Info.Add(CHARACTER_ENUM, new Character_DB());
                characters_Info.TryGetValue(CHARACTER_ENUM, out Character_DB DB_Value);
                DB_Value.Index = CHARACTER_ENUM;
                DB_Value.Initialized();
                //////
            }
        }


        

        public partial class Character_DB
        {
            #region ĳ���� �⺻����
            [SerializeField, LabelText("���� ����"), ReadOnly] private bool isHaving;
            public bool IsHaving => isHaving;

            [SerializeField, LabelText("ĳ���� �ε���")] PLAYABLE_UNIT_INDEX index;
            public PLAYABLE_UNIT_INDEX Index { get { return index; } set { index = value; } }

            [SerializeField, LabelText("���� ������")] YS.PlayableUnit.PlayableUnitData data;
            public YS.PlayableUnit.PlayableUnitData Data
            {
                get
                {
                    if (data == null)
                    { data = UserSaveData.ChararcterSO[index]; }
                    return data;
                }
            }

            [SerializeField, LabelText("���� ���̷��� ������")] private SkeletonDataAsset skeletonDataAsset;
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
            /// �ش� ĳ���Ϳ� ���� ��� �ʱ�ȭ�۾��� �����մϴ�.
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