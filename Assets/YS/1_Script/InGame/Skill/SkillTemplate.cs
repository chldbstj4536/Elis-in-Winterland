//using UnityEngine;
//using Sirenix.OdinInspector;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public partial class Skill
//        {
//            /// <summary>
//            /// 스킬에 필요한 변수 및 작동 방식만 정의
//            /// (초기화는 ASKILL_NAME_DATA에서 처리)
//            /// </summary>
//            private class ASKILL_NAME : TypeOfActiveSkill
//            {
//                #region Field
//                // 해당 스킬에 필요한 변수들 선언 (이펙트, 피해량 등)
//                // ★ ASKILL_NAME_DATA와 같아야 한다 (그래야 DATA에 저장된 정보로 초기화 할 수 있다)
//                // type field1
//                // type field2
//                // ...
//                #endregion
//                public ASKILL_NAME(ASKILL_NAME_Data data, Unit skillOwner) : base(data, skillOwner)
//                {
//                    // 해당 스킬에 선언한 변수들을 ASKILL_DATA 정보로 초기화
//                    // field1 = data.field1;
//                    // field2 = data.field2;
//                    // ...
//                }

//                //--------------------------------------//
//                // 스킬에 필요한 이벤트 오버라이딩           //
//                //--------------------------------------//
//                // On(Begin/Update/End)SelectingTarget  //
//                // On(Begin/Update/End)Casting          //
//                // On(Begin/Update/End)Attack           //
//                //--------------------------------------//

//                // ...

//                protected override void Trigger()
//                {
//                    // 발동 시 작동할 로직
//                }
//                protected override void SetRange()
//                {
//                    base.SetRange();
//                }
//            }
//            protected class ASKILL_NAME_Data : TypeOfActiveSkillData
//            {
//                // 해당 스킬에 필요한 변수들 선언 (이펙트, 피해량 등)
//                // ★ ASKILL_NAME과 같아야 한다 (그래야 DATA에 저장된 정보로 초기화 할 수 있다)
//                // type field1
//                // type field2
//                // ...
//                public ASKILL_NAME_Data()
//                {
//                    // 액티브 스킬의 캐스팅 타입
//                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
//                    // 기본공격여부
//                    isDefaultAttack = false;
//                }
//                public override BaseSkill Instantiate(Unit owner)
//                {
//                    return new ASKILL_NAME(this, owner);
//                }
//            }
//            /// <summary>
//            /// 스킬에 필요한 변수 및 작동 방식만 정의
//            /// (초기화는 PSKILL_NAME_DATA에서 처리)
//            /// </summary>
//            private class PSKILL_NAME : TypeOfPassiveSkill
//            {
//                #region Field
//                // 해당 스킬에 필요한 변수들 선언 (이펙트, 피해량 등)
//                // ★ PSKILL_NAME_DATA와 같아야 한다 (그래야 DATA에 저장된 정보로 초기화 할 수 있다)
//                // type field1
//                // type field2
//                // ...
//                #endregion

//                public PSKILL_NAME(PSKILL_NAME_Data data, Unit skillOwner) : base(data, skillOwner)
//                {
//                    // 해당 스킬에 선언한 변수들 초기화
//                    // field1 = data.field1;
//                    // field2 = data.field2;
//                    // ...
//                }
//                protected override void OnBeginPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnEndPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnEmptyInEffect() { }
//                protected override void OnTickPassiveEffect(Unit unit, int tickStack) { }
//            }
//            protected class PSKILL_NAME_Data : TypeOfPassiveSkillData
//            {
//                // 해당 스킬에 필요한 변수들 선언 (이펙트, 피해량 등)
//                // ★ PSKILL_NAME과 같아야 한다 (그래야 DATA에 저장된 정보로 초기화 할 수 있다)
//                // [BoxGroup("스킬 이름", true, true)]
//                // [LabelText("변수 이름"), Tooltip("변수 설명")]
//                // type field1
//                // type field2
//                // ...
//                public PSKILL_NAME_Data()
//                {
//                }
//                public override BaseSkill Instantiate(Unit owner)
//                {
//                    return new PSKILL_NAME(this, owner);
//                }
//            }
//        }
//    }
//}