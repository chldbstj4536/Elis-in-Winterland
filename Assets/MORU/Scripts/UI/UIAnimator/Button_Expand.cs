using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Moru.UI
{
    [AddComponentMenu("UI_Expent/Button_Expand", 30)]
    public class Button_Expand : Button
    {
        public ButtonGroup group;
        public GameObject selectedGraphic;
        protected override void Awake()
        {
            base.Awake();
            if (transform.Find("{{Seleted}}") != null)
            {
                selectedGraphic = transform.Find("{{Seleted}}").gameObject;
            }
        }



        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            UnityEngine.Debug.Log("Ŭ���̺�Ʈ");
        }
        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            UnityEngine.Debug.Log("���� �̺�Ʈ");
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            UnityEngine.Debug.Log("���� �̺�Ʈ");

        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            UnityEngine.Debug.Log("������ �̺�Ʈ");
            if (selectedGraphic != null)
            {
                selectedGraphic.SetActive(true);
                if (group != null)
                {
                    group.SetGrahpic();
                }
            }
        }

        public void SetGraphic_ToSelectGraphic()
        {
            if (currentSelectionState != SelectionState.Selected)
            {
                selectedGraphic.SetActive(false);
            }
        }
    }

}