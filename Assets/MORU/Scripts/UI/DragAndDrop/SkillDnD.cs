using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YS;
using Moru;

namespace Moru.UI
{
    [RequireComponent(typeof(UI_Character_Skill))]
    public class SkillDnD : DragAndDrop
    {
        public UserSaveData.Character_DB.Skill_DB data => GetComponent<UI_Character_Skill>().MySkillDB;
        [SerializeField] GameObject BackGround;
        protected override void Start()
        {
            base.Start();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            //��ų�� ������ ���°� �ƴϸ� �巡�׾� ����� ������� �ʽ��ϴ�.
            if (UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Skill_DB != data) return;
            //����� ������ ������� �ʽ��ϴ�.
            if(BackGround != null) BackGround.SetActive(true);


            SetDragIMG();


            base.OnBeginDrag(eventData);
        }
        public override void DropIn(GameObject dropLoaction)
        {
            dropLoaction.transform.GetComponent<Image>().sprite = dragImg;
            var adress = dropLoaction.GetComponent<SlotAdress>();
            UserInfo.instance.Userinfo.GetCurrentCharacterDB.UpdateSkill(adress.myAdress, data);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Skill_DB != data) return;
            base.OnEndDrag(eventData);
            if (BackGround != null) BackGround.SetActive(false);
        }

        public override void SetDragIMG()
        {
            dragImg = data.Data.SkillData.icon;
        }
    }
}