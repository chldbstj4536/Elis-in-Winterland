using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AFlash : NoneSkill
            {
                #region Field
                private readonly PFlashDashBuff pFlashDashBuff;

                private ParticleSystem flashFX;
                #endregion

                public PFlashDashBuff PFlashDashBuff => pFlashDashBuff;

                public AFlash(AFlashData data, Unit skillOwner) : base(data, skillOwner)
                {
                    pFlashDashBuff = data.pFlashDashBuffData.Instantiate(skillOwner) as PFlashDashBuff;
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    pFlashDashBuff.AddPassiveSkillToUnit(caster);
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            protected class AFlashData : NoneSkillData
            {
                [BoxGroup("플래시", true, true)]

                [FoldoutGroup("플래시/대쉬 강화 버프")]
                [HideLabel]
                public PFlashDashBuffData pFlashDashBuffData;

                public AFlashData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AFlash(this, owner);
                }
            }
            private class PFlashDashBuff : PassiveSkill
            {
                #region Field
                private AElisDash elisDash;

                private float rangeRate;
                private float dashSpeedRate;
                private int basicPower;
                private float magicPowerRate;
                private PFlashFreezingBuff pFreezingBuff;
                private SphereHitcast hitbox;
                private HashSet<int> hitSet = new HashSet<int>();

                private PoolingComponent flashFXPrefab;
                private AutoReleaseParticlePrefab hitFXPrefab;
                #endregion

                public PFlashDashBuff(PFlashDashBuffData data, Unit skillOwner) : base(data, skillOwner)
                {
                    elisDash = (caster as PlayableUnit).dashSkill.activeSkill as AElisDash;
                    elisDash.OnMovingEvent += OnMovingUpdate;

                    rangeRate = data.rangeRate;
                    dashSpeedRate = data.dashSpeedRate;
                    basicPower = data.basicPower;
                    magicPowerRate = data.magicPowerRate;
                    pFreezingBuff = data.pFreezingBuffData.Instantiate(caster) as PFlashFreezingBuff;
                    hitbox = data.hitbox.Instantiate() as SphereHitcast;
                    hitbox.SetRange(TotalRange);

                    flashFXPrefab = data.flashFXPrefab;
                    hitFXPrefab = data.hitFXPrefab;
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    elisDash.RangeCoefficient *= rangeRate;
                    elisDash.dashSpeedCoef *= dashSpeedRate;

                    PrefabPool.GetObject(flashFXPrefab, true).GetComponent<ParticleSystem>().time = Duration;

                    hitSet.Clear();
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    elisDash.RangeCoefficient /= rangeRate;
                    elisDash.dashSpeedCoef /= dashSpeedRate;
                }
                private void OnMovingUpdate(Vector3 lastPos, Vector3 curPos)
                {
                    if (elisDash.Active && unitsInEffect.Count != 0)
                    {
                        hitbox.direction = lastPos - curPos;
                        hitbox.maxDistance = Vector3.Distance(curPos, lastPos);

                        var hits = Utility.SweepUnit(hitbox, lastPos, Quaternion.identity, true, TargetLayerMask, TargetType);

                        foreach (var hit in hits)
                        {
                            Unit victim = hit.transform.GetComponent<Unit>();
                            if (hitSet.Add(victim.GetInstanceID()))
                            {
                                Vector3 hitpos = Utility.GetHitPoint(lastPos, hit);
                                Attack(victim, DAMAGE_TYPE.NORMAL, hitpos);
                                PrefabPool.GetObject(hitFXPrefab).transform.position = hitpos;
                                pFreezingBuff.AddPassiveSkillToUnit(victim);
                            }
                        }
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return basicPower + (int)(caster.TotalMagicPower * magicPowerRate);
                }
            }
            [System.Serializable]
            protected class PFlashDashBuffData : PassiveSkillData
            {
                [FoldoutGroup("플래시 강화 버프")]
                [LabelText("최대 거리 증가량")]
                public float rangeRate;
                [FoldoutGroup("플래시 강화 버프")]
                [LabelText("대쉬속도 증가량")]
                public float dashSpeedRate;
                [FoldoutGroup("플래시 강화 버프")]
                [LabelText("기본 주문 공격력")]
                public int basicPower;
                [FoldoutGroup("플래시 강화 버프")]
                [LabelText("주문력 계수")]
                public float magicPowerRate;
                [BoxGroup("플래시 강화 버프/빙결 효과", true, true)]
                [HideLabel]
                public PFlashFreezingBuffData pFreezingBuffData;
                [FoldoutGroup("플래시 강화 버프/피격범위")]
                [HideLabel]
                public SphereHitcast hitbox;

                [FoldoutGroup("플래시 강화 버프/이펙트")]
                [LabelText("버프 효과 이펙트")]
                public PoolingComponent flashFXPrefab;
                [FoldoutGroup("플래시 강화 버프/이펙트")]
                [LabelText("피격 효과")]
                public AutoReleaseParticlePrefab hitFXPrefab;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PFlashDashBuff(this, owner);
                }
            }
            private class PFlashSpeedBuff : PassiveSkill
            {
                #region Field
                private float moveSpeedRate;
                #endregion

                public PFlashSpeedBuff(PFlashSpeedBuffData data, Unit skillOwner) : base(data, skillOwner)
                {
                    moveSpeedRate = data.moveSpeedRate;
                }
                protected override void FindUnitsInEffectRange()
                {
                    newUnits.Add(caster);
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef *= moveSpeedRate;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.mc.MoveSpeedCoef /= moveSpeedRate;
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            protected class PFlashSpeedBuffData : PassiveSkillData
            {
                [BoxGroup("플래시")]
                [LabelText("이동속도 증가량")]
                public float moveSpeedRate;
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PFlashSpeedBuff(this, owner);
                }
            }
            private class PFlashFreezingBuff : PFreezing
            {
                public PFlashFreezingBuff(PFlashFreezingBuffData data, Unit skillOwner) : base(data, skillOwner) { }
            }
            [System.Serializable]
            protected class PFlashFreezingBuffData : PFreezingData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PFlashFreezingBuff(this, owner);
                }
            }
        }
    }
}