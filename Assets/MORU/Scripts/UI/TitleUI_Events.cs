using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace Moru
{
    public class TitleUI_Events
    {
        public delegate void ClickEvents();

        public static ClickEvents Del_BindAll;

        public static ClickEvents Del_UserInfo_Update;
        public static ClickEvents Del_CharacterClick;
        public static ClickEvents Del_CharacterSKill_Click;
        public static ClickEvents Del_CharacterMastery_Click;
        public static ClickEvents Del_Tower_Click;


        public delegate void CharacterSkin_Click(UserSaveData.Character_DB.Skin_DB skinDB);
        public static CharacterSkin_Click Del_CharacterSKin_Click;


        

    }

}