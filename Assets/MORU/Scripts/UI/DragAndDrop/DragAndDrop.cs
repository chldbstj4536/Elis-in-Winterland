using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using YS;

namespace Moru.UI
{
    public abstract class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        protected Sprite dragImg;
        [SerializeField]
        protected GraphicRaycaster gr;

        private Image selectedItem;
        public GameObject selectedUIObj;

        protected virtual void Start()
        {
            
            selectedItem = selectedUIObj.transform.GetChild(0).GetComponent<Image>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            selectedUIObj.SetActive(true);
            selectedItem.sprite = dragImg;
        }
        public virtual void OnDrag(PointerEventData eventData)
        {
            selectedUIObj.transform.position = eventData.position;
        }
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> hits = new List<RaycastResult>();
            selectedUIObj.SetActive(false);
            gr.Raycast(eventData, hits);
            if (hits.Count != 0)    DropIn(hits[0].gameObject);
        }
        public abstract void DropIn(GameObject dropLoaction);
        public abstract void SetDragIMG();
    }
}