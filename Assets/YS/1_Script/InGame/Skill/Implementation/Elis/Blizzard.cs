using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ABlizzard : VectorSkill
            {
                #region Field
                private readonly float duringTime;
                private readonly float basicPower;
                private readonly float magicPowerRate;
                private readonly float tick;
                private PBlizzardSlow pSlow;
                private BoxHitcast hitbox;
                private static readonly Vector3 HitboxOffset = new Vector3(1.0f, 0.0f, 0.0f);

                private ParticleSystem areaFX;
                private readonly AutoReleaseParticlePrefab hitFXPrefab;
                #endregion

                public ABlizzard(ABlizzardData data, Unit skillOwner) : base(data, skillOwner)
                {
                    duringTime = data.duringTime;
                    basicPower = data.basicPower;
                    magicPowerRate = data.magicPowerRate;
                    tick = data.tick;
                    pSlow = data.pSlowData.Instantiate(caster) as PBlizzardSlow;
                    hitbox = new BoxHitcast(HitboxOffset, Vector3.right, TotalRange, new Vector3(0.0f, 1.0f, GameManager.HalfSizeField_Z), Quaternion.identity);

                    areaFX = PrefabPool.GetObject(data.areaFXPrefab, false).GetComponent<ParticleSystem>();
                    hitFXPrefab = data.hitFXPrefab;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    areaFX.gameObject.SetActive(true);
                    caster.StartCoroutine(TickAttack());
                }
                protected override void SetRange()
                {
                    base.SetRange();

                    hitbox.SetRange(TotalRange);
                }
                private IEnumerator TickAttack()
                {
                    float remainDuration = duringTime;
                    Vector3 origin = caster.transform.position;
                    Quaternion rot = dest.x - caster.transform.position.x > 0.0f ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up);

                    while (remainDuration > 0.0f)
                    {
                        var hits = Utility.SweepUnit(hitbox, origin, rot, true, TargetLayerMask, TargetType);

                        foreach (var hit in hits)
                        {
                            var victim = hit.transform.GetComponent<Unit>();
                            Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                            pSlow.AddPassiveSkillToUnit(victim);
                            PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;
                        }

                        yield return CachedWaitForSeconds.Get(tick);

                        remainDuration -= tick;
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return (int)(basicPower + magicPowerRate * caster.TotalMagicPower);
                }
            }
            protected class ABlizzardData : VectorSkillData
            {
                [BoxGroup("���ڵ�", true, true)]
                [LabelText("���ӽð�"), Tooltip("��ų�� ������ ���ӽð��Դϴ�")]
                public float duringTime;
                [BoxGroup("���ڵ�")]
                [LabelText("�⺻ �ֹ����ݷ�"), Tooltip("��ų�� �⺻ �ֹ����ݷ��Դϴ�")]
                public int basicPower;
                [BoxGroup("���ڵ�")]
                [LabelText("�ֹ��� ���"), Tooltip("��ų�� �ֹ��� ����Դϴ�")]
                public float magicPowerRate;
                [BoxGroup("���ڵ�")]
                [LabelText("���� �ֱ�(cycle) �ð�"), Tooltip("�ǰ����� �ð������Դϴ�")]
                public float tick;
                [BoxGroup("���ڵ�/������", false)]
                [HideLabel]
                public PBlizzardSlowData pSlowData;

                [BoxGroup("���ڵ�/����Ʈ", true, true)]
                [LabelText("���� ����Ʈ"), Tooltip("�ǰݹ��� ����Ʈ")]
                public PoolingComponent areaFXPrefab;
                [BoxGroup("���ڵ�/����Ʈ")]
                [LabelText("�ǰ� ����Ʈ"), Tooltip("�ǰ� �� ����Ʈ")]
                public AutoReleaseParticlePrefab hitFXPrefab;

                public ABlizzardData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
                    isDefaultAttack = false;
                }
#if UNITY_EDITOR
                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    base.DrawGizmos(origin, rot);

                    var hitbox = new BoxHitcast(new Vector3(1.0f, 0.0f, 0.0f), Vector3.right, range, new Vector3(0.0f, 1.0f, GameManager.HalfSizeField_Z), Quaternion.identity);
                    hitbox.SetRange(range);
                    hitbox.DrawGizmos(origin, rot);
                }
#endif
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ABlizzard(this, owner);
                }
            }
            /// <summary>
            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
            /// (�ʱ�ȭ�� PSKILL_NAME_DATA���� ó��)
            /// </summary>
            private class PBlizzardSlow : PSlow
            {
                public PBlizzardSlow(PBlizzardSlowData data, Unit skillOwner) : base(data, skillOwner) { }
            }
            [System.Serializable]
            protected class PBlizzardSlowData : PSlowData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PBlizzardSlow(this, owner);
                }
            }
        }
    }
}