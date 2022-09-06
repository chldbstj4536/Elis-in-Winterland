using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AFirebatDA : TargetingSkill
            {
                #region Field
                private PoolingComponent hitFXPrefab;

                private BoxHitcast hitbox;
                private PFirebatBurn ignitePSkill;
                #endregion
                public AFirebatDA(AFirebatDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    hitFXPrefab = data.hitFXPrefab;

                    ignitePSkill = data.ignitePSkillData.Instantiate(skillOwner) as PFirebatBurn;

                    hitbox = data.hitbox.Instantiate() as BoxHitcast;
                    hitbox.SetRange(TotalRange);
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                    CooltimeStart();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    Transform casterTr = caster.transform;
                    Utility.FlipUnit(caster, target.position);
                    
                    // �ǰ� ���� �������� ������ �ڽ��� ȸ����Ų �� ��������
                    //float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, (target.position - casterTr.position).normalized)) * Mathf.Rad2Deg;
                    //Quaternion rot = Quaternion.AngleAxis(target.position.x < casterTr.position.x ? -angle : angle, Vector3.up);
                    // ������ �¿� �ڽ��� ��������
                    Quaternion rot = caster.IsLookingLeft ? Quaternion.identity : Quaternion.AngleAxis(180.0f, Vector3.up);


                    var hits = Utility.SweepUnit(hitbox, target.position, rot, true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit taker = hit.transform.GetComponent<Unit>();
                        Attack(taker, DAMAGE_TYPE.NORMAL, hit.transform.position);
                        ignitePSkill.AddPassiveSkillToUnit(taker);
                        PrefabPool.GetObject(hitFXPrefab).transform.position = hit.transform.position;
                    }
#if UNITY_EDITOR
                    caster.OnGizmosEvent = () =>
                    {
                        hitbox.SetRange(TotalRange);
                        hitbox.DrawGizmos(target.position, rot);
                    };
#endif
                }
                protected override void SetRange()
                {
                    base.SetRange();
                    hitbox.SetRange(TotalRange);
                }
            }
            [System.Serializable]
            protected class AFirebatDAData : TargetingSkillData
            {
                [BoxGroup("�Ҳɹ��� �⺻����", true, true)]
                [LabelText("�ǰݽ� ȿ�� ������"), Tooltip("�������� ���ظ� ������ ��Ÿ���� ȿ��")]
                public PoolingComponent hitFXPrefab;
                [FoldoutGroup("�Ҳɹ��� �⺻����/���� ���� ��Ʈ�ڽ�", false)]
                [HideLabel, Tooltip("���� ���ظ� �ִ� ���� ����")]
                public BoxHitcast hitbox;
                [BoxGroup("�Ҳɹ��� �⺻����/ȭ��", false)]
                [HideLabel]
                public PFirebatBurnData ignitePSkillData = new PFirebatBurnData();

                public AFirebatDAData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AFirebatDA(this, owner);
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

            private class PFirebatBurn : PBurn
            {
                public PFirebatBurn(PFirebatBurnData data, Unit skillOwner) : base(data, skillOwner) { }
            }
            [System.Serializable]
            protected class PFirebatBurnData : PBurnData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PFirebatBurn(this, owner);
                }
            }
        }
    }
}