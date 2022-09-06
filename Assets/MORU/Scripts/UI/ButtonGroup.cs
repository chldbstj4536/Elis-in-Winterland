using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


namespace Moru.UI
{
    [DisallowMultipleComponent]
    public class ButtonGroup : UIBehaviour
    {
        public bool AutoRegister;
        public List<Button_Expand> ButtonList;

        protected override void Awake()
        {
            if(AutoRegister)
            {
                for(int i =0; i < this.transform.childCount; i++)
                {
                    var child = this.transform.GetChild(i).gameObject;
                    if(child.TryGetComponent<Button_Expand>(out Button_Expand comp))
                    {
                        ButtonList.Add(comp);
                    }
                }
            }
            foreach(var btn in ButtonList)
            {
                btn.group = this;
            }
        }

        public void SetGrahpic()
        {
            foreach(var btn in ButtonList)
            {
                btn.SetGraphic_ToSelectGraphic();
            }
        }
    }
}