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
                [FoldoutGroup("Ȥ���� �г�", true)]
                [LabelText("�⺻ ���ط�"), Tooltip("��ų�� �⺻ ���ط�")]
                public int magicDamage;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("�ֹ��� ���"), Tooltip("��ų�� �ֹ��� ���")]
                public float magicPowerRate;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("�ǰݹ��� �ִ����"), Tooltip("��ų�� �ǰݹ����� Ȯ��Ǵ� �ִ����")]
                public float maxRadius = 2.0f;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("�⺻ �ǰݹ���"), Tooltip("�⺻ �ǰݹ���")]
                public float minRadius = 1.0f;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("�ǰݹ��� �����ӵ�"), Tooltip("��ų�� �ǰݹ��� �����ӵ�")]
                public float increaseSpeed = 2.5f;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("����ü�� ��"), Tooltip("����ü�� �������� �ð�")]
                public uint projectileCount = 15;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("��ų ���� �ð�"), Tooltip("��ų ���� �ð�")]
                public float duration = 4.0f;

                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("���� ���� ���� ����Ʈ ������"), Tooltip("���� ���� ������ �����ִ� ����Ʈ")]
                public PoolingComponent castingFxPrefab;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("�ִ� ���� ��Ÿ� ����Ʈ ������"), Tooltip("�ִ� ���� ��Ÿ��� �����ִ� ����Ʈ")]
                public PoolingComponent rangeFxPrefab;
                [FoldoutGroup("Ȥ���� �г�")]
                [LabelText("Ȥ���� �г� ��ų ������"), Tooltip("����ü�� ����߷� ���������� ������ ��ų�� ���� ������")]
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