using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
namespace YS
{
    public class StackUIPopAndPush : MonoBehaviour
    {
        [SerializeField]
        private bool bPopOneStep;
        [SerializeField, HideIf("bPopOneStep")]
        public StackUIComponent popUI;
        [SerializeField]
        public StackUIComponent pushUI;

        private Button btn;
        public Button Btn => btn;
        private Toggle toggle;
        private StackUIManager stack;

        private void Start()
        {
            btn = GetComponent<Button>();
            stack = StackUIManager.Instance;
            toggle = GetComponent<Toggle>();
            if (btn != null)
            { btn.onClick.AddListener(OnClick); }
            else if(toggle!= null)
            { toggle.onValueChanged.AddListener(OnToggleClick); }
            else
            { }
        }


        protected virtual void OnClick()
        {
            if (stack.StackTop == pushUI?.Index)
                return;

            if (bPopOneStep)            StackUIManager.Pop();
            else if (popUI != null)     StackUIManager.Pop(popUI);
            if (pushUI != null)         StackUIManager.Push(pushUI);
        }

        protected virtual void OnToggleClick(bool isOn)
        {
            if(isOn)
            { OnClick(); }
        }
    }
}