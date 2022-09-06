using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;



namespace Moru
{

    public class UserInfo : SingleToneMono<UserInfo>
    {
        /// <summary>
        /// 나중엔 데이터 로드결과를 통해 신규생성
        /// </summary>
        [SerializeField] UserSaveData userinfo = new UserSaveData("TestID", "처음플레이어", 3);
        [SerializeField] public UserSaveData Userinfo => userinfo;

        protected override void Awake()
        {
            base.Awake();
            
            userinfo.Initialized();
            StartCoroutine(Co_AutoSave());
        }

        IEnumerator Co_AutoSave()
        {
            var sec = new WaitForSecondsRealtime(0.1f);
            while(true)
            {
                JsonData.Save<UserSaveData.Character_DB>(userinfo.GetCurrentCharacterDB, "");
                yield return sec;
            }
        }

        public void GameOver(string StageName, bool isClear)
        {
            //게임데이터 세이브
            //userinfo.Stages[StageName].IsClear = isClear;
        }
    }

    [System.Serializable]
    public partial class UserSaveData
    {
        public UserSaveData(string ID, string NickName, int level)
        {
            userID = ID;
            userNick = NickName;
            playerLevel = level;
        }

        #region #0. 스크립터블 오브젝트

        [SerializeReference, ShowInInspector]
        private static PlayableUnitDataSO chararcterSO;
        public static PlayableUnitDataSO ChararcterSO
        {
            get
            {
                if (chararcterSO == null) { chararcterSO = Resources.Load<PlayableUnitDataSO>("Datas/Units/PlayableUnitData"); }
                if (chararcterSO != null) { }
                else { Debug.Log("캐릭터SO 리소스 할당 실패"); }
                return chararcterSO;
            }
        }

        [SerializeField, ShowInInspector]
        private static TowerDataSO towerSO;
        public static TowerDataSO TowerSO
        {
            get
            {
                if (towerSO == null) { towerSO = Resources.Load<TowerDataSO>("Datas/Units/TowerData"); }
                if (towerSO != null) { }
                else { Debug.Log("캐릭터SO 리소스 할당 실패"); }
                return towerSO;
            }
        }

        [SerializeField, ShowInInspector]
        private static SkeletonSO skeletonSO;
        public static SkeletonSO SkeletonSO
        {
            get
            {
                if (skeletonSO == null) { skeletonSO = Resources.Load<SkeletonSO>("SO/SkeletonSO"); }
                if (skeletonSO != null) { }
                else { Debug.Log("스켈레톤SO 리소스 할당 실패"); }
                return skeletonSO;
            }
        }

        [SerializeField, ShowInInspector] 
        private static MasterySO masterySO;
        public static MasterySO MasterySO
        {
            get
            {
                if (masterySO == null) { masterySO = Resources.Load<MasterySO>("SO/MasterySO"); }
                if (masterySO != null) { }
                else { Debug.Log("스켈레톤SO 리소스 할당 실패"); }
                return masterySO;
            }
        }

        [SerializeField, ShowInInspector]
        private static BindingDataSO bindingSO;
        public static BindingDataSO BindingSO
        {
            get
            {
                if (bindingSO == null) { bindingSO = Resources.Load<BindingDataSO>("SO/BindingDataSo"); }
                if (bindingSO != null) { }
                else { Debug.Log("바인딩SO 리소스 할당 실패"); }
                return bindingSO;
            }
        }

        #endregion


        #region #1. UserBasicData : 유저 기본정보

        [SerializeField, LabelText("유저 아이디")] string userID;
        public string UserID { get { return userID; } set { userID = value; } }

        [SerializeField, LabelText("유저 닉네임")] string userNick;
        public string UserNick { get { return userNick; } set { userNick = value; } }

        [SerializeField, LabelText("유저 레벨")] private int PlayerLevel;
        public int playerLevel { get { return PlayerLevel; } set { PlayerLevel = value; } }

        [SerializeField, LabelText("현재 경험치")] private int current_EXP;
        public int Current_EXP { get { return current_EXP; } }

        [SerializeField, LabelText("레벨업 요구 경험치")] private int target_EXP;
        public int Target_EXP { get { return target_EXP; } }
        [SerializeField, LabelText("경험치 퍼센트")]
        public float EXP_Percent
        {
            get
            {
                if (Target_EXP != 0) { return (float)Current_EXP / (float)Target_EXP; }
                else return 1;
            }
        }
        [SerializeField, LabelText("골드")]
        private int cash;
        public int Cash { get => cash; }

        #endregion


        #region #2. StageInfo : 스테이지에 대한 정보

        /////스테이지 정보/////
        [SerializeField, ReadOnly, LabelText("선택 스테이지")] private string selected_Stage;
        public string Selected_Stage { get { return selected_Stage; } set { selected_Stage = value; } }

        [ShowInInspector, LabelText("스테이지 정보")] public Dictionary<string, Stage_DB> Stages = new Dictionary<string, Stage_DB>();

        #endregion

        #region #4. 콜렉션&업적 정보

        #endregion


        #region Methods

        /// <summary>
        /// 유저정보를 초기화합니다.
        /// </summary>
        public void Initialized()
        {
            Debug.Log(ChararcterSO);
            Debug.Log(TowerSO);
            Debug.Log(SkeletonSO);
            Debug.Log(MasterySO);

            //캐릭터 리스트 초기화 : 캐릭터별 초기화
            Init_Character();
            //타워 리스트 초기화 : 타워별 초기화
            Init_Tower();

            //스테이지 리스트 초기화
            #region #Method_3. 스테이지 정보 초기화

            var Stages = Resources.LoadAll<Stage>("Datas/Stages/");
            Debug.Log($"스테이지 숫자 : {Stages.Length}");
            for (int i = 0; i < Stages.Length; i++)
            {
                var value = new Stage_DB(false, Stages[i]);
                this.Stages.Add(Stages[i].name, value);
            }

            #endregion
        }

        public void Select_Stage(string stageName)
        {
            selected_Stage = stageName;
        }

        /// <summary>
        /// 골드를 매개변수만큼 소비합니다.
        /// </summary>
        /// <param name="Cost"></param>
        public void Paid(int Cost) { }
        /// <summary>
        /// 골드를 매개변수만큼 벌어들입니다.
        /// </summary>
        /// <param name="Cost"></param>
        public void Earn(int Cost) { }
        #endregion


    }
}