using System.Collections.Generic;
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
            /// (�ʱ�ȭ�� ASKILL_NAME_DATA���� ó��)
            /// </summary>
            private class AEverFrost : NoneSkill
            {
                #region Field
                private int magicHeal;
                private float magicHealRate;
                private int magicDamage;
                private float magicDamageRate;
                private IHitBox hitbox;

                private AutoReleaseParticlePrefab skillFXPrefab;
                private AutoReleaseParticlePrefab hitFXPrefab;
                private AutoReleaseParticlePrefab healFXPrefab;
                #endregion

                public AEverFrost(AEverFrostData data, Unit skillOwner) : base(data, skillOwner)
                {
                    magicHeal = data.magicHeal;
                    magicHealRate = data.magicHealRate;
                    magicDamage = data.magicDamage;
                    magicDamageRate = data.magicDamageRate;
                    hitbox = data.hitbox.Instantiate();
                    hitbox.SetRange(TotalRange);

                    skillFXPrefab = data.skillFXPrefab;
                    hitFXPrefab = data.hitFXPrefab;
                    healFXPrefab = data.healFXPrefab;
#if UNITY_EDITOR
                    caster.OnGizmosEvent += () =>
                    {
                        hitbox.SetRange(TotalRange);
                        hitbox.DrawGizmos(caster.transform.position, Quaternion.identity);
                    };
#endif
                }

                //--------------------------------------//
                // ��ų�� �ʿ��� �̺�Ʈ �������̵�           //
                //--------------------------------------//
                // On(Begin/Update/End)SelectingTarget  //
                // On(Begin/Update/End)Casting          //
                // On(Begin/Update/End)Attack           //
                //--------------------------------------//
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                    CooltimeStart();
                    caster.curMP -= ManaCost;
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var hits = Utility.SweepUnit(hitbox, caster.transform.position, Quaternion.identity, true, TargetLayerMask, TargetType);

                    foreach (var hit in hits)
                    {
                        Unit victim = hit.transform.GetComponent<Unit>();

                        if (victim.gameObject.layer == caster.gameObject.layer)
                        {
                            Attack(victim, DAMAGE_TYPE.HEAL, victim.transform.position);
                            PrefabPool.GetObject(healFXPrefab).transform.position = victim.transform.position;
                        }
                        else
                        {
                            Attack(victim, DAMAGE_TYPE.NORMAL, victim.transform.position);
                            PrefabPool.GetObject(hitFXPrefab).transform.position = victim.transform.position; 
                        }
                    }

                    var fx = PrefabPool.GetObject(skillFXPrefab, true);
                    Vector3 pos = caster.transform.position;
                    pos.y = 3.0f;
                    fx.transform.position = pos;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    if (victim.gameObject.layer == caster.gameObject.layer)
                        return -(int)(magicHeal + magicHealRate * caster.TotalMagicPower);
                    else
                        return (int)(magicDamage + magicDamageRate * caster.TotalMagicPower);
                }
            }
            protected class AEverFrostData : NoneSkillData
            {
                [FoldoutGroup("���⼭��", true)]
                [LabelText("ȸ����"), Tooltip("��ų ���� ���� �Ʊ����� ����Ǵ� ȸ����")]
                public int magicHeal;
                [FoldoutGroup("���⼭��")]
                [LabelText("ȸ���� ���"), Tooltip("�÷��̾��� �������ݷ¿� ���� ȸ������ �����ϴ� ����")]
                public float magicHealRate;
                [FoldoutGroup("���⼭��")]
                [LabelText("���ط�"), Tooltip("��ų ���� ���� �������� ����Ǵ� ���ط�")]
                public int magicDamage;
                [FoldoutGroup("���⼭��")]
                [LabelText("���ط� ���"), Tooltip("�÷��̾��� �������ݷ¿� ���� ���ط��� �����ϴ� ����")]
                public float magicDamageRate;
                [FoldoutGroup("���⼭��/���� ����"), SerializeReference]
                [HideLabel, Tooltip("��ų ��Ʈ ������ ����")]
                public IHitBox hitbox;

                [FoldoutGroup("���⼭��/����Ʈ")]
                [LabelText("��ų ȿ�� ������"), Tooltip("��ų�� ȿ�� ����Ʈ ������")]
                public AutoReleaseParticlePrefab skillFXPrefab;
                [FoldoutGroup("���⼭��/����Ʈ")]
                [LabelText("�ǰ� ȿ��"), Tooltip("�ǰ� ȿ�� ������")]
                public AutoReleaseParticlePrefab hitFXPrefab;
                [FoldoutGroup("���⼭��/����Ʈ")]
                [LabelText("ġ�� ȿ��"), Tooltip("ġ�� ȿ�� ������")]
                public AutoReleaseParticlePrefab healFXPrefab;

                public AEverFrostData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AEverFrost(this, owner);
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