using System.Collections;
using System.Collections.Generic;
using YS;
using Moru;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Moru.UI
{

    public class UI_Character_Skin : MonoBehaviour
    {
        [SerializeField] private int adress;
        private DataBindContext m_Context;
        public GameObject EquipButton;
        public GameObject Background;
        public SkeletonGraphic Skeleton_UGUI;
        #region Varis
        [SerializeField] private Moru.UserSaveData.Character_DB.Skin_DB skinData;
        public Moru.UserSaveData.Character_DB.Skin_DB SkinData => skinData != null ? skinData : null;

        public string skinName => SkinData != null ? SkinData.Skin_Name : "비어있음";

        public bool isHave => SkinData != null ? SkinData.IsHaving : false;

        [SerializeField] public Color SkeletonColor => isHave ? haveColor : noHaveColor;
        [SerializeField] private Color haveColor;
        [SerializeField] private Color noHaveColor;


        #endregion

        bool isFirst = true;
        #region Methods
        private void OnEnable()
        {
            if(isFirst) { isFirst = !isFirst; return; }
            if (Skeleton_UGUI.SkeletonDataAsset != UserInfo.instance.Userinfo.GetCurrentCharacterDB.skeleton) return;
            if (skinData == null) return;

            Initialized();

            //Skeleton_UGUI.initialSkinName = skinName;
            Skeleton_UGUI.Skeleton.SetSkin(SkinData.Skin_Name);
            Skeleton_UGUI.Skeleton.SetSlotsToSetupPose();
            Skeleton_UGUI.Skeleton.SetToSetupPose();
        }

        private void Start()
        {
            TitleUI_Events.Del_CharacterClick += Initialized;
            TitleUI_Events.Del_CharacterSKin_Click += OnUpdateSKinList;
        }


        public void OnClick()
        {
            if (!isHave) return;
            TitleUI_Events.Del_CharacterSKin_Click?.Invoke(SkinData);
        }


        public void Initialized()
        {

            var CurrentCharacterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
            var skins_DB = CurrentCharacterDB.Character_Skin_DB;
            //스킨 딕셔너리는 1부터 시작
            if (adress < skins_DB.Count + 1)
            {
                if (skins_DB.TryGetValue(adress, out UserSaveData.Character_DB.Skin_DB value))
                {
                    skinData = value;
                    Skeleton_UGUI.skeletonDataAsset = CurrentCharacterDB.skeleton;



                    //미보유상태일때를 위한 업데이트
                    bool isActive = isHave ? false : true;
                    Background?.SetActive(isActive);
                }


            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnUpdateSKinList(UserSaveData.Character_DB.Skin_DB _DB)
        {
            if (_DB == skinData)
            {
                EquipButton.SetActive(true);
                var CurrentCharacterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
                CurrentCharacterDB.current_Skin = _DB;
                TitleUI_Events.Del_CharacterClick?.Invoke();
            }
            else
            {
                EquipButton.SetActive(false);
            }
        }
        #endregion

    }
}