using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
            /// (�ʱ�ȭ�� ABlueFlameTowerDA_DATA���� ó��)
            /// </summary>
            private class ABlueFlameTowerDA : NoneSkill
            {
                #region Field
                private readonly float duration;
                private readonly float tickInterval;
                private readonly BoxHitcast hitbox;
                private readonly GameObject skillFX;
                private readonly AutoReleaseParticlePrefab hitFXPrefab;

                private float curTime;
                private float curMaxTime;
                #endregion

                public ABlueFlameTowerDA(ABlueFlameTowerDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    duration = data.duration;
                    tickInterval = data.tickInterval;
                    hitbox = data.hitbox.Instantiate() as BoxHitcast;
                    skillFX = PrefabPool.GetObject(data.skillFXPrefab, false);
                    hitFXPrefab = data.hitFXPrefab;

#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        hitbox.DrawGizmos(caster.transform.position, caster.IsLookingLeft ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up));
                    };
#endif
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Vector3 skillFXPos = caster.transform.position;
                    skillFXPos.y = 0.5f;
                    skillFX.transform.position = skillFXPos;
                    skillFX.transform.rotation = Quaternion.Euler(0.0f, caster.IsLookingLeft ? -90.0f : 90.0f, 0.0f);

                    curMaxTime = curTime = 0.0f;

                    skillFX.SetActive(true);
                }
                protected override void OnUpdateAttack()
                {
                    base.OnUpdateAttack();

                    if (curTime >= tickInterval)
                    {
                        curTime -= tickInterval;
                        Trigger();
                    }

                    curTime += Time.deltaTime;
                    curMaxTime += Time.deltaTime;
                    if (curMaxTime >= duration)
                        CompleteTickAttack();
                }
                protected override void OnEndAttack(FSM.State newState)
                {
                    base.OnEndAttack(newState);

                    CooltimeStart();
                    skillFX.SetActive(false);
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, caster.IsLookingLeft ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up), true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();
                        Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                        PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position;
                    }
                }
                protected override void SetRange()
                {
                    base.SetRange();

                    hitbox.SetRange(TotalRange);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class ABlueFlameTowerDAData : NoneSkillData
            {
                [BoxGroup("�����Ҳ� Ÿ��", true, true)]
                [LabelText("���ӽð�"), Min(0.0f)]
                public float duration;
                [BoxGroup("�����Ҳ� Ÿ��")]
                [LabelText("���� ����"), Min(0.0001f)]
                public float tickInterval = 0.2f;
                [BoxGroup("�����Ҳ� Ÿ��/�ǰ� ����", true, true)]
                [HideLabel]
                public BoxHitcast hitbox;
                [BoxGroup("�����Ҳ� Ÿ��/����Ʈ", true, true)]
                [LabelText("��ų ȿ��")]
                public PoolingComponent skillFXPrefab;
                [BoxGroup("�����Ҳ� Ÿ��/����Ʈ")]
                [LabelText("�ǰ� ȿ��")]
                public AutoReleaseParticlePrefab hitFXPrefab;

                public ABlueFlameTowerDAData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.TICK;
                    // �⺻���ݿ���
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ABlueFlameTowerDA(this, owner);
                }
#if UNITY_EDITOR
                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    base.DrawGizmos(origin, rot);

                    hitbox.SetRange(range);
                    hitbox.DrawGizmos(origin, rot);
                }
#endif
            }
        }
    }
}