using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moru;

namespace Moru
{
    public class UI_Mastery_Content : MonoBehaviour
    {
        [SerializeField] private Moru.Define.MasteryType masteryType;

        // Start is called before the first frame update
        void Start()
        {
            TitleUI_Events.Del_CharacterClick += Initialized;
        }

        bool isFirst = true;
        private void OnEnable()
        {
            if (isFirst) { isFirst = false; return; }
            Initialized();
        }

        [ContextMenu("�ʱ�ȭ")]
        public void Initialized()
        {
            if (!this.gameObject.activeInHierarchy) return;

            var MasteryInfo = Moru.UserSaveData.MasterySO[UserInfo.instance.Userinfo.Current_PLAYABLE_UNIT_INDEX];

            List<MasterySO.Horizontal_MasteryData> targetMasteryDatas;
            if (masteryType == Define.MasteryType.Character) { targetMasteryDatas = MasteryInfo.Vertical_Masterys_TypeAttack; }
            else { targetMasteryDatas = MasteryInfo.Vertical_Masterys_TypeUtility; }

            Debug.Log($"�����͸� ������ ����� \n ã���� �ϴ� ����ĳ���� �ε��� :{UserInfo.instance.Userinfo.Current_PLAYABLE_UNIT_INDEX}  �����͸�Ÿ�� : {masteryType}" +
                $"\n �����͸� ������ ���� : {Moru.UserSaveData.MasterySO}" +
                $"\n ������ ��� : {Moru.UserSaveData.MasterySO[UserInfo.instance.Userinfo.Current_PLAYABLE_UNIT_INDEX]}" +
                $"\n �����͸� ī��Ʈ : {targetMasteryDatas.Count}");
            int adress = 0;


            List<MasteryContent> contents = new List<MasteryContent>();
            for(int i = 0; i < transform.childCount; i++)
            {
                contents.Add(transform.GetChild(i).GetComponent<MasteryContent>());
            }


            for (int i = 0; i < (int)Define.MasteryLevel.MAX; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var Mastery_level = targetMasteryDatas[i];
                    var Mastery = Mastery_level.Masterys[j];
                    var Content = contents[i * 4 + j];
                    Content.Initialize(Mastery, adress);
                    adress++;
                    if (i == 0 && j == 0)
                    {
                        UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Mastery = Mastery;
                    }
                }
            }
            foreach(var comp in contents)
            { comp.SetView(); }

            TitleUI_Events.Del_CharacterMastery_Click?.Invoke();
        }
    }
}