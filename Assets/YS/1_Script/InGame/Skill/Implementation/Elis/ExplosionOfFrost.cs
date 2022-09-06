using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// 스킬에 필요한 변수 및 작동 방식만 정의
            /// </summary>
            private class AExplosionOfFrost : VectorSkill
            {
                #region Field
                private readonly LinearProjectile projectilePrefab;
                private int hitDmg;
                private float hitDmgCoef;
                private PExplosionOfFrostBurn pFreezeHit;
                private int explosionDmg;
                private float explosionDmgCoef;
                private PExplosionOfFrostBurn pFreezeExplosion;
                private LinearProjectile curProjectile;

                public delegate void OnExplosion(Vector3 explosionPos);
                public event OnExplosion OnExplosionEvent;
                #endregion

                public AExplosionOfFrost(AExplosionOfFrostData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as LinearProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                    hitDmg = data.hitDmg;
                    hitDmgCoef = data.hitDmgCoef;
                    explosionDmg = data.explosionDmg;
                    explosionDmgCoef = data.explosionDmgCoef;

                    PExplosionOfFrostBurnData freezeData = new PExplosionOfFrostBurnData();
                    freezeData.remainTimeType = SKILL_REMAINTIME_TYPE.RESET;
                    freezeData.duration = data.hitFreezeTime;
                    pFreezeHit = freezeData.Instantiate(skillOwner) as PExplosionOfFrostBurn;
                    freezeData.duration = data.explosionFreezeTime;
                    pFreezeExplosion = freezeData.Instantiate(skillOwner) as PExplosionOfFrostBurn;
                }

                public override SKILL_ERROR_CODE CastCombo()
                {
                    curProjectile.SetArrive();

                    return SKILL_ERROR_CODE.NO_ERR;
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    curProjectile = PrefabPool.GetObject(projectilePrefab).GetComponent<LinearProjectile>();

                    curProjectile.SetProjectile(this, caster.transform.position, dest, TargetLayerMask, TargetType);
                    curProjectile.SetAttackOnHit(GetOnHitTotalDamamge, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                    curProjectile.SetAttackOnArrive(GetOnArriveHitTotalDamamge, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);

                    curProjectile.OnHit += (Unit hit, Vector3 hitPos) =>
                    {
                        pFreezeHit.AddPassiveSkillToUnit(hit);
                    };
                    curProjectile.OnArriveHit += (Unit hit, Vector3 arrivePos) =>
                    {
                        pFreezeExplosion.AddPassiveSkillToUnit(hit);
                    };
                    curProjectile.OnArrive += (Vector3 arrivePos) =>
                    {
                        curCombo = 0;
                        OnExplosionEvent?.Invoke(arrivePos);
                    };
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
                private int GetOnHitTotalDamamge(Unit victim)
                {
                    return hitDmg + (int)(caster.TotalMagicPower * hitDmgCoef);
                }
                private int GetOnArriveHitTotalDamamge(Unit victim)
                {
                    return explosionDmg + (int)(caster.TotalMagicPower * explosionDmgCoef);
                }
            }
            protected class AExplosionOfFrostData : VectorSkillData
            {
                [BoxGroup("서리의 폭발", true, true)]
                [LabelText("투사체 프리팹"), Tooltip("스킬 사용시 생성할 투사체 프리팹을 설정합니다.")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("서리의 폭발")]
                [LabelText("비행 속도"), Tooltip("투사체가 비행하는 속도")]
                public float projectileSpeed;
                [BoxGroup("서리의 폭발")]
                [LabelText("기본 피해량"), Tooltip("투사체가 비행하는 동안에 닿는 적들에게 가하는 기본피해량")]
                public int hitDmg;
                [BoxGroup("서리의 폭발")]
                [LabelText("기본 피해 주문력계수"), Tooltip("투사체가 비행하는 동안에 닿는 적들에게 가하는 피해량의 주문력계수")]
                public float hitDmgCoef;
                [BoxGroup("서리의 폭발")]
                [LabelText("기본 빙결 시간"), Tooltip("투사체가 비행하는 동안의 닿는 적들에게 가하는 상태이상 빙결 지속시간")]
                public float hitFreezeTime;

                [BoxGroup("서리의 폭발")]
                [LabelText("폭발 피해량"), Tooltip("투사체가 폭발할 때 닿는 적들에게 가하는 피해량")]
                public int explosionDmg;
                [BoxGroup("서리의 폭발")]
                [LabelText("폭발 피해 주문력계수"), Tooltip("투사체가 폭발할 때 닿는 적들에게 가하는 피해량의 문력계수")]
                public float explosionDmgCoef;
                [BoxGroup("서리의 폭발")]
                [LabelText("폭발 빙결 시간"), Tooltip("투사체가 폭발할 때 닿는 적들에게 가하는 상태이상 빙결 지속시간")]
                public float explosionFreezeTime;

                public AExplosionOfFrostData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AExplosionOfFrost(this, owner);
                }
            }

            private class PExplosionOfFrostBurn : PFreezing
            {
                public PExplosionOfFrostBurn(PExplosionOfFrostBurnData data, Unit skillOwner) : base(data, skillOwner) { }
            }
            [System.Serializable]
            protected class PExplosionOfFrostBurnData : PFreezingData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PExplosionOfFrostBurn(this, owner);
                }
            }
        }
    }
}