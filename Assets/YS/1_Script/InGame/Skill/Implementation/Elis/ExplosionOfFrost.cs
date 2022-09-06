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
                [BoxGroup("������ ����", true, true)]
                [LabelText("����ü ������"), Tooltip("��ų ���� ������ ����ü �������� �����մϴ�.")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("������ ����")]
                [LabelText("���� �ӵ�"), Tooltip("����ü�� �����ϴ� �ӵ�")]
                public float projectileSpeed;
                [BoxGroup("������ ����")]
                [LabelText("�⺻ ���ط�"), Tooltip("����ü�� �����ϴ� ���ȿ� ��� ���鿡�� ���ϴ� �⺻���ط�")]
                public int hitDmg;
                [BoxGroup("������ ����")]
                [LabelText("�⺻ ���� �ֹ��°��"), Tooltip("����ü�� �����ϴ� ���ȿ� ��� ���鿡�� ���ϴ� ���ط��� �ֹ��°��")]
                public float hitDmgCoef;
                [BoxGroup("������ ����")]
                [LabelText("�⺻ ���� �ð�"), Tooltip("����ü�� �����ϴ� ������ ��� ���鿡�� ���ϴ� �����̻� ���� ���ӽð�")]
                public float hitFreezeTime;

                [BoxGroup("������ ����")]
                [LabelText("���� ���ط�"), Tooltip("����ü�� ������ �� ��� ���鿡�� ���ϴ� ���ط�")]
                public int explosionDmg;
                [BoxGroup("������ ����")]
                [LabelText("���� ���� �ֹ��°��"), Tooltip("����ü�� ������ �� ��� ���鿡�� ���ϴ� ���ط��� ���°��")]
                public float explosionDmgCoef;
                [BoxGroup("������ ����")]
                [LabelText("���� ���� �ð�"), Tooltip("����ü�� ������ �� ��� ���鿡�� ���ϴ� �����̻� ���� ���ӽð�")]
                public float explosionFreezeTime;

                public AExplosionOfFrostData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
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