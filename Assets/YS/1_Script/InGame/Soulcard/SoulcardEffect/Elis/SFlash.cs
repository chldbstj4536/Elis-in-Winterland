using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static YS.Utility;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SFlash : SoulCardEffect
            {
                private readonly float armorCoef;
                private readonly float physxPowerCoef;
                private readonly float magicPowerCoef;

                private readonly PFlashDashBuff pFlashBuff;

                public SFlash(SFlashData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    armorCoef = data.armorCoef;
                    physxPowerCoef = data.physxPowerCoef;
                    magicPowerCoef = data.magicPowerCoef;

                    foreach (var skill in skillOwner.skills)
                    {
                        if (skill.ASkill is AFlash)
                        {
                            pFlashBuff = (skill.ASkill as AFlash).PFlashDashBuff;
                            break;
                        }
                    }

                    inRangeHitboxes = new IHitBox[caster.hitboxCols.Length];
                    for (int i = 0; i < caster.hitboxCols.Length; ++i)
                        inRangeHitboxes[i] = ConvertColliderToHitbox(caster.hitboxCols[i]);
                }
                protected override void InstantEffect() { }
                protected override void FindUnitsInEffectRange()
                {
                    if (!pFlashBuff.IsUnitInPassive(caster))
                        return;

                    base.FindUnitsInEffectRange();
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    switch ((unit as Tower).Category)
                    {
                        case TOWER_CATEGORY.ATTACK:
                            unit.PhysicsPowerCoef *= physxPowerCoef;
                            break;
                        case TOWER_CATEGORY.DEFENSE:
                            unit.ArmorAdd *= armorCoef;
                            break;
                        case TOWER_CATEGORY.SUPPORT:
                            unit.MagicPowerCoef *= magicPowerCoef;
                            break;
                    }
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    switch ((unit as Tower).Category)
                    {
                        case TOWER_CATEGORY.ATTACK:
                            unit.PhysicsPowerCoef /= physxPowerCoef;
                            break;
                        case TOWER_CATEGORY.DEFENSE:
                            unit.ArmorAdd /= armorCoef;
                            break;
                        case TOWER_CATEGORY.SUPPORT:
                            unit.MagicPowerCoef /= magicPowerCoef;
                            break;
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SFlashData : SoulCardEffectData
            {
                [BoxGroup("플래시 소울카드", true, true)]
                [LabelText("방어타워 방여력 증가량")]
                public float armorCoef;
                [BoxGroup("플래시 소울카드")]
                [LabelText("공격타워 공격력 증가량")]
                public float physxPowerCoef;
                [BoxGroup("플래시 소울카드")]
                [LabelText("버프타워 주문력 증가량")]
                public float magicPowerCoef;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SFlash(this, owner as PlayableUnit);
                }
            }
        }
    }
}