using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// 스킬에 필요한 변수 및 작동 방식만 정의
            /// (초기화는 ASKILL_NAME_DATA에서 처리)
            /// </summary>
            private class AIceBulwark : NoneSkill
            {
                #region Field
                private BaseShield shieldPrefab;
                private float radius;
                private int maxShieldAmount;
                private float maxShieldAmountCoef;
                private float maxDuration;

                public event BaseShield.OnDestroy OnDestroyEvent;
                #endregion
                public AIceBulwark(AIceBulwarkData data, Unit skillOwner) : base(data, skillOwner)
                {
                    shieldPrefab = data.iceBulwarkPrefab;
                    radius = data.radius;
                    maxShieldAmount = data.maxShieldAmount;
                    maxShieldAmountCoef = data.maxShieldAmountCoef;
                    maxDuration = data.duration;
                }

                //--------------------------------------//
                // 스킬에 필요한 이벤트 오버라이딩           //
                //--------------------------------------//
                // On(Begin/Update/End)SelectingTarget  //
                // On(Begin/Update/End)Casting          //
                // On(Begin/Update/End)Attack           //
                //--------------------------------------//

                // ...

                protected override void Trigger()
                {
                    base.Trigger();

                    var shield = PrefabPool.GetObject(shieldPrefab, false).GetComponent<BaseShield>();

                    shield.SetShield(caster, maxShieldAmount + (int)(maxShieldAmountCoef * caster.TotalMagicPower), maxDuration, TargetLayerMask, TargetType);
                    shield.transform.position = caster.transform.position;
                    shield.transform.localScale = radius * Vector3.one;
                    shield.OnDestroyEvent += OnDestroyEvent;
                    shield.gameObject.SetActive(true);

                    CooltimeStart();
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            protected class AIceBulwarkData : NoneSkillData
            {
                [BoxGroup("얼음방벽", true, true)]
                [LabelText("보호막 프리팹")]
                public BaseShield iceBulwarkPrefab;
                [BoxGroup("얼음방벽"), Min(0.0f)]
                [LabelText("보호막 반지름")]
                public float radius;
                [BoxGroup("얼음방벽"), Min(0.0f)]
                [LabelText("보호막 지속시간")]
                public float duration;
                [BoxGroup("얼음방벽"), Min(0.0f)]
                [LabelText("보호량")]
                public int maxShieldAmount;
                [BoxGroup("얼음방벽"), Min(0.0f)]
                [LabelText("보호량 계수")]
                public float maxShieldAmountCoef;

                public AIceBulwarkData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AIceBulwark(this, owner);
                }
            }
        }
    }
}