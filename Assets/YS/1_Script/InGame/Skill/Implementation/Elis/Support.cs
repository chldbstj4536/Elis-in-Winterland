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
            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
            /// (�ʱ�ȭ�� ASKILL_NAME_DATA���� ó��)
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
                [BoxGroup("����", true, true)]
                [HideLabel]
                public PSupportBuffData pBuffData;
                [BoxGroup("����", true, true)]
                [LabelText("���ӽð�"), Min(0.0f)]
                public float buffDuration;
                [BoxGroup("����")]
                [LabelText("���� ����Ʈ"), Tooltip("ȿ�� ������ �����ִ� ȿ�� ����Ʈ")]
                public PoolingComponent rangeFXPrefab;

                public ASupportData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
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
                [BoxGroup("���� ����", true, true)]
                [LabelText("�Ʊ� �ɷ�ġ ������"), Tooltip("�Ʊ� Ÿ���� ü��, ���ݷ�, �ֹ���, ���� ������")]
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