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
                /// 0부터 시작입니다.
                /// CurrentStack == 2면 3스택
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
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("스택 수정 허용")]
                protected bool PP_AllowModifyStack = true;
                [BoxGroup("소울카드", true, true), Min(1), OnValueChanged(nameof(OnChangedMaxStack))]
                [LabelText("최대 중첩횟수"), Tooltip("소울카드를 몇번 중첩해서 뽑을 수 있는지")]
                [EnableIf("PP_AllowModifyStack")]
                public int maxStack = 1;

                protected SoulCardEffectData() { OnChangedMaxStack(); }

                protected virtual void OnChangedMaxStack() { }
            }
        }
    }
}