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
            private class AElisDash : VectorSkill
            {
                #region Field
                private float dashSpeed;
                public float dashSpeedCoef = 1.0f;
                private Vector3 dir;
                public event MoveComponent.OnMoving OnMovingEvent;
                #endregion

                public AElisDash(AElisDashData data, Unit skillOwner) : base(data, skillOwner)
                {
                    dashSpeed = data.dashSpeed;
                    dashSpeedCoef = 1.0f;
                }

                //--------------------------------------//
                // 스킬에 필요한 이벤트 오버라이딩           //
                //--------------------------------------//
                // On(Begin/Update/End)SelectingTarget  //
                // On(Begin/Update/End)Casting          //
                // On(Begin/Update/End)Attack           //
                //--------------------------------------//
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    CooltimeStart();

                    Vector3 pos = caster.transform.position;

                    dir = (dest - pos).normalized;
                }
                protected override void OnUpdateAttack()
                {
                    base.OnUpdateAttack();

                    Vector3 lastPos = caster.transform.position;
                    caster.transform.position += dir * (dashSpeed * dashSpeedCoef * Time.deltaTime);
                    OnMovingEvent?.Invoke(lastPos, caster.transform.position);

                    if (dir == Vector3.zero || Vector3.Dot(dir, dest - caster.transform.position) < 0.0f)
                    {
                        caster.transform.position = dest;
                        caster.mainAnimPlayer.ExitLoopAllTrack();
                    }
                }
                protected override void OnEndAttack(FSM.State newState)
                {
                    base.OnEndAttack(newState);

                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            protected class AElisDashData : VectorSkillData
            {
                [BoxGroup("앨리스 대쉬", true, true)]
                [LabelText("대쉬 스피드")]
                public float dashSpeed;

                public AElisDashData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AElisDash(this, owner);
                }
            }
        }
    }
}