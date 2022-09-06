using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class NoneSkill : ActiveSkill
            {
                #region Field
                #endregion

                #region Methods
                protected NoneSkill(NoneSkillData data, Unit skillOwner) : base(data, skillOwner) { }
                #region FSM Event
                #endregion
                #endregion
            }
            public abstract class NoneSkillData : ActiveSkillData
            {

            }
        }
    }
}