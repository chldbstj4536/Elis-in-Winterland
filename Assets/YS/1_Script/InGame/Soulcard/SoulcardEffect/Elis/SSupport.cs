using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public partial class BaseSkill
            {
                private class SSupport : SoulCardEffect
                {
                    private readonly int baseDmg;
                    private readonly float magicPowerCoef;

                    private ASupport baseSkill;

                    public SSupport(SSupportData data, PlayableUnit skillOwner) : base(data, skillOwner)
                    {
                        baseDmg = data.baseDmg;

                        foreach (var skill in skillOwner.skills)
                        {
                            if (skill.ASkill is ASupport)
                            {
                                baseSkill = skill.ASkill as ASupport;
                                break;
                            }
                        }

                        inRangeHitboxes = new IHitBox[baseSkill.PBuff.inRangeHitboxes.Length];
                        for (int i = 0; i < baseSkill.PBuff.inRangeHitboxes.Length; ++i)
                            inRangeHitboxes[i] = baseSkill.PBuff.inRangeHitboxes[i].Instantiate();

                        baseSkill.PBuff.OnChangedActive += () =>
                        {
                            Active = baseSkill.PBuff.Active;
                        };
                        Active = baseSkill.PBuff.Active;
                    }
                    protected override void InstantEffect() { }
                    protected override void OnTickPassiveEffect(Unit unit, int tickStack)
                    {
                        Attack(unit, DAMAGE_TYPE.DOT, unit.transform.position);
                    }
                    protected override int GetTotalDamage(Unit victim)
                    {
                        return baseDmg + (int)(magicPowerCoef * caster.TotalMagicPower);
                    }
                }

                [System.Serializable]
                private class SSupportData : SoulCardEffectData
                {
                    [BoxGroup("지원 소울카드", true, true)]
                    [LabelText("기본 피해량")]
                    public int baseDmg;
                    [BoxGroup("지원 소울카드")]
                    [LabelText("마법 공격력 계수")]
                    public float magicPowerCoef;

                    public SSupportData()
                    {
#if UNITY_EDITOR
                        ShowPPSetting = false;
                        PP_UseAreaStack = false;
                        PP_UseDefaultTarget = true;
                        PP_UseInRangeHitboxes = false;
                        PP_UseRange = false;
                        PP_UseRemainTimeType = false;
                        PP_UseRestrictionFlag = false;
                        PP_UseTick = true;
                        PP_UseDuration = false;
#endif
                    }

                    public override BaseSkill Instantiate(Unit owner)
                    {
                        return new SSupport(this, owner as PlayableUnit);
                    }
                }
            }
        }
    }
}