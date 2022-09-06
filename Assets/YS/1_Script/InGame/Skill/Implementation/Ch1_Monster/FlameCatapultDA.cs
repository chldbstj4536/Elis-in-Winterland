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
                    
                    // 타워가 없다면, 코어를 공격
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
                [BoxGroup("불꽃 투석기 기본공격", true, true)]
                [LabelText("사거리 랜덤값"), Tooltip("스폰시 기본사거리에 사거리 랜덤값+- 만큼 더해진 값이 기본사거리로 정해집니다."), Min(0.0f)]
                public float randomRangeOffset;
                [BoxGroup("불꽃 투석기 기본공격")]
                [LabelText("마법 피해량 계수"), Tooltip("시전자의 최종 마법 피해량의 추가피해량을 결정합니다.")]
                public float magicPowerRate;
                [BoxGroup("불꽃 투석기 기본공격")]
                [LabelText("곡사형 투사체 프리팹"), Tooltip("공격 시 생성할 투사체 프리팹을 설정합니다.")]
                public BezierProjectile projectilePrefab;
                [BoxGroup("불꽃 투석기 기본공격"), Min(0.0001f)]
                [LabelText("곡사형 투사체 속도"), Tooltip("투사체의 속도를 설정합니다.")]
                public float projectileSpeed = 1.0f;
                [BoxGroup("불꽃 투석기 기본공격")]
                [LabelText("공격 위치 표시 효과 프리팹"), Tooltip("투사체가 떨어지는 위치를 알려주는 효과입니다.")]
                public PoolingComponent hitRangeFXPrefab;
                [BoxGroup("불꽃 투석기 기본공격/피해 대상 설정", true, true)]
                [LabelText("피해 대상 유닛의 타입"), Tooltip("투사체가 도착 후 피격판정을 받는 유닛 타입"), EnumToggleButtons]
                public UNIT_FLAG hitUnitType;

                public AFlameCatapultDAData()
                {
                    // 스킬의 초기값 설정 (선택)

                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 피격 대상 레이어
                    targetLayerMask = TARGET_LAYER_MASK.ENEMY;
                    // 기본공격여부
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