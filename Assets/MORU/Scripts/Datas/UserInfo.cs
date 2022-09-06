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
        /// ���߿� ������ �ε����� ���� �űԻ���
        /// </summary>
        [SerializeField] UserSaveData userinfo = new UserSaveData("TestID", "ó���÷��̾�", 3);
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
            //���ӵ����� ���̺�
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

        #region #0. ��ũ���ͺ� ������Ʈ

        [SerializeReference, ShowInInspector]
        private static PlayableUnitDataSO chararcterSO;
        public static PlayableUnitDataSO ChararcterSO
        {
            get
            {
                if (chararcterSO == null) { chararcterSO = Resources.Load<PlayableUnitDataSO>("Datas/Units/PlayableUnitData"); }
                if (chararcterSO != null) { }
                else { Debug.Log("ĳ����SO ���ҽ� �Ҵ� ����"); }
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
                else { Debug.Log("ĳ����SO ���ҽ� �Ҵ� ����"); }
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
                else { Debug.Log("���̷���SO ���ҽ� �Ҵ� ����"); }
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
                else { Debug.Log("���̷���SO ���ҽ� �Ҵ� ����"); }
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
                else { Debug.Log("���ε�SO ���ҽ� �Ҵ� ����"); }
                return bindingSO;
            }
        }

        #endregion


        #region #1. UserBasicData : ���� �⺻����

        [SerializeField, LabelText("���� ���̵�")] string userID;
        public string UserID { get { return userID; } set { userID = value; } }

        [SerializeField, LabelText("���� �г���")] string userNick;
        public string UserNick { get { return userNick; } set { userNick = value; } }

        [SerializeField, LabelText("���� ����")] private int PlayerLevel;
        public int playerLevel { get { return PlayerLevel; } set { PlayerLevel = value; } }

        [SerializeField, LabelText("���� ����ġ")] private int current_EXP;
        public int Current_EXP { get { return current_EXP; } }

        [SerializeField, LabelText("������ �䱸 ����ġ")] private int target_EXP;
        public int Target_EXP { get { return target_EXP; } }
        [SerializeField, LabelText("����ġ �ۼ�Ʈ")]
        public float EXP_Percent
        {
            get
            {
                if (Target_EXP != 0) { return (float)Current_EXP / (float)Target_EXP; }
                else return 1;
            }
        }
        [SerializeField, LabelText("���")]
        private int cash;
        public int Cash { get => cash; }

        #endregion


        #region #2. StageInfo : ���������� ���� ����

        /////�������� ����/////
        [SerializeField, ReadOnly, LabelText("���� ��������")] private string selected_Stage;
        public string Selected_Stage { get { return selected_Stage; } set { selected_Stage = value; } }

        [ShowInInspector, LabelText("�������� ����")] public Dictionary<string, Stage_DB> Stages = new Dictionary<string, Stage_DB>();

        #endregion

        #region #4. �ݷ���&���� ����

        #endregion


        #region Methods

        /// <summary>
        /// ���������� �ʱ�ȭ�մϴ�.
        /// </summary>
        public void Initialized()
        {
            Debug.Log(ChararcterSO);
            Debug.Log(TowerSO);
            Debug.Log(SkeletonSO);
            Debug.Log(MasterySO);

            //ĳ���� ����Ʈ �ʱ�ȭ : ĳ���ͺ� �ʱ�ȭ
            Init_Character();
            //Ÿ�� ����Ʈ �ʱ�ȭ : Ÿ���� �ʱ�ȭ
            Init_Tower();

            //�������� ����Ʈ �ʱ�ȭ
            #region #Method_3. �������� ���� �ʱ�ȭ

            var Stages = Resources.LoadAll<Stage>("Datas/Stages/");
            Debug.Log($"�������� ���� : {Stages.Length}");
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
        /// ��带 �Ű�������ŭ �Һ��մϴ�.
        /// </summary>
        /// <param name="Cost"></param>
        public void Paid(int Cost) { }
        /// <summary>
        /// ��带 �Ű�������ŭ ������Դϴ�.
        /// </summary>
        /// <param name="Cost"></param>
        public void Earn(int Cost) { }
        #endregion


    }
}