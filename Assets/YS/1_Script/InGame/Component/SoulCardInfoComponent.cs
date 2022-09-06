using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public class SoulCardInfoComponent : PoolingComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public delegate void OnSelectSoulcard(SOULCARD_INDEX si);

        public Image img_Frame;
        public TMP_Text tmp_Name;
        public TMP_Text tmp_Desc;
        public Image img_Icon;

        private SOULCARD_INDEX soulCardIndex;

        public event OnSelectSoulcard OnSelectSoulcardEvent;

        public SOULCARD_INDEX SoulCardIndex => soulCardIndex;

        private void OnDisable()
        {
            OnSelectSoulcardEvent = null;
        }
        public void SetSoulCardUI(SOULCARD_INDEX soulCardIndex, string name, string desc, Sprite icon)
        {
            this.soulCardIndex = soulCardIndex;
            tmp_Name.text = name;
            tmp_Desc.text = desc;
            img_Icon.sprite = icon;
        }
        public void OnPointerClick(PointerEventData e)
        {
            OnSelectSoulcardEvent?.Invoke(soulCardIndex);
            img_Frame.color = Color.white;
        }
        public void OnPointerEnter(PointerEventData e)
        {
            img_Frame.color = Color.yellow;
        }
        public void OnPointerExit(PointerEventData e)
        {
            img_Frame.color = Color.white;
        }
    }
}