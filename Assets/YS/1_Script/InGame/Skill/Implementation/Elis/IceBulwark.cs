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
                // ��ų�� �ʿ��� �̺�Ʈ �������̵�           //
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
                [BoxGroup("�����溮", true, true)]
                [LabelText("��ȣ�� ������")]
                public BaseShield iceBulwarkPrefab;
                [BoxGroup("�����溮"), Min(0.0f)]
                [LabelText("��ȣ�� ������")]
                public float radius;
                [BoxGroup("�����溮"), Min(0.0f)]
                [LabelText("��ȣ�� ���ӽð�")]
                public float duration;
                [BoxGroup("�����溮"), Min(0.0f)]
                [LabelText("��ȣ��")]
                public int maxShieldAmount;
                [BoxGroup("�����溮"), Min(0.0f)]
                [LabelText("��ȣ�� ���")]
                public float maxShieldAmountCoef;

                public AIceBulwarkData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
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