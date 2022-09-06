using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace Moru.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class Auto_EventTrigger_Delegate : MonoBehaviour
    {
        private EventTrigger trigger;
        private bool isGroup;
        public bool IsGroup { get => isGroup; set => isGroup = value; }

        [SerializeField, ReadOnly] bool isActived;
        [BoxGroup("이벤트 등록"), LabelText("클릭 시 이벤트 등록")] public bool IsClick_Trigger;
        [BoxGroup("이벤트 등록"), LabelText("마우스 진입 시 이벤트 등록")] public bool IsOnMouseEnter_Trigger;
        [BoxGroup("이벤트 등록"), LabelText("마우스 나갈 시 이벤트 등록")] public bool IsOnMOuseExit_Trigger;
        private void Start()
        {
            trigger = GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerEnter;
            entry2.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry2);
        }

        public virtual void OnPointerDownDelegate(PointerEventData data)
        {
            Debug.Log("OnPointerDownDelegate called.");
        }

        public virtual void OnPointerEnterDelegate(PointerEventData data)
        {
            Debug.Log("OnPointerDownDelegate called.");
        }
        public virtual void OnPointerExitDelegate(PointerEventData data)
        {
            Debug.Log("OnPointerDownDelegate called.");
        }
    }
}