using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ARageOfFrost : VectorSkill
            {
                private readonly int magicDamage;
                private readonly float magicPowerRate;
                private readonly float maxRadius;
                private readonly float minRadius;
                private readonly float increaseSpeed;
                private readonly uint projectileCount;
                private readonly float duration;

                private float curRadius;

                private GameObject castingFX;
                private GameObject rangeFX;
                private GameObject attackFX;

                private PoolingComponent castingFxPrefab;
                private PoolingComponent rangeFxPrefab;
                private RageOfFrostGen attackFxPrefab;

                public delegate void OnFinish(Vector3 spawnPos, float radius);
                public event OnFinish OnFinishEvent;

                public ARageOfFrost(ARageOfFrostData data, Unit skillOwner) : base(data, skillOwner)
                {
                    maxRadius = data.maxRadius;
                    curRadius = minRadius = data.minRadius;
                    increaseSpeed = data.increaseSpeed;
                    duration = data.duration;

                    magicDamage = data.magicDamage;
                    magicPowerRate = data.magicPowerRate;
                    projectileCount = data.projectileCount;

                    castingFxPrefab = data.castingFxPrefab;
                    rangeFxPrefab = data.rangeFxPrefab;
                    attackFxPrefab = data.attackFxPrefab;

                    castingFX = PrefabPool.GetObject(castingFxPrefab, false);
                    rangeFX = PrefabPool.GetObject(rangeFxPrefab, false);
                }

                protected override void OnBeginCasting()
                {
                    rangeFX.SetActive(true);
                    rangeFX.transform.localScale = new Vector3(TotalRange, 1.0f, TotalRange);
                    rangeFX.transform.position = caster.transform.position;

                    castingFX.SetActive(true);
                    curRadius = minRadius;
                    castingFX.transform.localScale = new Vector3(curRadius, 1.0f, minRadius);
                    castingFX.transform.position = dest;

                    base.OnBeginCasting();
                }

                protected override void OnUpdateCasting()
                {
                    SetDestinationForPlayer();
                    castingFX.transform.position = dest;

                    if (curRadius != maxRadius)
                    {
                        curRadius += increaseSpeed * Time.deltaTime;

                        if (curRadius >= maxRadius)
                            curRadius = maxRadius;

                        castingFX.transform.localScale = new Vector3(curRadius, 1.0f, curRadius);
                    }

                    base.OnUpdateCasting();
                }

                protected override void OnEndCasting(FSM.State newState)
                {
                    base.OnEndCasting(newState);

                    rangeFX.SetActive(false);
                    castingFX.SetActive(false);
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    CooltimeStart();
                    caster.curMP -= ManaCost;

                    Trigger();
                }

                protected override void OnEndAttack(FSM.State newState)
                {
                    base.OnEndAttack(newState);
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    attackFX = PrefabPool.GetObject(attackFxPrefab);

                    attackFX.transform.position = dest;
                    
                    RageOfFrostGen gen = attackFX.GetComponent<RageOfFrostGen>();
                    gen.Initialize(this, curRadius, duration, projectileCount, (Unit victim) =>
                    {
                        Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                    });
                    gen.Play();
                    gen.OnEmissionEndEvent += () =>
                    {
                        OnFinishEvent?.Invoke(gen.transform.position + gen.offset, curRadius);
                    };
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return 1;
                    //return magicDamage + (int)(magicPowerRate * caster.TotalMagicPower);
                }
            }
            protected class ARageOfFrostData : VectorSkillData
            {
                [FoldoutGroup("혹한의 분노", true)]
                [LabelText("기본 피해량"), Tooltip("스킬의 기본 피해량")]
                public int magicDamage;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("주문력 계수"), Tooltip("스킬의 주문력 계수")]
                public float magicPowerRate;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("피격범위 최대범위"), Tooltip("스킬의 피격범위가 확대되는 최대범위")]
                public float maxRadius = 2.0f;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("기본 피격범위"), Tooltip("기본 피격범위")]
                public float minRadius = 1.0f;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("피격범위 증가속도"), Tooltip("스킬의 피격범위 증가속도")]
                public float increaseSpeed = 2.5f;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("투사체의 수"), Tooltip("투사체가 떨어지는 시간")]
                public uint projectileCount = 15;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("스킬 지속 시간"), Tooltip("스킬 지속 시간")]
                public float duration = 4.0f;

                [FoldoutGroup("혹한의 분노")]
                [LabelText("현재 공격 범위 이펙트 프리팹"), Tooltip("현재 공격 범위를 보여주는 이펙트")]
                public PoolingComponent castingFxPrefab;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("최대 공격 사거리 이펙트 프리팹"), Tooltip("최대 공격 사거리를 보여주는 이펙트")]
                public PoolingComponent rangeFxPrefab;
                [FoldoutGroup("혹한의 분노")]
                [LabelText("혹한의 분노 스킬 프리팹"), Tooltip("투사체를 떨어뜨려 공격판정을 입히는 스킬에 대한 프리팹")]
                public RageOfFrostGen attackFxPrefab;
                private ARageOfFrostData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.CHARGING;
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ARageOfFrost(this, owner);
                }
            }
        }
    }
}