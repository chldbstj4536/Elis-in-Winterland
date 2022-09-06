using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YS;
using Sirenix.OdinInspector;


namespace Moru
{

    public class UI_Character_Skill : MonoBehaviour
    {
        [SerializeField] int index;
        UserSaveData.Character_DB.Skill_DB mySkillDB => UserInfo.instance.Userinfo.GetCurrentCharacterDB.Character_Skills[index];
        public UserSaveData.Character_DB.Skill_DB MySkillDB => mySkillDB;

        private void Start()
        {
            TitleUI_Events.Del_CharacterClick += Initialized;
            Initialized();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void Initialized()
        {
            if (mySkillDB == null)
            {
                //��ų�� �������ó��(9���� ���� ����)
                gameObject.SetActive(false);
                return;
            }
            else
            {gameObject.SetActive(true); }
            if(index == 0)
            {
                //���� ���ӽ��۽� �ش�ĳ������ ��ų������ ���õ� ��ų�� �ش罺ų�� ����
                UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Skill_DB = mySkillDB;
                TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
            }

        }

        public void OnClick()
        {
            UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Skill_DB = mySkillDB;
            TitleUI_Events.Del_CharacterSKill_Click?.Invoke();
        }
    }
}