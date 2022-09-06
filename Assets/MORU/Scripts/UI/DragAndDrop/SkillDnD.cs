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
            //스킬을 선택한 상태가 아니면 드래그앤 드롭이 실행되지 않습니다.
            if (UserInfo.instance.Userinfo.GetCurrentCharacterDB.Current_Selected_Skill_DB != data) return;
            //배경이 없으면 실행되지 않습니다.
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