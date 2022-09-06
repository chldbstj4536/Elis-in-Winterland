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
        [SerializeField] public bool NoHave => !isHave;     //�Ȱ����� ������ true, ������ ������ false
        [SerializeField] public string NoHave_String => NoHave? "�̺���" : " ";
        [SerializeField] public string Name => Data != null ? data.Data.name: "ĳ���� ����";

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

            //���� �ʱ�ȭ
            this.myIndex = (PLAYABLE_UNIT_INDEX )myIndex;
            data = DB;

            //���̷��� ������ ������Ʈ
            if(Skeleton_UGUI != null && skeleton != null)
            {
                Skeleton_UGUI.skeletonDataAsset = skeleton;
                var Skins = UserInfo.instance.Userinfo.characters_Info[this.myIndex].current_Skin.Skin_Name;
                if(Skeleton_UGUI.SkeletonData.FindSkin(Skins) != null)
                {
                    Skeleton_UGUI.Skeleton.SetSkin(Skins);
                    Skeleton_UGUI.Skeleton.SetSlotsToSetupPose();
                }
                Debug.LogWarning($"ĳ���� �̸� : {(data !=null? data.Data.name : "null")}/{this.myIndex} \n" +
                    $"��ӵ� ���̷��� ������ : {(skeleton != null ? skeleton.name : "null")} \n " +
                    $"�Ҵ�� ���̷��� ������ : " +
                    $"{(Skeleton_UGUI != null ? Skeleton_UGUI.skeletonDataAsset.name: "null")}   \n" +
                    $"�Ҵ��Ϸ� �� ��Ų�̸� {Skins} \n" +
                    $"���� �Ҵ�� ��Ų {(Skeleton_UGUI.Skeleton.Skin != null ? Skeleton_UGUI.Skeleton.Skin.Name: "null")}\n" +
                    $"�õ� ��� : {Skeleton_UGUI.SkeletonData.FindSkin(Skins) != null} => {this.gameObject.name}\n" +
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