using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 2)]
    public class SkillSO : ScriptableObject
    {
        [SerializeField, BoxGroup("스킬 데이터", true, true), HideLabel]
        private Unit.Skill.SkillData data;

        public Unit.Skill.SkillData SkillData => data;
    }
}