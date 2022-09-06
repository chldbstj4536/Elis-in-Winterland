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
            private class ASnowball : VectorSkill
            {
                #region Field
                private readonly int baseDmg;
                private readonly float baseDmgMagicPowerRate;
                private readonly int maxDmg;
                private readonly float veloDmg;
                private readonly float veloScale;
                private readonly float maxScale;
                private readonly float ballSpeed;
                private readonly int maxHitCount;

                public SnowballProjectile snowballPrefab;
                public bool isEnhanced;
                #endregion

                public ASnowball(ASnowballData data, Unit skillOwner) : base(data, skillOwner)
                {
                    baseDmg = data.baseDmg;
                    baseDmgMagicPowerRate = data.baseDmgMagicPowerRate;
                    maxDmg = data.maxDmg;
                    veloDmg = data.veloDmg;
                    veloScale = data.veloScale;
                    maxScale = data.maxScale;
                    ballSpeed = data.ballSpeed;
                    maxHitCount = data.maxHitCount;

                    snowballPrefab = data.projectilePrefab.Instantiate(false) as SnowballProjectile;
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                    CooltimeStart();
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    var snowball = PrefabPool.GetObject(snowballPrefab).GetComponent<SnowballProjectile>();

                    snowball.SetProjectile(this, caster.transform.position, dest, TargetLayerMask, TargetType);
                    snowball.Speed = ballSpeed;
                    snowball.SetAttackOnHit(null, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                    if (isEnhanced)
                    {
                        snowball.SetAttackOnArrive(null, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                        snowball.SetSnowballProjectile(maxDmg, 1.0f, 0.0f, maxDmg, 1.0f, 1.0f, 1);
                    }
                    else
                        snowball.SetSnowballProjectile(baseDmg, baseDmgMagicPowerRate, veloDmg, maxDmg, veloScale, maxScale, maxHitCount);
                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            protected class ASnowballData : VectorSkillData
            {
                [FoldoutGroup("����캼", true), Min(0)]
                [LabelText("�⺻ ���ط�"), Tooltip("��ų�� �⺻ ���ط�")]
                public int baseDmg;
                [FoldoutGroup("����캼"), Min(0.0f)]
                [LabelText("�ֹ��� ���"), Tooltip("�⺻ ���ط��� �ֹ��� ���")]
                public float baseDmgMagicPowerRate;
                [FoldoutGroup("����캼"), Min(0)]
                [LabelText("�ִ� ���ط�"), Tooltip("��ų�� �ִ� ���ط�, �ش簪�� �ʰ��� �� �����ϴ�.")]
                public int maxDmg;
                [FoldoutGroup("����캼"), Min(0.0f)]
                [LabelText("�ʴ� ���� ������"), Tooltip("�� �ʸ��� �����Ǵ� ���ط��� ���� ��")]
                public float veloDmg;
                [FoldoutGroup("����캼"), Min(0.0f)]
                [LabelText("�ʴ� ũ�� ������"), Tooltip("�� �ʸ��� �����Ǵ� ũ���� ���� ��")]
                public float veloScale;
                [FoldoutGroup("����캼"), Min(0.0f)]
                [LabelText("�������� �ִ� ũ��"), Tooltip("�����̰� �ִ�� Ŀ�� �� �ִ� �Ѱ谪")]
                public float maxScale;
                [FoldoutGroup("����캼"), Min(0.0f)]
                [LabelText("�������� �ӵ�"), Tooltip("������ ����ü�� �ӵ��� ȸ���ӵ�")]
                public float ballSpeed;
                [FoldoutGroup("����캼"), Min(0)]
                [LabelText("�ǰ� �ִ� Ƚ��"), Tooltip("�ǰݰ����� �ִ� ��� ��")]
                public int maxHitCount;

                [FoldoutGroup("����캼")]
                [LabelText("������ ����ü ������"), Tooltip("������ ����ü ������")]
                public SnowballProjectile projectilePrefab;

                public ASnowballData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ASnowball(this, owner);
                }
            }
        }
    }
}