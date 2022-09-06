using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AFlameCatapultDA : VectorSkill
            {
                #region Field
                private readonly PoolingComponent hitRangeFXPrefab;
                private readonly BezierProjectile projectilePrefab;

                private readonly float magicPowerRate;

                private readonly UNIT_FLAG hitUnitType;

                private GameObject hitRangeFX;
                #endregion

                #region Methods
                public AFlameCatapultDA(AFlameCatapultDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    hitRangeFXPrefab = data.hitRangeFXPrefab;
                    
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as BezierProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;

                    hitUnitType = data.hitUnitType;

                    magicPowerRate = data.magicPowerRate;

                    RangeAdditional = Random.Range(-data.randomRangeOffset, data.randomRangeOffset);
                    SetRange();
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

                    Vector3 randomDest;
                    var hitList = TileManager.Instance.GetZLaneTowers((caster as Monster).ZLaneNum);
                    
                    // Ÿ���� ���ٸ�, �ھ ����
                    if (hitList.Count == 0)
                        randomDest = dest;
                    else
                        randomDest = hitList[Random.Range(0, hitList.Count)].transform.position;

                    var p = PrefabPool.GetObject(projectilePrefab).GetComponent<BezierProjectile>();
                    p.SetProjectile(this, caster.transform.position, randomDest, TargetLayerMask, hitUnitType);
                    p.SetAttackOnArrive(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, hitUnitType);
                    p.OnArrive += (Vector3 pos) => { hitRangeFX.GetComponent<PoolingComponent>().Release(); };
                    hitRangeFX = PrefabPool.GetObject(hitRangeFXPrefab);
                    hitRangeFX.transform.position = randomDest;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return caster.TotalPhysicsPower + (int)(caster.TotalMagicPower * magicPowerRate);
                }
                #endregion
            }
            protected class AFlameCatapultDAData : VectorSkillData
            {
                [BoxGroup("�Ҳ� ������ �⺻����", true, true)]
                [LabelText("��Ÿ� ������"), Tooltip("������ �⺻��Ÿ��� ��Ÿ� ������+- ��ŭ ������ ���� �⺻��Ÿ��� �������ϴ�."), Min(0.0f)]
                public float randomRangeOffset;
                [BoxGroup("�Ҳ� ������ �⺻����")]
                [LabelText("���� ���ط� ���"), Tooltip("�������� ���� ���� ���ط��� �߰����ط��� �����մϴ�.")]
                public float magicPowerRate;
                [BoxGroup("�Ҳ� ������ �⺻����")]
                [LabelText("����� ����ü ������"), Tooltip("���� �� ������ ����ü �������� �����մϴ�.")]
                public BezierProjectile projectilePrefab;
                [BoxGroup("�Ҳ� ������ �⺻����"), Min(0.0001f)]
                [LabelText("����� ����ü �ӵ�"), Tooltip("����ü�� �ӵ��� �����մϴ�.")]
                public float projectileSpeed = 1.0f;
                [BoxGroup("�Ҳ� ������ �⺻����")]
                [LabelText("���� ��ġ ǥ�� ȿ�� ������"), Tooltip("����ü�� �������� ��ġ�� �˷��ִ� ȿ���Դϴ�.")]
                public PoolingComponent hitRangeFXPrefab;
                [BoxGroup("�Ҳ� ������ �⺻����/���� ��� ����", true, true)]
                [LabelText("���� ��� ������ Ÿ��"), Tooltip("����ü�� ���� �� �ǰ������� �޴� ���� Ÿ��"), EnumToggleButtons]
                public UNIT_FLAG hitUnitType;

                public AFlameCatapultDAData()
                {
                    // ��ų�� �ʱⰪ ���� (����)

                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �ǰ� ��� ���̾�
                    targetLayerMask = TARGET_LAYER_MASK.ENEMY;
                    // �⺻���ݿ���
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AFlameCatapultDA(this, owner);
                }
            }
        }
    }
}