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

            // ī�� ������
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
            /// �̹� �ҿ�ī�尡 �����Ѵٸ� �ҿ�ī���� ��ø������ ������Ų��.
            /// �ִ� ��ø���ÿ� �����ϸ� ���� ��ȯ�Ѵ�.
            /// </summary>
            /// <returns>�ִ� ��ø ����</returns>
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
        [BoxGroup("�ҿ�ī��", true, true), EnumToggleButtons]
        [LabelText("����"), Tooltip("CHARACTER_SOULCARD : ĳ���Ϳ��� ����Ǵ� �ҿ�ī��\nTOWER_SOULCARD : Ÿ������ ����Ǵ� �ҿ�ī��")]
        public SOULCARD_TYPE type;
        [BoxGroup("�ҿ�ī��")]
        [LabelText("���"), EnumToggleButtons]
        public SOULCARD_GRADE grade;
        [BoxGroup("�ҿ�ī��"), Min(1)]
        [LabelText("����Ȯ��"), Tooltip("�ٸ� ī���� �������� �󸶸�ŭ�� ������ �����ų��")]
        public int weight;
        [BoxGroup("�ҿ�ī��"), Required]
        [LabelText("���� �ҿ�ī���"), Tooltip("�ش� �ҿ�ī�带 ���������� ������ �� �ִ� �ҿ�ī�� ����Ʈ")]
        public SOULCARD_INDEX[] linkedSoulCards;
        [BoxGroup("�ҿ�ī��"), Required]
        [LabelText("���� �ҿ�ī���"), Tooltip("�ش� �ҿ�ī�带 ���������� �������� �ʴ� �ҿ�ī�� ����Ʈ")]
        public SOULCARD_INDEX[] removedSoulCards;
        [BoxGroup("�ҿ�ī��")]
        [LabelText("�̸�")]
        public string name;
        [BoxGroup("�ҿ�ī��"), SerializeField]
        [LabelText("����"), TextArea]
        private string desc;
        [BoxGroup("�ҿ�ī��"), Required]
        [LabelText("������")]
        public Sprite icon;

        [BoxGroup("�ҿ�ī��/ȿ��", true, true), Required, SerializeReference]
        [HideLabel]
#if UNITY_EDITOR
        [CustomContextMenu("������Ƽ ����", nameof(PPSettingStart))]
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