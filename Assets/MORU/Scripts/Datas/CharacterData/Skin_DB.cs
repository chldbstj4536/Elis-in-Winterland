using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace Moru
{
    public partial class UserSaveData
    {
        public partial class Character_DB
        {
            [SerializeField, ShowInInspector] private Dictionary<int, Skin_DB> character_Skin_DB = new Dictionary<int, Skin_DB>();
            public Dictionary<int, Skin_DB> Character_Skin_DB => character_Skin_DB;

            [SerializeField, ShowInInspector] public Skin_DB current_Skin;

            [System.Serializable]
            public class Skin_DB
            {
                [SerializeField, LabelText("보유 여부"), ReadOnly] private bool isHaving;
                public bool IsHaving => isHaving;
                [SerializeField, LabelText("스킨 인덱스"), ReadOnly] private int index;
                public int Index => index;
                [SerializeField, LabelText("스킨 대상"), ReadOnly] private YS.PLAYABLE_UNIT_INDEX character_Index;
                public YS.PLAYABLE_UNIT_INDEX Character_Index => character_Index;

                [SerializeField, LabelText("스킨 이름")] private string skin_Name;
                public string Skin_Name => skin_Name;

                [SerializeField, LabelText("스킨 데이터")] private Spine.Skin skinData;
                public Spine.Skin SkinData => skinData;

                public Skin_DB(bool isHaving, int index, YS.PLAYABLE_UNIT_INDEX char_index, string skinName, Spine.Skin skinData)
                {
                    this.isHaving = isHaving;
                    this.index = index;
                    this.character_Index = char_index;
                    this.skin_Name = skinName;
                    this.skinData = skinData;
                }
            }

            /// <summary>
            /// 해당 캐릭터의 스킨데이터를 초기화합니다. 또한 첫번째 인덱스로 스킨을 설정합니다.
            /// </summary>
            public void Init_Skin()
            {
                if (UserSaveData.skeletonSO == null) Debug.Log("null ref");
                var SkeletonData = UserSaveData.skeletonSO.character_Skeleton[(int)Index];
                var skins = SkeletonData.GetAnimationStateData().SkeletonData.Skins;
                int i = 0;
                foreach (var skin in skins)
                {
                    //default 스킨을 제외하기 위해 첫0은 넘어갑니다.
                    if (i == 0) { i++; continue; }

                    var skindata = new Skin_DB(true, i, Index, skin.Name, skin);
                    character_Skin_DB.Add(i, skindata);

                    //첫번째 인덱스의 스킨데이터를 현재스킨값으로 설정합니다.
                    if (i == 1) { current_Skin = skindata; }     
                    i++;
                }
            }
        }
    }
}