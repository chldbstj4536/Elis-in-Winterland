using UnityEngine;

namespace YS
{
    public class InputManager : MonoBehaviour
    {
        static private KeyCode[] mainKeys = new KeyCode[(int)E_CONTROL_TYPE.MAX];
        static private KeyCode[] subKeys = new KeyCode[(int)E_CONTROL_TYPE.MAX];

        static private Delegate_NoRetVal_NoParam[] KeyHoldEvent = new Delegate_NoRetVal_NoParam[(int)E_CONTROL_TYPE.MAX];
        static private Delegate_NoRetVal_NoParam[] KeyDownEvent = new Delegate_NoRetVal_NoParam[(int)E_CONTROL_TYPE.MAX];
        static private Delegate_NoRetVal_NoParam[] KeyUpEvent = new Delegate_NoRetVal_NoParam[(int)E_CONTROL_TYPE.MAX];

        private void Awake()
        {
            mainKeys[(int)E_CONTROL_TYPE.CLOSE_UI]                   = KeyCode.Escape;
            mainKeys[(int)E_CONTROL_TYPE.PAUSE]                      = KeyCode.Escape;
            mainKeys[(int)E_CONTROL_TYPE.GUIDE]                      = KeyCode.F1;
            mainKeys[(int)E_CONTROL_TYPE.CANCLE]                     = KeyCode.Escape;
            mainKeys[(int)E_CONTROL_TYPE.MOVE]                       = KeyCode.None;
            mainKeys[(int)E_CONTROL_TYPE.CANCLE_N_MOVE]              = KeyCode.Mouse1;
            mainKeys[(int)E_CONTROL_TYPE.DASH]                       = KeyCode.Space;
            mainKeys[(int)E_CONTROL_TYPE.INTERACT]                   = KeyCode.Mouse0;
            mainKeys[(int)E_CONTROL_TYPE.DEFAULT_ATTACK]             = KeyCode.F;
            mainKeys[(int)E_CONTROL_TYPE.TS_1]                       = KeyCode.Q;
            mainKeys[(int)E_CONTROL_TYPE.TS_2]                       = KeyCode.W;
            mainKeys[(int)E_CONTROL_TYPE.TS_3]                       = KeyCode.E;
            mainKeys[(int)E_CONTROL_TYPE.TS_4]                       = KeyCode.A;
            mainKeys[(int)E_CONTROL_TYPE.TS_5]                       = KeyCode.S;
            mainKeys[(int)E_CONTROL_TYPE.TS_6]                       = KeyCode.D;
            mainKeys[(int)E_CONTROL_TYPE.U_SKILL]                    = KeyCode.R;
            mainKeys[(int)E_CONTROL_TYPE.USE_ITEM]                   = KeyCode.T;
            mainKeys[(int)E_CONTROL_TYPE.QUICK_SHIFT]                = KeyCode.LeftShift;
            
            subKeys[(int)E_CONTROL_TYPE.CLOSE_UI]                    = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.PAUSE]                       = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.GUIDE]                       = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.CANCLE]                      = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.MOVE]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.CANCLE_N_MOVE]               = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.DASH]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.INTERACT]                    = KeyCode.Return;
            subKeys[(int)E_CONTROL_TYPE.DEFAULT_ATTACK]              = KeyCode.C;
            subKeys[(int)E_CONTROL_TYPE.TS_1]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.TS_2]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.TS_3]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.TS_4]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.TS_5]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.TS_6]                        = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.U_SKILL]                     = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.USE_ITEM]                    = KeyCode.None;
            subKeys[(int)E_CONTROL_TYPE.QUICK_SHIFT]                 = KeyCode.None;
        }
        private void Update()
        {
            for (int i = 0; i < (int)E_CONTROL_TYPE.MAX; i++)
            {
                if ((Input.GetKey(mainKeys[i]) || Input.GetKey(subKeys[i])) && KeyHoldEvent[i] != null)
                    KeyHoldEvent[i]();
                if ((Input.GetKeyDown(mainKeys[i]) || Input.GetKeyDown(subKeys[i])) && KeyDownEvent[i] != null)
                    KeyDownEvent[i]();
                if ((Input.GetKeyUp(mainKeys[i]) || Input.GetKeyUp(subKeys[i])) && KeyUpEvent[i] != null)
                    KeyUpEvent[i]();
            }
        }

        static public void AddKeyEvent(E_CONTROL_TYPE cType, INPUT_STATE state, Delegate_NoRetVal_NoParam e)
        {
            switch (state)
            {
                case INPUT_STATE.HOLD:
                    KeyHoldEvent[(int)cType] = e;
                    break;
                case INPUT_STATE.DOWN:
                    KeyDownEvent[(int)cType] = e;
                    break;
                case INPUT_STATE.UP:
                    KeyUpEvent[(int)cType] = e;
                    break;
            }
        }
        static public void ChangeMainKeySetting(E_CONTROL_TYPE cType, KeyCode key)
        {
            mainKeys[(int)cType] = key;
        }
        static public void ChangeSubKeySetting(E_CONTROL_TYPE cType, KeyCode key)
        {
            subKeys[(int)cType] = key;
        }
        static public bool IsInvokedEvent(E_CONTROL_TYPE cType, INPUT_STATE state)
        {
            bool bInvoked = false;
            
            switch (state)
            {
                case INPUT_STATE.HOLD:
                    bInvoked = Input.GetKey(mainKeys[(int)cType]) || Input.GetKey(subKeys[(int)cType]);
                    break;
                case INPUT_STATE.UP:
                    bInvoked = Input.GetKeyUp(mainKeys[(int)cType]) || Input.GetKeyUp(subKeys[(int)cType]);
                    break;
                case INPUT_STATE.DOWN:
                    bInvoked = Input.GetKeyDown(mainKeys[(int)cType]) || Input.GetKeyDown(subKeys[(int)cType]);
                    break;
            }

            return bInvoked;
        }
        static public void ClearEvent()
        {
            for (int i = 0; i < (int)E_CONTROL_TYPE.MAX; i++)
            {
                KeyHoldEvent[i] = null;
                KeyDownEvent[i] = null;
                KeyUpEvent[i] = null;
            }
        }
    }
}