using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public enum TITLE_UI_INDEX
    {
        TITLE,
        LOBBY,
        STAGE_SELECTED,
        MYPAGE_USERINFO,
        MYPAGE_CHARINFO,
        MYPAGE_TOWERINFO,
        MYPAGE_ACHIEVEINFO,
        SHOP,
        COLLECTION,
        OPTION,
        MYPAGE_CHARINFO_DETAILINFO,
        MYPAGE_CHARINFO_SKILL,
        MYPAGE_CHARINFO_MASTERY,
        MYPAGE_CHARINFO_SKIN,
        MYPAGE_CHARINFO_STORY,
        MYPAGE_TOWERINFO_DETAILINFO,
        MYPAGE_TOWERINFO_SKILL,
        MYPAGE_TOWERINFO_SOULCARD
    }

    public enum VisibleState
    {
        APPEARING = 100,
        APPEARED,
        DISAPPEARING,
        DISAPPEARED
    }

    public class StackUIManager : SingleToneMono<StackUIManager>
    {
        [ShowInInspector]
        private Stack<StackUIComponent> stackUI = new Stack<StackUIComponent>();
        public Stack<   
            List<Moru.UI_CustomAnimate> > stackAnim = new Stack<List<Moru.UI_CustomAnimate>>();

        [SerializeField]
        private StackUIComponent startUI;

        public event Delegate_NoRetVal_NoParam PushEvent;
        public event Delegate_NoRetVal_NoParam PopEvent;
        public TITLE_UI_INDEX StackTop => stackUI.Peek().Index;
        public TITLE_UI_INDEX StartIndex => startUI.Index;

        private void Start()
        {
            Push(startUI);
        }

        public static void Push(StackUIComponent comp)
        {
            var manager = Instance;

            comp.Show();
            manager.stackUI.Push(comp);
            manager.PushEvent?.Invoke();
        }
        public static void Pop()
        {
            var manager = Instance;

            manager.stackUI.Peek().Hide();
            manager.stackUI.Pop();
            manager.PopEvent?.Invoke();
        }
        public static void Pop(StackUIComponent comp)
        {
            var manager = Instance;

            while (manager.stackUI.Count != 0)
            {
                if (manager.stackUI.Peek().GetInstanceID() == comp.GetInstanceID())
                    break;
                Pop();
            }
        }
    }
}