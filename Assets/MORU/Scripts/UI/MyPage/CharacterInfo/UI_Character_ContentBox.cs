using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using YS;
using Moru;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{

    public class UI_Character_ContentBox : MonoBehaviour
    {
        public GameObject EquipButton;
        public SkeletonGraphic Skeleton_UGUI;

        #region Varis
        [SerializeField]        private PLAYABLE_UNIT_INDEX myIndex;
                                public PLAYABLE_UNIT_INDEX MyIndex => myIndex;

        [ShowInInspector]
        private Moru.UserSaveData.Character_DB data;
        public Moru.UserSaveData.Character_DB Data => data !=null ? data : null;
        [SerializeField] public bool isHave => Data != null ? data.IsHaving: false;
        [SerializeField] public bool NoHave => !isHave;     //안가지고 있으면 true, 가지고 있으면 false
        [SerializeField] public string NoHave_String => NoHave? "미보유" : " ";
        [SerializeField] public string Name => Data != null ? data.Data.name: "캐릭터 없음";

        [SerializeField] public SkeletonDataAsset skeleton => Data!= null ? data.skeleton : null;
        [SerializeField] public Color SkeletonColor => isHave ? haveColor : noHaveColor;
        [SerializeField] private Color haveColor;
        [SerializeField] private Color noHaveColor;

        #endregion

        private void Start()
        {
            TitleUI_Events.Del_CharacterClick += OnUpdateCharacterList;
        }

        public void Initialize(int myIndex, Moru.UserSaveData.Character_DB DB)
        {

            if (myIndex >= (int)YS.PLAYABLE_UNIT_INDEX.MAX || DB == null)
            {
                gameObject.SetActive(false);
                return;
            }

            //변수 초기화
            this.myIndex = (PLAYABLE_UNIT_INDEX )myIndex;
            data = DB;

            //스켈레톤 데이터 업데이트
            if(Skeleton_UGUI != null && skeleton != null)
            {
                Skeleton_UGUI.skeletonDataAsset = skeleton;
                var Skins = UserInfo.instance.Userinfo.characters_Info[this.myIndex].current_Skin.Skin_Name;
                if(Skeleton_UGUI.SkeletonData.FindSkin(Skins) != null)
                {
                    Skeleton_UGUI.Skeleton.SetSkin(Skins);
                    Skeleton_UGUI.Skeleton.SetSlotsToSetupPose();
                }
                Debug.LogWarning($"캐릭터 이름 : {(data !=null? data.Data.name : "null")}/{this.myIndex} \n" +
                    $"약속된 스켈레톤 데이터 : {(skeleton != null ? skeleton.name : "null")} \n " +
                    $"할당된 스켈레톤 데이터 : " +
                    $"{(Skeleton_UGUI != null ? Skeleton_UGUI.skeletonDataAsset.name: "null")}   \n" +
                    $"할당하려 한 스킨이름 {Skins} \n" +
                    $"현재 할당된 스킨 {(Skeleton_UGUI.Skeleton.Skin != null ? Skeleton_UGUI.Skeleton.Skin.Name: "null")}\n" +
                    $"시도 결과 : {Skeleton_UGUI.SkeletonData.FindSkin(Skins) != null} => {this.gameObject.name}\n" +
                    $"" );
            }

        }

        public void OnClick()
        {
            TitleUIManager.instance.userInfo.Userinfo.Current_PLAYABLE_UNIT_INDEX = myIndex;
            TitleUI_Events.Del_CharacterClick?.Invoke();
        }

        private void OnUpdateCharacterList()
        {
            if(TitleUIManager.instance.userInfo.Userinfo.GetCurrentCharacterDB == data)
            {
                EquipButton.SetActive(true);
            }
            else
            {
                EquipButton.SetActive(false);
            }
        }
    }


}