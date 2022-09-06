using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AFlameElementalistHeal : NoneSkill
            {
                #region Field
                private int targetMaxCount;
                #endregion
                public AFlameElementalistHeal(AFlameElementalistHealData data, Unit skillOwner) : base(data, skillOwner)
                {
                    targetMaxCount = data.targetMaxCount;
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

                    int curCount = targetMaxCount;
                    var hitList = Utility.SweepUnit(inRangeHitboxes, caster.transform.position, InRangeHitBoxesRot, true, TargetLayerMask, TargetType);
                    foreach (var hit in hitList)
                    {
                        Monster taker = hit.transform.GetComponent<Monster>();
                        if (taker.Category <= MONSTER_CATEGORY.HIGH_CLASS && taker.CurrentHP < taker.MaxHP)
                        {
                            Attack(taker, DAMAGE_TYPE.HEAL, taker.transform.position);
                            if (--curCount == 0)
                                break;
                        }
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return -caster.TotalMagicPower;
                }
                protected override bool TargetCheck(List<RaycastHit> targets)
                {
                    if (!base.TargetCheck(targets))
                        return false;
                    
                    bool result = false;

                    foreach (var target in targets)
                    {
                        Monster taker = target.transform.GetComponent<Monster>();
                        if (taker.Category <= MONSTER_CATEGORY.HIGH_CLASS && taker.CurrentHP < taker.MaxHP)
                        {
                            result = true;
                            break;
                        }
                    }

                    return result;
                }
            }
            protected class AFlameElementalistHealData : NoneSkillData
            {
                [BoxGroup("정령술사의 회복술", true, true), Min(0), LabelText("판정 대상의 최대 수"), Tooltip("스킬 발동시 스킬 적용이 가능한 최대치를 결정합니다.")]
                public int targetMaxCount;

                public AFlameElementalistHealData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = false;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AFlameElementalistHeal(this, owner);
                }
            }
        }
    }
}