using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        /// <summary>
        /// 유닛에게 스킬 인터페이스를 제공
        /// </summary>
        [System.Serializable]
        public partial class Skill
        {
            #region Field
            protected readonly string skillName;
            protected readonly string skillDesc;
            private readonly Sprite icon;
            protected readonly int skillCurLv;

            protected ActiveSkill activeSkill;
            protected List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

            protected Unit caster;
            protected MoveComponent mc;
            #endregion

            #region Properties
            public Sprite Icon => icon;
            public ActiveSkill ASkill => activeSkill;
            public List<PassiveSkill> PSkills => passiveSkills;
            public bool IsCancelable => caster.fsm.CurrentStateIndex == FSM.STATE_INDEX.SELECTING_TARGET || activeSkill.CanCancel;
            #endregion

            #region Methods
            protected Skill(Unit caster, SkillSO sso, int skillLv)
            {
                this.caster = caster;
                mc = caster.GetComponent<MoveComponent>();

                skillName = sso.SkillData.skillName;
                skillDesc = sso.SkillData.skillDesc;
                icon = sso.SkillData.icon;
                skillCurLv = skillLv;

                activeSkill = sso.SkillData.activeSkillData[skillCurLv].Instantiate(caster) as ActiveSkill;
                foreach (var passiveSkillData in sso.SkillData.passiveSkillDatas)
                    passiveSkills.Add(passiveSkillData[skillCurLv].Instantiate(caster) as PassiveSkill);
            }
            #endregion

            [System.Serializable]
            public class SkillData
            {
                [BoxGroup("Skill Data", true, true), LabelText("스킬 이름")]
                public string skillName;
                [BoxGroup("Skill Data"), LabelText("스킬 설명"), TextArea]
                public string skillDesc;
                [BoxGroup("Skill Data"), LabelText("스킬 아이콘")]
                public Sprite icon;
                [BoxGroup("Skill Data"), LabelText("스킬 최대 레벨"), Min(1), OnValueChanged(nameof(MaxLevelChanged))]
                public int maxLv = 1;

                [BoxGroup("Skill Data/액티브 스킬 데이터", true, true), SerializeReference]
                [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement)), DisableContextMenu]
                [LabelText("액티브 스킬"), Tooltip("해당 스킬에 적용할 액티브 스킬을 설정합니다.")]
                public List<ActiveSkillData> activeSkillData = new List<ActiveSkillData>();
                [BoxGroup("Skill Data/패시브 스킬 데이터들", true, true), SerializeField]
                [DisableContextMenu, ListDrawerSettings(CustomAddFunction = nameof(AddPassiveSkill))]
                [LabelText("패시브 스킬들"), Tooltip("해당 스킬에 적용할 패시브 스킬들을 설정합니다.")]
                public List<PassiveSkillsWrap> passiveSkillDatas = new List<PassiveSkillsWrap>();

                private SkillData() { MaxLevelChanged(); }
                public Skill Instantiate(Unit caster, SkillSO sso, int skillLv)
                {
                    return new Skill(caster, sso, skillLv);
                }
                private void MaxLevelChanged()
                {
                    if (activeSkillData.Count > maxLv)
                    {
                        int removeRange = activeSkillData.Count - maxLv;
                        activeSkillData.RemoveRange(maxLv, removeRange);
                        foreach (var passiveSkillData in passiveSkillDatas)
                            passiveSkillData.passiveSkillDatas.RemoveRange(maxLv, removeRange);
                    }
                    else if (activeSkillData.Count < maxLv)
                    {
                        for (int i = activeSkillData.Count; i < maxLv; ++i)
                        {
                            activeSkillData.Add(null);
                            foreach (var passiveSkillData in passiveSkillDatas)
                                passiveSkillData.passiveSkillDatas.Add(null);
                        }
                    }
                }
                private void AddPassiveSkill()
                {
                    PassiveSkillsWrap psWrap = new PassiveSkillsWrap();
                    psWrap.passiveSkillDatas = new List<PassiveSkillData>();
                    for (int i = 0; i < maxLv; ++i)
                        psWrap.passiveSkillDatas.Add(null);
                    passiveSkillDatas.Add(psWrap);
                }
                private void BeginDrawListElement(int index)
                {
#if UNITY_EDITOR
                    Sirenix.Utilities.Editor.SirenixEditorGUI.Title($"Lv{index + 1}", "", TextAlignment.Center, true);
#endif
                }
                [System.Serializable]
                public struct PassiveSkillsWrap
                {
                    [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement)), DisableContextMenu, SerializeReference]
                    [LabelText("패시브 스킬"), Tooltip("해당 스킬에 적용할 패시브 스킬들을 설정합니다.")]
                    public List<PassiveSkillData> passiveSkillDatas;

                    public PassiveSkillData this[int index]
                    {
                        get { return passiveSkillDatas[index]; }
                    }
                    private void BeginDrawListElement(int index)
                    {
#if UNITY_EDITOR
                        Sirenix.Utilities.Editor.SirenixEditorGUI.Title($"Lv{index + 1}", "", TextAlignment.Center, true);
#endif
                    }
                }
            }
        }
    }
}