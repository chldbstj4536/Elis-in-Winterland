using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{

    public abstract partial class Unit
    {
        public class SoulCard
        {
            #region Field
            private Skill.SoulCardEffect effect;

            // 카드 정보들
            private static readonly SoulCardSO soulCardDatas = ResourceManager.GetResource<SoulCardSO>("Datas/SoulCardData");
            #endregion

            #region Properties
            public static SoulCardSO SoulCardDatas => soulCardDatas;
            public bool IsMaxLevel => effect.CurrentStack + 1 == effect.MaxStack;
            public Skill.SoulCardEffect Effect => effect;
            #endregion

            #region Methods
            public SoulCard(SOULCARD_INDEX index, PlayableUnit owner)
            {
                var data = soulCardDatas[index];

                effect = data.effectData.Instantiate(owner) as Skill.SoulCardEffect;
                effect.CurrentStack = 0;
            }

            /// <summary>
            /// 이미 소울카드가 존재한다면 소울카드의 중첩스택을 증가시킨다.
            /// 최대 중첩스택에 도달하면 참을 반환한다.
            /// </summary>
            /// <returns>최대 충첩 여부</returns>
            public void AddStack()
            {
                ++effect.CurrentStack;
            }
            #endregion
        }
    }
    [Serializable]
    public struct SoulCardData
    {
        [BoxGroup("소울카드", true, true), EnumToggleButtons]
        [LabelText("종류"), Tooltip("CHARACTER_SOULCARD : 캐릭터에게 적용되는 소울카드\nTOWER_SOULCARD : 타워에게 적용되는 소울카드")]
        public SOULCARD_TYPE type;
        [BoxGroup("소울카드")]
        [LabelText("등급"), EnumToggleButtons]
        public SOULCARD_GRADE grade;
        [BoxGroup("소울카드"), Min(1)]
        [LabelText("등장확률"), Tooltip("다른 카드들과 비교했을때 얼마만큼의 비율로 등장시킬지")]
        public int weight;
        [BoxGroup("소울카드"), Required]
        [LabelText("연관 소울카드들"), Tooltip("해당 소울카드를 보유했을때 등장할 수 있는 소울카드 리스트")]
        public SOULCARD_INDEX[] linkedSoulCards;
        [BoxGroup("소울카드"), Required]
        [LabelText("제외 소울카드들"), Tooltip("해당 소울카드를 보유했을때 등장하지 않는 소울카드 리스트")]
        public SOULCARD_INDEX[] removedSoulCards;
        [BoxGroup("소울카드")]
        [LabelText("이름")]
        public string name;
        [BoxGroup("소울카드"), SerializeField]
        [LabelText("설명"), TextArea]
        private string desc;
        [BoxGroup("소울카드"), Required]
        [LabelText("아이콘")]
        public Sprite icon;

        [BoxGroup("소울카드/효과", true, true), Required, SerializeReference]
        [HideLabel]
#if UNITY_EDITOR
        [CustomContextMenu("프로퍼티 수정", nameof(PPSettingStart))]
#endif
        public Unit.Skill.SoulCardEffectData effectData;

        private string convertedDesc;

        public string Description
        {
            get
            {
                if (convertedDesc == null || convertedDesc.Length == 0)
                    convertedDesc = Utility.ConvertYSFormatToString(this, desc);
                return convertedDesc;
            }
        }

#if UNITY_EDITOR
        private void PPSettingStart() { effectData.ShowPPSetting = true; }
#endif
        public SoulCardData(bool init)
        {
            type = SOULCARD_TYPE.CHARACTER_SOULCARD;
            grade = SOULCARD_GRADE.COMMON;
            weight = 1;
            linkedSoulCards = new SOULCARD_INDEX[0];
            removedSoulCards = new SOULCARD_INDEX[0];
            name = null;
            desc = null;
            icon = null;
            effectData = null;
            convertedDesc = null;
        }
    }
}