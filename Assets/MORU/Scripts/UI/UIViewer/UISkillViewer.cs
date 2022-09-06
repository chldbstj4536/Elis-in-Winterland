using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru.UI
{
    public partial class UIViewer : MonoBehaviour
    {
        #region #��ų ������ ���ε�
        public void InitSkillData_Bind()
        {
            StartCoroutine(Skill_Bind());

        }
        IEnumerator Skill_Bind()
        {
            yield return null;
            m_Context["CurrentSkillPreset"] = userData.GetCurrentCharacterDB.Current_preset_Adress+1;       //������ �ѹ��� ������Ʈ
            m_Context["CurrentSkillPoint"] = userData.GetCurrentCharacterDB.current_Skill_Point[userData.GetCurrentCharacterDB.Current_preset_Adress];       //������ �� ��ų����Ʈ 
            //ĳ���Ͱ� �����ϰ� �ִ� ��ų���� ������Ʈ
            for (int i = 0; i < 9; i++)
            {

                var SkillDB = userData.GetCurrentCharacterDB.Character_Skills[i];
                var currentLevel = SkillDB!=null? SkillDB.Preset_Level[userData.GetCurrentCharacterDB.Current_preset_Adress] : 0;
                if (SkillDB == null) continue;
                m_Context[$"CharacterSkill_{i}/Icon"] = SkillDB.Data.SkillData.icon;
                m_Context[$"CharacterSkill_{i}/Name"] = SkillDB.Data.SkillData.skillName;
                m_Context[$"CharacterSkill_{i}/Level"] = SkillDB.Preset_Level[userData.GetCurrentCharacterDB.Current_preset_Adress];
                m_Context[$"CharacterSkill_{i}/Desc"] = SkillDB.Data.SkillData.skillDesc;
                // ���� �ڵ�
                //m_Context[$"CharacterSkill_{i}/ManaCost"] = SkillDB.Data.SkillData.activeSkillData.manaCost;
                //m_Context[$"CharacterSkill_{i}/Cool"] = SkillDB.Data.SkillData.activeSkillData.coolTime;
                // ���� �ڵ�
                m_Context[$"CharacterSkill_{i}/ManaCost"] = SkillDB.Data.SkillData.activeSkillData[currentLevel].manaCost;
                m_Context[$"CharacterSkill_{i}/Cool"] = SkillDB.Data.SkillData.activeSkillData[currentLevel].coolTime;
            }

            //���� ������ ��ų�� ������ ������Ʈ
            if (UserData.GetCurrentCharacterDB.Current_Selected_Skill_DB != null)
            {
                var currentLevel = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Preset_Level[userData.GetCurrentCharacterDB.Current_preset_Adress];
                m_Context["CurrentSkill/Icon"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.icon;
                m_Context["CurrentSkill/Name"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.skillName;
                m_Context["CurrentSkill/Level"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Preset_Level
                                                    [userData.GetCurrentCharacterDB.Current_preset_Adress];
                m_Context["CurrentSkill/Desc"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.skillDesc;
                // ���� �ڵ�
                //m_Context["CurrentSkill/ManaCost"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.activeSkillData.manaCost;
                //m_Context["CurrentSkill/Cool"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.activeSkillData.coolTime;
                // ���� �ڵ�
                m_Context["CurrentSkill/ManaCost"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.activeSkillData[currentLevel].manaCost;
                m_Context["CurrentSkill/Cool"] = userData.GetCurrentCharacterDB.Current_Selected_Skill_DB.Data.SkillData.activeSkillData[currentLevel].coolTime;
            }

            //ĳ���Ͱ� �������� ��ų���� ������Ʈ
            for (int i = 0; i < 6; i++)
            {
                var Preset_SkillSlots = UserData.GetCurrentCharacterDB.Skill_Preset[UserData.GetCurrentCharacterDB.Current_preset_Adress];
                if (Preset_SkillSlots[(Define.Slot)i] != null)
                {
                    m_Context[$"CharacterSlot_{i}/Icon"] = Preset_SkillSlots[(Define.Slot)i].Data.SkillData.icon;
                    m_Context[$"CharacterSlot_{i}/Name"] = Preset_SkillSlots[(Define.Slot)i].Data.SkillData.skillName;
                    m_Context[$"CharacterSlot_{i}/Level"] = Preset_SkillSlots[(Define.Slot)i].Preset_Level
                                                            [UserData.GetCurrentCharacterDB.Current_preset_Adress];
                }
                else
                {
                    m_Context[$"CharacterSlot_{i}/Icon"] = UserSaveData.BindingSO.Icon_Null;
                    m_Context[$"CharacterSlot_{i}/Name"] = "�������";
                    m_Context[$"CharacterSlot_{i}/Level"] = 0;
                }
            }
        }
        #endregion
    }
}