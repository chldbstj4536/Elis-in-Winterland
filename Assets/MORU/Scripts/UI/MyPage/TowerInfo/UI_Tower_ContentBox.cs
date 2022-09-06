using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using YS;
using Moru;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;

namespace Moru.UI
{

    public class UI_Tower_ContentBox : MonoBehaviour
    {
        #region Ref
        [SerializeField] SkeletonGraphic skeleton_UGUI;
        [SerializeField] private GameObject Background;
        [SerializeField] private GameObject Seleted_Frame;

        #endregion

        #region  Varis&Property
        [SerializeField] private int adress;

        [SerializeField] private UserSaveData.Tower_DB data;
        public UserSaveData.Tower_DB Data => data;

        public int Level => Data != null ? Data.Level : 0;
        public string Name => Data != null ? Data.Data.name : "";
        public string LevelString => Data != null ? $"Lv. {Level:D2}": "Lv. 0";
        public bool IsHave => Data != null ? Data.IsHaving : false;

        public TOWER_INDEX myIndex { get; private set; }

        [SerializeField] public SkeletonDataAsset skeleton => Data != null ? data.skeleton : null;
        [SerializeField] public Color SkeletonColor => IsHave ? haveColor : noHaveColor;
        [SerializeField] private Color haveColor;
        [SerializeField] private Color noHaveColor;

        private Moru.UI.TowerDnD DnD;

        bool isFirst = true;
        #endregion
        void OnEnable()
        {
            //별짓거리를 다해봐도 안되는 스켈레톤 스킨및 스켈레톤데이터 업데이트
            if (isFirst) { isFirst = false; return; }
            var skeletondata = UserSaveData.SkeletonSO.tower_Skeleton[(int)myIndex];
            skeleton_UGUI.skeletonDataAsset = skeletondata;
            var skin = skeleton_UGUI.SkeletonData.DefaultSkin;
            //skeleton_UGUI.initialSkinName = skin.Name;
            skeleton_UGUI.Skeleton.SetSkin("default");
            skeleton_UGUI.startingAnimation = skeletondata.GetSkeletonData(true).Animations.Items[0].Name;
            skeleton_UGUI.Skeleton.SetToSetupPose();
            skeleton_UGUI.Initialize(true);
        }


        void Awake()
        {
            TitleUI_Events.Del_Tower_Click += UpdateDnD;
            GetComponent<Button>().onClick.AddListener(OnClick);
            if (DnD == null) DnD = GetComponent<TowerDnD>();
        }

        /// <summary>
        /// 타워 콘텐츠 박스를 초기화합니다.
        /// </summary>
        /// <param name="myIndex"></param>
        /// <param name="DB"></param>
        public void Initialized(int myIndex, UserSaveData.Tower_DB DB)
        {
            //데이터가 없을 경우 게임오브젝트를 비활성화합니다.
            if (DB == null) { gameObject.SetActive(false); return; }
            else { gameObject.SetActive(true); }
            this.data = DB;
            this.adress = myIndex;
            this.myIndex = DB.Index;
            var isActiveBackground = IsHave ? false : true;
            Background?.SetActive(isActiveBackground);
            if (this.adress == 1)
            {
                UserInfo.instance.Userinfo.Current_TOWER_UNIT_INDEX = this.myIndex;
                TitleUI_Events.Del_Tower_Click?.Invoke();
            }
            //스켈레톤 업데이트

        }

        public void OnClick()
        {
            UserInfo.instance.Userinfo.Current_TOWER_UNIT_INDEX = myIndex;
            TitleUI_Events.Del_Tower_Click?.Invoke();
        }

        public void UpdateDnD()
        {
            if(UserInfo.instance.Userinfo.Current_TOWER_UNIT_INDEX != myIndex)
            { 
                DnD.enabled = false;
                Seleted_Frame.SetActive(false);
            }
            else
            { 
                DnD.enabled = true;
                Seleted_Frame.SetActive(true);
            }
        }
    }


}