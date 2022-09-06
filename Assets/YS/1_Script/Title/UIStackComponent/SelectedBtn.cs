using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    [RequireComponent(typeof(Button))]
    public class SelectedBtn : MonoBehaviour
    {
        [SerializeField]
        private TITLE_UI_INDEX[] selectedIndex;
        [SerializeField]
        private ColorBlock selectedColorBlock;

        private StackUIManager manager;
        private Button btn;

        private void Awake()
        {
            manager = StackUIManager.Instance;
            manager.PushEvent += OnStackChanged;
            manager.PopEvent += OnStackChanged;

            btn = GetComponent<Button>();
        }
        private void OnStackChanged()
        {
            for (int i = 0; i < selectedIndex.Length; ++i)
            {
                if (manager.StackTop == selectedIndex[i])
                {
                    btn.colors = selectedColorBlock;
                    return;
                }
            }

            btn.colors = ColorBlock.defaultColorBlock;
        }
    }
}