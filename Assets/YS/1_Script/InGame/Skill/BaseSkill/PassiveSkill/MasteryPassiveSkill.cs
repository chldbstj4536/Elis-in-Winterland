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
            public abstract partial class MasteryEffect : PassiveSkill
            {
                #region Field
                #endregion

                #region Properties
                #endregion

                #region Methods
                protected MasteryEffect(MasteryEffectData data, Unit skillOwner) : base(data, skillOwner)
                {
                }

                protected abstract void InstantEffect();
                #endregion
            }
            [System.Serializable]
            public abstract class MasteryEffectData : PassiveSkillData
            {
                protected MasteryEffectData() { OnChangedMaxStack(); }

                protected virtual void OnChangedMaxStack() { }
            }
        }
    }
}