using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class CharacterStackPnP : StackUIPopAndPush
    {
        [SerializeField]
        private PLAYABLE_UNIT_INDEX charIndex;

        protected override void OnClick()
        {
            base.OnClick();

        }
    }
}