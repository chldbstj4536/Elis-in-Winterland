using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Moru;


public class UI_illustUpdater : MonoBehaviour
{
    public enum IllustType { Character, Tower}
    public IllustType illustType;

    private Define.UnitViewMode viewMode;

    [SerializeField] Button IllustChangeButton;
    [SerializeField] SkeletonGraphic skeletonUGUI;
    [SerializeField] Image IllustUGUI;


    private void Start()
    {
        viewMode = Define.UnitViewMode.SPINEMODE;
        IllustChangeButton?.onClick.AddListener(OnClick);
        if(illustType == IllustType.Character)
        {
            TitleUI_Events.Del_CharacterClick += CharacterUpdate;
            CharacterUpdate();
        }
        else if (illustType == IllustType.Tower)
        {
            TitleUI_Events.Del_Tower_Click += TowerUpdate;
            TowerUpdate();
        }
    }


    void Update()
    {
        if(Define.unitViewMode != viewMode)
        {
            viewMode = Define.unitViewMode; IlustUpdate();
            if (illustType == IllustType.Character)
            {
                CharacterUpdate();
            }
            else if (illustType == IllustType.Tower)
            {
                TowerUpdate();
            }
        }
    }


    public void OnClick()
    {
        if(Define.unitViewMode == Define.UnitViewMode.ILLUSTMODE)
            Define.unitViewMode = Define.UnitViewMode.SPINEMODE;
        else { Define.unitViewMode = Define.UnitViewMode.ILLUSTMODE; }
    }

    private void TowerUpdate()
    {
        if (skeletonUGUI == null) { Debug.Log($"스켈레톤 UGUI가 null입니다."); return; }
        //스켈레톤 데이터 업데이트
        var character = UserInfo.instance.Userinfo.GetCurrentTowerDB;
        skeletonUGUI.skeletonDataAsset = character.skeleton;
        skeletonUGUI.Skeleton.SetSkin("default");
        skeletonUGUI.startingAnimation = character.skeleton.GetSkeletonData(true).Animations.Items[0].Name;
        skeletonUGUI.Initialize(true);
        //일러스트 업데이트
        IllustUGUI.sprite = character.Data.icon;    //일단 아이콘으로 대체
    }
    private void CharacterUpdate()
    {
        if (skeletonUGUI == null) { Debug.Log($"스켈레톤 UGUI가 null입니다."); return; }
        //스켈레톤 데이터 업데이트
        var character = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
        skeletonUGUI.skeletonDataAsset = character.skeleton;
        var skin_name = character.current_Skin.Skin_Name;
        skeletonUGUI.initialSkinName = skin_name;
        skeletonUGUI.Skeleton.SetSkin(skin_name);
        skeletonUGUI.startingAnimation = character.skeleton.GetSkeletonData(true).Animations.Items[3].Name;
        skeletonUGUI.Initialize(true);
        //일러스트 업데이트
        IllustUGUI.sprite = character.Data.icon;    //일단 아이콘으로 대체
    }
    private void CharacterSkinUpdate(UserSaveData.Character_DB.Skin_DB skinDB)
    {
        if (skeletonUGUI == null) { Debug.Log($"스켈레톤 UGUI가 null입니다."); return; }
        //스켈레톤 데이터 업데이트
        var character = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
        skeletonUGUI.skeletonDataAsset = character.skeleton;
        var skin_name = skinDB.Skin_Name;
        skeletonUGUI.Skeleton.SetSkin(skin_name);
        skeletonUGUI.startingAnimation = character.skeleton.GetSkeletonData(true).Animations.Items[3].Name;
        skeletonUGUI.Initialize(true);
        //일러스트 업데이트
        IllustUGUI.sprite = character.Data.icon;    //일단 아이콘으로 대체
    }


    private void IlustUpdate()
    {
        if(viewMode == Define.UnitViewMode.ILLUSTMODE)
        { 
            skeletonUGUI.gameObject.SetActive(false);
            IllustUGUI.gameObject.SetActive(true);
        }
        else if(viewMode == Define.UnitViewMode.SPINEMODE)
        { 
            skeletonUGUI.gameObject.SetActive(true);
            IllustUGUI.gameObject.SetActive(false);
        }
    }
}
