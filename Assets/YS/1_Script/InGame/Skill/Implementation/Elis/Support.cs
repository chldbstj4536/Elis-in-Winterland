using System.Collections;
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
            private class ASupport : NoneSkill
            {
                #region Field
                private readonly PSupportBuff pBuff;
                private readonly float buffDuration;
                private readonly GameObject rangeFX;

                private float curBuffDuration;
                #endregion

                #region Properties
                public PSupportBuff PBuff => pBuff;
                #endregion

                public ASupport(ASupportData data, Unit skillOwner) : base(data, skillOwner)
                {
                    pBuff = data.pBuffData.Instantiate(caster) as PSupportBuff;
                    pBuff.Active = false;
                    buffDuration = data.buffDuration;
                    rangeFX = PrefabPool.GetObject(data.rangeFXPrefab, false);
                    rangeFX.transform.SetParent(caster.transform);
                    rangeFX.transform.position = Vector3.zero;
                    rangeFX.transform.localScale = new Vector3(pBuff.TotalRange, 1.0f, pBuff.TotalRange);
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    if (pBuff.Active) curBuffDuration = buffDuration;
                    else caster.StartCoroutine(CheckDuration());
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
                private IEnumerator CheckDuration()
                {
                    pBuff.Active = true;
                    rangeFX.SetActive(true);
                    curBuffDuration = buffDuration;

                    while (curBuffDuration > 0.0f)
                    {
                        yield return CachedWaitForSeconds.Get(0.1f);
                        curBuffDuration -= 0.1f;
                    }

                    pBuff.Active = false;
                    rangeFX.SetActive(false);
                }
            }
            protected class ASupportData : NoneSkillData
            {
                [BoxGroup("지원", true, true)]
                [HideLabel]
                public PSupportBuffData pBuffData;
                [BoxGroup("지원", true, true)]
                [LabelText("지속시간"), Min(0.0f)]
                public float buffDuration;
                [BoxGroup("지원")]
                [LabelText("범위 이펙트"), Tooltip("효과 범위를 보여주는 효과 이펙트")]
                public PoolingComponent rangeFXPrefab;

                public ASupportData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
                    isDefaultAttack = false;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ASupport(this, owner);
                }
            }
            private class PSupportBuff : PassiveSkill
            {
                private readonly float raiseAbility;

                public PSupportBuff(PSupportBuffData data, Unit skillOwner) : base(data, skillOwner)
                {
                    raiseAbility = data.raiseAbility;
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.HPCoef *= raiseAbility;
                    unit.MagicPowerCoef *= raiseAbility;
                    unit.PhysicsPowerCoef *= raiseAbility;
                    unit.ArmorAdd *= raiseAbility;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.HPCoef /= raiseAbility;
                    unit.MagicPowerCoef /= raiseAbility;
                    unit.PhysicsPowerCoef /= raiseAbility;
                    unit.ArmorAdd /= raiseAbility;
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            [System.Serializable]
            protected class PSupportBuffData : PassiveSkillData
            {
                #region Fields
                [BoxGroup("지원 버프", true, true)]
                [LabelText("아군 능력치 증가량"), Tooltip("아군 타워의 체력, 공격력, 주문력, 방어력 증가량")]
                public float raiseAbility;
                #endregion
                public PSupportBuffData()
                {
#if UNITY_EDITOR
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = true;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = true;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
                    PP_UseDuration = true;
#endif
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PSupportBuff(this, owner);
                }
            }
        }
    }
}