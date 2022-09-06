using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using YS;

namespace Moru.UI
{
    [RequireComponent(typeof(UI_Tower_ContentBox))]
    public class TowerDnD : DragAndDrop
    {
        public UserSaveData.Tower_DB data => GetComponent<UI_Tower_ContentBox>().Data;

        [SerializeField] GameObject BackGround;


        protected override void Start()
        {
            base.Start();
            //gr = FindObjectOfType<UI_Tower_DetailInfo>(true).gr;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            //스킬을 선택한 상태가 아니면 드래그앤 드롭이 실행되지 않습니다.
            if (UserInfo.instance.Userinfo.GetCurrentTowerDB != data) return;
            if (BackGround != null) BackGround.SetActive(true);
            SetDragIMG();
            base.OnBeginDrag(eventData);
        }

        public override void DropIn(GameObject dropLoaction)
        {
            dropLoaction.transform.GetComponent<Image>().sprite = dragImg;
            var adress = dropLoaction.GetComponent<SlotAdress>();
            TitleUIManager.instance.userInfo.Userinfo.UpdateTower(adress.myAdress, data);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            BackGround?.SetActive(false);
        }
        public override void SetDragIMG()
        {
            dragImg = data.Data.icon;
        }
    }
}