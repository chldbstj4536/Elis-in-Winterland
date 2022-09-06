using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ATinyWispDA : TargetingSkill
            {
                #region Field
                private PoolingComponent hitFXPrefab;
                public float attackDMG_IncreaseRate;
                private int totalDmg = 0;
                private Transform targetTr;
                #endregion

                public ATinyWispDA(ATinyWispDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    hitFXPrefab = data.hitFXPrefab;
                    attackDMG_IncreaseRate = data.attackDMG_IncreaseRate;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                    CooltimeStart();
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    if (targetTr != target.transform)
                    {
                        targetTr = target.transform;
                        totalDmg = caster.TotalPhysicsPower;
                    }
                    else
                        totalDmg = (int)(totalDmg * attackDMG_IncreaseRate);

                    Attack(target.GetComponent<Unit>(), DAMAGE_TYPE.NORMAL, target.position);
                    PrefabPool.GetObject(hitFXPrefab).transform.position = target.position;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(totalDmg, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class ATinyWispDAData : TargetingSkillData
            {
                [BoxGroup("TinyWispDA", true, true), LabelText("피격 효과 프리팹")]
                public PoolingComponent hitFXPrefab;
                [BoxGroup("TinyWispDA"), Min(0.0f), LabelText("열기중첩의 공격력 증가율"), Tooltip("단일 대상을 공격할때 증가하는 공격력 증가율입니다.")]
                public float attackDMG_IncreaseRate;

                public ATinyWispDAData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ATinyWispDA(this, owner);
                }
            }
        }
    }
}