using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract partial class SoulCardEffect : PassiveSkill
            {
                #region Field
                private int curStack;
                private readonly int maxStack;
                #endregion

                #region Properties
                /// <summary>
                /// 0���� �����Դϴ�.
                /// CurrentStack == 2�� 3����
                /// </summary>
                public int CurrentStack
                {
                    get { return curStack; }
                    set
                    {
                        curStack = value;
                        InstantEffect();
                    }
                }
                public int MaxStack => maxStack;
                #endregion

                #region Methods
                protected SoulCardEffect(SoulCardEffectData data, Unit skillOwner) : base(data, skillOwner)
                {
                    maxStack = data.maxStack;
                }

                protected abstract void InstantEffect();
                #endregion
            }
            [System.Serializable]
            public abstract class SoulCardEffectData : PassiveSkillData
            {
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("���� ���� ���")]
                protected bool PP_AllowModifyStack = true;
                [BoxGroup("�ҿ�ī��", true, true), Min(1), OnValueChanged(nameof(OnChangedMaxStack))]
                [LabelText("�ִ� ��øȽ��"), Tooltip("�ҿ�ī�带 ��� ��ø�ؼ� ���� �� �ִ���")]
                [EnableIf("PP_AllowModifyStack")]
                public int maxStack = 1;

                protected SoulCardEffectData() { OnChangedMaxStack(); }

                protected virtual void OnChangedMaxStack() { }
            }
        }
    }
}