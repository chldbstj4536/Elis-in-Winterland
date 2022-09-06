using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Assertions;
using DamageNumbersPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class GameManager : SingleToneMono<GameManager>
    {
        #region Field
        [SerializeField]
        private GameResult gameResult;

        #region ���� ����
        [BoxGroup("���� ����", true, true), SerializeField]
        [LabelText("ī�޶�")]
        private CameraController cam;
        #region �ҿ� ī��
        [BoxGroup("���� ����/�ҿ� ī��", true, true), SerializeField]
        [LabelText("�ҿ�ī�� ���� ������")]
        private PoolingComponent soulCardInfoPrefab;
        [BoxGroup("���� ����/�ҿ� ī��"), Min(0.0f), SerializeField]
        [LabelText("�̺�Ʈ �߻� ���ð�"), Tooltip("���̺� ��� ���� �� �ҿ�ī�� �̺�Ʈ �߻��� ���ð�")]
        private float soulCardEventDelayTime;
        [BoxGroup("���� ����/�ҿ� ī��"), Min(0), SerializeField]
        [LabelText("���ΰ�ħ �ǹ�����"), Tooltip("�������� ������ �ҿ�ī���� ���ΰ�ħ �� �⺻ �ǹ� �Ҹ𰡰�")]
        private int defaultSoulCardRerollCost;
        [BoxGroup("���� ����/�ҿ� ī��"), Min(0.0f), SerializeField]
        [LabelText("���ΰ�ħ �ǹ����� ������"), Tooltip("���ΰ�ħ �� ������������\n���� ���ΰ�ħ �ǹ����ݿ��� �����Ͽ� �ٽ� �ʱ�ȭ�˴ϴ�.\n(ex) ���ΰ�ħ �ǹ����� 10, ������ 2, ���ΰ�ħ 3ȸ ���� �� ���� ���ΰ�ħ ���� : 10*2*2 = 40")]
        private float rerollSoulCardCostIncRate;
        [BoxGroup("���� ����/�ҿ� ī��"), Min(0.0f), SerializeField]
        [LabelText("�ҿ�ī�� ���ýð�"), Tooltip("�ҿ�ī�带 �����ϴµ� �־����� �ð�")]
        private float soulCardEventTime;

        private List<SoulCardInfoComponent> soulCardUIs = new List<SoulCardInfoComponent>();
        private int soulCardAppearCount = 3;
        private int curRerollCost;
        private float curSCEventProgressTime;
        #endregion
        #region �µ�
        [BoxGroup("���� ����/�µ�", true, true), Min(1), SerializeField]
        [LabelText("�µ� ���� �ܰ� ����"), Tooltip("�µ��� �ܰ踦 �����ϴ� ������ ��ġ")]
        private int tempLevelInterval = 5;
        [BoxGroup("���� ����/�µ�"), Min(1), SerializeField]
        [LabelText("�µ� ���� �ִ� �ܰ�"), Tooltip("�µ��� �ܰ谡 �ö� �� �ִ� �ִ� ��ġ")]
        private int tempMaxLevel = 4;
        private int curTempLevel = 0;
        [BoxGroup("���� ����/�µ�"), SerializeField]
        [LabelText("�µ� �ϰ� ȿ�� ���׸���")]
        private Material camColdFXMtrl;
        [BoxGroup("���� ����/�µ�"), SerializeField]
        [LabelText("�µ� �ϰ� �ְ� ȿ�� ���׸���")]
        private Material camColdDistortionFXMtrl;
        [BoxGroup("���� ����/�µ�"), SerializeField]
        [LabelText("�µ� ��� ȿ�� ���׸���")]
        private Material camWarmFXMtrl;
        // ī�޶� �µ� ȿ���� ���� ����
        private float curTempRatio;
        private float aimTempRatio;
        private float tempRatioTime;
        #endregion
        #region �ܰ���
        [BoxGroup("���� ����/�ܰ���", true, true), SerializeField]
        [LabelText("�ܰ��� ���׸���")]
        private Material outlineMtrl;
        [BoxGroup("���� ����/�ܰ���"), SerializeField]
        [LabelText("���� �ܰ��� ����")]
        private Color infoColor = new Color(0.0f, 0.3f, 1.0f);
        [BoxGroup("���� ����/�ܰ���"), SerializeField]
        [LabelText("���õ� ������Ʈ �ܰ��� ����")]
        private Color selectedObjColor = new Color(0.0f, 0.4f, 1.0f);
        [BoxGroup("���� ����/�ܰ���"), SerializeField]
        [LabelText("Ÿ�� ���� �ܰ��� ����")]
        private Color selectTileColor = Color.yellow;
        [BoxGroup("���� ����/�ܰ���"), SerializeField]
        [LabelText("Ÿ�� ���� �ܰ��� ����")]
        private Color selectTargetColor = Color.red;
        #endregion
        #region ��Ʈ
        [BoxGroup("���� ����/��Ʈ", true, true)]
        [LabelText("���ط� ��Ʈ")]
        public DamageNumber dmgFont;
        [BoxGroup("���� ����/��Ʈ")]
        [LabelText("ȸ���� ��Ʈ")]
        public DamageNumber healFont;
        [BoxGroup("���� ����/��Ʈ")]
        [LabelText("��Ʈ ���ط� ��Ʈ")]
        public DamageNumber dotFont;
        [BoxGroup("���� ����/��Ʈ")]
        [LabelText("��� ȹ�� ��Ʈ")]
        public DamageNumber silverFont;
        [BoxGroup("���� ����/��Ʈ")]
        [LabelText("����ġ ȹ�� ��Ʈ")]
        public DamageNumber expFont;
        #endregion

        #region UI
        [FoldoutGroup("���� ����/UI", false), SerializeField]
        [LabelText("�� ��ŵ ��ư"), Tooltip("���ð����� ��ٸ��� �ʰ� �ٷ� ������ ��ŵ�ϴ� ��ư")]
        private Button btn_Skip;
        [FoldoutGroup("���� ����/UI"), SerializeField]
        [LabelText("���� �ؽ�Ʈ"), Tooltip("ȭ�鿡 ���� ������ �˷��ִ� UI�� �ؽ�Ʈ ������Ʈ")]
        private TMP_Text tmp_InfoText;
        [FoldoutGroup("���� ����/UI"), SerializeField]
        [LabelText("���� ���� HP�� UI")]
        public EpicInfoBar epicInfoBar;
        [FoldoutGroup("���� ����/UI/��� �����ܵ�", false), SerializeField]
        [LabelText("���� ��� �����ܵ�")]
        private GameObject[] warningLeftIcons;
        [FoldoutGroup("���� ����/UI/��� �����ܵ�"), SerializeField]
        [LabelText("������ ��� �����ܵ�")]
        private GameObject[] warningRightIcons;
        [FoldoutGroup("���� ����/UI/��� �߾� UI", false), HideLabel, SerializeField]
        private TopUIData topUIData;
        [FoldoutGroup("���� ����/UI/�»�� ���̺� ���� UI", false), HideLabel, SerializeField]
        private WaveInfoUIData waveInfoUIData;
        private SortedDictionary<MONSTER_INDEX, uint> remainWaveMonsters = new SortedDictionary<MONSTER_INDEX, uint>();
        private Dictionary<MONSTER_INDEX, uint> pairIconIndex = new Dictionary<MONSTER_INDEX, uint>();
        [FoldoutGroup("���� ����/UI/�ϴ� �߾� UI", false), HideLabel, SerializeField]
        private BottomUIData bottomUIData;
        [FoldoutGroup("���� ����/UI/���ϴ� ���õ� ������Ʈ ���� UI", false), HideLabel, SerializeField]
        private SelectedInfoUIData selectedInfoUIData;
        [FoldoutGroup("���� ����/UI/�ҿ�ī�� UI", false), HideLabel, SerializeField]
        private SoulCardUIData soulCardUIData;
        [FoldoutGroup("���� ����/UI/�¸� UI", false), SerializeField]
        [LabelText("�¸� UI �г�")]
        private GameObject gameClearUI;
        [FoldoutGroup("���� ����/UI/�¸� UI"), HideLabel, SerializeField]
        private ClearUIData clearUIData;
        [FoldoutGroup("���� ����/UI/�й� UI", false), SerializeField]
        [LabelText("�й� UI �г�")]
        private GameObject gameLoseUI;
        [FoldoutGroup("���� ����/UI/�й� UI"), HideLabel, SerializeField]
        private LoseUIData loseUIData;
        #endregion
        #endregion

        private int silver;
        private int temperature;

        private Stage stage;
        private uint waveIndex;
        // ���̺갡 �����°�
        private bool bWaveDone;
        // ���� �������ΰ�
        private bool isInBattle = false;

        // ���� ���� �ð�
        private float gameTime;

        // ���ֵ�
        private PlayableUnit player;
        private Core core;
        private Dictionary<int, Unit> epics = new Dictionary<int, Unit>();
        private Dictionary<int, Unit> enemies = new Dictionary<int, Unit>();
        private Dictionary<int, Unit> aligns = new Dictionary<int, Unit>();

        private GameSetting setting = new GameSetting() { isUsingFastSkill = true, invisibleInFullHP = true };
        
        // ���� ������ ��ġ ����� ����Ʈ
        private List<Vector3> spawnPosRecord = new List<Vector3>();
        // ���õ� ������Ʈ ����
        [HideInInspector]
        public Transform selectedTr;

        private TileManager tm;

        public const float HalfSizeField_X = 30.0f;
        public const float HalfSizeField_Z = 4.25f;
        #endregion

        #region Events
        public delegate void OnSpawnTower(Tower tower);

        public event OnChangedValue OnChangedSilver;
        public event OnChangedValue OnChangedTemperature;
        public event OnChangedValue OnChangedWaveIndex;
        public event OnChangedValue OnChangedTemperatureLevel;

        public event OnSpawnTower OnSpawnTowerEvent;

        public event Delegate_NoRetVal_NoParam OnBeginReadyForBattle;
        public event Delegate_NoRetVal_NoParam OnEndReadyForBattle;
        public event Delegate_NoRetVal_NoParam OnBeginBattle;
        public event Delegate_NoRetVal_NoParam OnEndBattle;
        #endregion

        #region Properties
        public int Silver
        {
            get { return silver; }
            set
            {
                silver = value;
                OnChangedSilver?.Invoke();
            }
        }
        public int Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnChangedTemperature?.Invoke();
            }
        }
        public uint WaveIndex
        {
            get { return waveIndex; }
            private set
            {
                waveIndex = value;
                OnChangedWaveIndex?.Invoke();
            }
        }
        public int CurrentTempLevel
        {
            get { return curTempLevel; }
            private set
            {
                curTempLevel = value;
                OnChangedTemperatureLevel?.Invoke();
            }
        }
        public GameSetting Setting => setting;
        public PlayableUnit Player => player;
        public Core Core => core;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            tm = TileManager.Instance;

            camColdDistortionFXMtrl.SetFloat("_DistortAmount", 0.0f);
            camColdFXMtrl.SetFloat("_Ice", 0.0f);
            camWarmFXMtrl.SetFloat("_Radial", 0.0f);

            ResourcePoolling();
            LoadData();
            BindPlayerInfoEvent();

            OnChangedSilver += () =>
            {
                topUIData.tmp_Silver.text = silver.ToString();
            };
            OnChangedTemperature += () =>
            {
                topUIData.tmp_Temperature.text = Temperature.ToString() + "��C";
                int tempLevel = Mathf.Min(Temperature / tempLevelInterval, tempMaxLevel);
                if (CurrentTempLevel != tempLevel)
                    CurrentTempLevel = tempLevel;
            };
            OnChangedWaveIndex += () =>
            {
                topUIData.tmp_Wave.text = waveIndex.ToString();
            };
            OnChangedTemperatureLevel += () =>
            {
                aimTempRatio = CurrentTempLevel / (float)tempMaxLevel;
                tempRatioTime = 0.0f;
            };

            btn_Skip.onClick.AddListener(FromReadyToBattle);
            clearUIData.returnTitleBtn.onClick.AddListener(ReturnToTitle);
            clearUIData.returnLobbyBtn.onClick.AddListener(ReturnToLobby);
            clearUIData.nextStageBtn.onClick.AddListener(GoToNextStage);
            loseUIData.returnTitleBtn.onClick.AddListener(ReturnToTitle);
            loseUIData.returnLobbyBtn.onClick.AddListener(ReturnToLobby);

            StartCoroutine(InfoTextAnimation());
            StartCoroutine(CameraTempFXUpdate());
        }
        private void Start()
        {
            WaveIndex = 0;
            SetNextWaveInfo();
            OnBeginReadyForBattle?.Invoke();

            bottomUIData.hpStatUI.BarUpdate(player.CurrentHP / player.MaxHP);
            bottomUIData.hpStatUI.BarUpdate(player.CurrentHP / player.MaxHP);
            bottomUIData.mpStatUI.BarUpdate(player.CurrentMP / player.MaxMP);
            bottomUIData.mpStatUI.BarUpdate(player.CurrentMP / player.MaxMP);
            bottomUIData.expStatUI.BarUpdate(player.CurrentExp / player.MaxExp);
            bottomUIData.expStatUI.BarUpdate(player.CurrentExp / player.MaxExp);
            bottomUIData.levelStatUI.StatUpdate(player.Level.ToString());
            bottomUIData.physxPowerStatUI.StatUpdate(player.TotalPhysicsPower.ToString());
            bottomUIData.magicPowerStatUI.StatUpdate(player.TotalMagicPower.ToString());
            bottomUIData.armorStatUI.StatUpdate(player.TotalArmor.ToString());
            bottomUIData.armorPntStatUI.StatUpdate(player.TotalArmorPnt.ToString());
            bottomUIData.atkSpdStatUI.StatUpdate(player.TotalAttackSpeed.ToString());
            bottomUIData.moveSpdStatUI.StatUpdate(player.mc.TotalMoveSpeed.ToString());
            bottomUIData.criticalRateStatUI.StatUpdate(player.TotalCriticalRate.ToString());
            bottomUIData.cdrStatUI.StatUpdate(player.TotalCDR.ToString());

            Invoke(nameof(FromReadyToBattle), stage.Waves[WaveIndex].readyTime);
        }
        private void Update()
        {
            CheckSelectedObject();
            if (soulCardUIData.panel_SoulCardUI.activeInHierarchy)
                UpdateSelectSoulCardEvent();

            gameTime += Time.deltaTime;

            string min = ((int)gameTime / 60).ToString();
            string sec = ((int)gameTime % 60).ToString();
            if (min.Length == 1)    min.Insert(0, "0");
            if (sec.Length == 1)    sec.Insert(0, "0");

            Utility.stringBuilder.Clear();
            Utility.stringBuilder.Append(min.Length == 1 ? min.Insert(0, "0") : min);
            Utility.stringBuilder.Append(":");
            Utility.stringBuilder.Append(sec.Length == 1 ? sec.Insert(0, "0") : sec);

            topUIData.tmp_Time.text = Utility.stringBuilder.ToString();

            if (Input.GetKeyDown(KeyCode.K))
                BeginSelectSoulCardEvent();
        }
        private void OnDestroy()
        {
            PrefabPool.Clear();
        }
        #endregion

        #region Methods
        public void CloseUI()
        {

        }
        public void PauseGame()
        {

        }
        public void SetSelectedMonsterInfoUI(Monster monster)
        {
            selectedInfoUIData.tmp_SelectedInfoLv.text = monster.Level.ToString() + ".Lv";
            selectedInfoUIData.img_SelectedInfoIcon.sprite = monster.Icon;
            selectedInfoUIData.tmp_SelectedInfoName.text = monster.Name;
            selectedInfoUIData.tmp_SelectedInfoTitle.text = "����";
            selectedInfoUIData.tmp_SelectedInfoDesc.text = monster.Description;
        }
        public void SetSelectedTowerInfoUI(Tower tower)
        {
            selectedInfoUIData.tmp_SelectedInfoLv.text = tower.Level.ToString() + ".Lv";
            selectedInfoUIData.img_SelectedInfoIcon.sprite = tower.Icon;
            selectedInfoUIData.tmp_SelectedInfoName.text = tower.Name;
            selectedInfoUIData.tmp_SelectedInfoTitle.text = "����";
            selectedInfoUIData.tmp_SelectedInfoDesc.text = tower.Description;
        }
        //public void SetSelectedItemInfoUI(Item item)
        //{

        //}
        public List<Unit> GetAllUnits()
        {
            List<Unit> units = enemies.Values.ToList();
            units.AddRange(aligns.Values);
            units.Add(player);
            units.Add(core);

            return units;
        }
        public List<Unit> GetAlignUnits()
        {
            return aligns.Values.ToList();
        }
        public List<Unit> GetMonsterUnits()
        {
            return enemies.Values.ToList();
        }
        public List<Unit> GetEpicMonsters()
        {
            return epics.Values.ToList();
        }
        public bool BuyAndBuildTower(EquipTowerInfo eti)
        {
            int spawnCost = UnitSO.TowerData[eti.ti].spawnCost;
            if (Silver < spawnCost)
                return false;

            if (!Utility.GetMouseRaycast(out RaycastHit hit, (int)LAYER_MASK.TILE))
                return false;

            // Ÿ�� ����
            Tower tower = hit.transform.GetComponent<TileComponent>().BuildTower(eti.ti, eti.lv);
            tower.OnTowerDestroyEvent += OnTowerDestroy;
            Silver -= UnitSO.TowerData[eti.ti].spawnCost;

            aligns.Add(tower.GetInstanceID(), tower);

            OnSpawnTowerEvent?.Invoke(tower);

            return true;
        }
        public void BeginSelectSoulCardEvent()
        {
            Time.timeScale = 0.0f;
            curSCEventProgressTime = 0.0f;
            curRerollCost = 0;
            soulCardUIData.panel_SoulCardUI.SetActive(true);

            for (int i = 0; i < soulCardAppearCount; ++i)
            {
                var scInfo = PrefabPool.GetObject(soulCardInfoPrefab).GetComponent<SoulCardInfoComponent>();
                scInfo.transform.SetParent(soulCardUIData.panel_SoulCard);
                scInfo.OnSelectSoulcardEvent += (SOULCARD_INDEX si) =>
                {
                    EndSelectSoulCardEvent(si);
                };
                soulCardUIs.Add(scInfo);
            }

            ReRollSoulCard();

            #region �ҿ�ī�� ��ġ ���� �ڵ�
            Rect panelRect = soulCardUIData.panel_SoulCard.rect;
            Rect rcRect = soulCardInfoPrefab.GetComponent<RectTransform>().rect;

            RectTransform rectTr;

            float xSpace, ySpace;

            xSpace = (panelRect.width - (soulCardAppearCount * rcRect.width * 1.5f)) / (soulCardAppearCount + 1);
            ySpace = panelRect.height / 2;

            for (int i = 0; i < soulCardAppearCount; ++i)
            {
                rectTr = soulCardUIs[i].GetComponent<RectTransform>();
                rectTr.pivot = Vector2.one * 0.5f;
                rectTr.anchorMin = rectTr.anchorMax = Vector2.zero;
                rectTr.localScale = Vector3.one * 1.5f;
                rectTr.anchoredPosition = new Vector3((xSpace * (i + 1)) + (rcRect.width * 1.5f * i) + (rcRect.width * 0.75f), ySpace, 0.0f);
            }
            #endregion
        }
        public void UpdateSelectSoulCardEvent()
        {
            curSCEventProgressTime += Time.unscaledDeltaTime;
            soulCardUIData.tmp_CountDown.text = ((int)(soulCardEventTime - curSCEventProgressTime)).ToString();

            // ī��Ʈ�ٿ��� �������� �������� ī�带 �̰� �̺�Ʈ ����
            if (curSCEventProgressTime >= soulCardEventTime)
            {
                EndSelectSoulCardEvent(soulCardUIs[Random.Range(0, soulCardUIs.Count)].SoulCardIndex);
                return;
            }
        }
        public void EndSelectSoulCardEvent(SOULCARD_INDEX si)
        {
            Time.timeScale = 1.0f;

            // �÷��̾�� �ҿ�ī�� �߰�
            player.AddSoulCard(si);

            for (int i = 0; i < soulCardUIs.Count; ++i)
                soulCardUIs[i].Release();
            soulCardUIs.Clear();
            soulCardUIData.panel_SoulCardUI.SetActive(false);
        }
        public void ReRollSoulCard()
        {
            Silver -= curRerollCost;
            curRerollCost = curRerollCost == 0 ? defaultSoulCardRerollCost : (int)(curRerollCost * rerollSoulCardCostIncRate);

            soulCardUIData.tmp_Reroll.text = $"���ΰ�ħ\n({curRerollCost})";
            soulCardUIData.btn_Reroll.interactable = Silver >= curRerollCost;

            var drawedSoulcards = player.DrawSoulCard(soulCardAppearCount);

            int i = 0;
            foreach (var soulCardIndex in drawedSoulcards)
            {
                SoulCardInfoComponent ui = soulCardUIs[i];
                SoulCardData data = Unit.SoulCard.SoulCardDatas[soulCardIndex];
                ui.SetSoulCardUI(soulCardIndex, data.name, data.Description, data.icon);
                ++i;
            }
        }
        private void LoadData()
        {
            // �����δ� Ÿ��Ʋ ������ ���� ���� ������ �����ؾ���
            InGameLoadData ld = FindObjectOfType<InGameLoadData>();

            gameResult.stage = ld.stage;

            // �ھ� ����
            core = UnitSO.CoreData[ld.coreIndex].Instantiate(0, Vector3.zero);
            core.OnDisableEvent += GameLose;

            player = UnitSO.PlayableUnitData[ld.playerIndex].Instantiate(ld.playerLv, ld.playerSkin, Vector3.zero, ld.skillSet, ld.deck);
            player.gameObject.SetActive(true);
            player.OnDisableEvent += GameLose;
            player.OnInteract += OnInteract;

            // ĳ���Ϳ� ���� UI �ʱ�ȭ
            bottomUIData.charImg.sprite = player.Icon;
            bottomUIData.charImg.rectTransform.anchoredPosition = player.IconOffset;
            // ī�޶� Ÿ�� ����
            cam.target = player.transform;
            // Ÿ�� ����
            player.towerSet = ld.towerSet;

            // ��ų�� ���� UI �ʱ�ȭ
            ChangeSwitchIcon(player.IsUsingTowerSwitch);

            // �������� ����
            stage = ResourceManager.GetResource<Stage>("Datas/Stages/" + ld.stage);
            topUIData.tmp_Stage.text = ld.stage;

            Silver = 5000;

            Destroy(ld.gameObject);
        }
        private void ResourcePoolling()
        {
            // �ڽ�ų Ǯ��
            //Unit.Skill.SkillInitialize();
        }
        private void SetNextWaveInfo()
        {
            for (int i = 0; i < waveInfoUIData.icons.Length; ++i)
                waveInfoUIData.icons[i].Remove();
            pairIconIndex.Clear();
            remainWaveMonsters.Clear();

            foreach (var timeline in stage.Waves[WaveIndex].Timeline)
            {
                foreach (var monster in timeline.spawnMonsters)
                {
                    if (remainWaveMonsters.ContainsKey(monster.monsterIndex))
                        ++remainWaveMonsters[monster.monsterIndex];
                    else
                        remainWaveMonsters.Add(monster.monsterIndex, 1);
                }
            }

            uint index = 0;
            foreach (var monsterCountInfo in remainWaveMonsters)
            {
                pairIconIndex.Add(monsterCountInfo.Key, index);
                waveInfoUIData.icons[index++].Initialize(monsterCountInfo.Key, monsterCountInfo.Value);
            }
        }
        private void FromBattleToReady()
        {
            if (!isInBattle)
                return;

            isInBattle = false;

            OnEndBattle?.Invoke(); 

            if (WaveIndex == stage.Waves.Length)
            {
                GameClear();
                return;
            }

            btn_Skip.gameObject.SetActive(true);
            SetNextWaveInfo();
            OnBeginReadyForBattle?.Invoke();

            Invoke(nameof(BeginSelectSoulCardEvent), soulCardEventDelayTime);
            Invoke(nameof(FromReadyToBattle), stage.Waves[WaveIndex].readyTime);
        }
        private void FromReadyToBattle()
        {
            if (isInBattle)
                return;

            CancelInvoke(nameof(FromReadyToBattle));

            btn_Skip.gameObject.SetActive(false);
            isInBattle = true;
            ++WaveIndex;
            OnEndReadyForBattle?.Invoke();

            OnBeginBattle?.Invoke();
            StartCoroutine(WaveStart());
        }
        private void GameClear()
        {
            gameResult.result = true;

            gameClearUI.SetActive(true);
        }
        private void GameLose()
        {
            if (gameLoseUI == null || gameLoseUI.activeInHierarchy)
                return;

            gameResult.result = false;

            gameLoseUI.SetActive(true);
            InputManager.ClearEvent();
        }
        private void OnTowerDestroy(Tower tower)
        {
            aligns.Remove(tower.GetInstanceID());
        }
        private void OnMonsterDie(Monster monster)
        {
            enemies.Remove(monster.GetInstanceID());

            if (monster.IsEpicMonster)
            {
                epics.Remove(monster.GetInstanceID());
                if (epics.Count == 0)
                    epicInfoBar.gameObject.SetActive(false);
                else
                    foreach (var epic in epics)
                        epic.Value.unitInfoBar.SetVisible(epics.Count != 1);
            }

            if (bWaveDone && enemies.Count == 0)
                FromBattleToReady();
        }
        private void SpawnMonster(MONSTER_INDEX index, int lv, Vector3 pos, int zLaneNum, bool isEpicMonster)
        {
            // ���� ����
            Monster monster = UnitSO.MonsterData[index].Instantiate(lv, pos, zLaneNum, isEpicMonster);
            
            monster.OnMonsterDieEvent += OnMonsterDie;

            enemies.Add(monster.GetInstanceID(), monster);
            waveInfoUIData.icons[pairIconIndex[index]].ChangeCount(--remainWaveMonsters[index]);

            if (isEpicMonster)
            {
                epics.Add(monster.GetInstanceID(), monster);
                epicInfoBar.gameObject.SetActive(true);
                epicInfoBar.AddEpicMonster(monster);

                if (epics.Count == 1)
                    monster.unitInfoBar.SetVisible(false);
                else
                    foreach (var epic in epics)
                        epic.Value.unitInfoBar.SetVisible(true);
            }
        }
        private void CheckSelectedObject()
        {
            Transform newTr = null;

            switch (player.InteractState)
            {
                case PlayableUnit.INTERACT_STATE.CHOOSING_TILE:
                    {
                        outlineMtrl.SetColor("_OutlineColor", selectTileColor);

                        if (Utility.GetMouseRaycast(out RaycastHit hit, (int)LAYER_MASK.TILE))
                            newTr = hit.transform;
                    }
                    break;
                case PlayableUnit.INTERACT_STATE.CHOOSING_TARGET:
                    {
                        outlineMtrl.SetColor("_OutlineColor", selectTargetColor);

                        if (Utility.GetUnitMouseRaycast(out RaycastHit hit, player.ChoosingTargetLayerMask, player.ChoosingTargetType))
                            newTr = hit.transform;
                    }
                    break;
                case PlayableUnit.INTERACT_STATE.NONE:
                    {
                        outlineMtrl.SetColor("_OutlineColor", infoColor);

                        if (Utility.GetMouseRaycast(out RaycastHit hit, (int)(LAYER_MASK.TEAM1 | LAYER_MASK.TEAM2 | LAYER_MASK.ITEM)))
                        {
                            var baseObj = hit.transform.GetComponent<BaseObject>();
                            if (!(baseObj is PlayableUnit || baseObj is Core))
                                newTr = hit.transform;
                        }
                    }
                    break;
            }

            if (newTr == null && selectedTr != null)
            {
                selectedTr.GetChild(0).gameObject.layer = 0;
                selectedTr = null;
            }
            else if (newTr != null)
            {
                if (selectedTr != null)
                    selectedTr.GetChild(0).gameObject.layer = 0;
                selectedTr = newTr;
                selectedTr.GetChild(0).gameObject.layer = (int)LAYER_NUMBER.OUTLINE;
            }
        }
        private void OnInteract()
        {
            switch (player.InteractState)
            {
                case PlayableUnit.INTERACT_STATE.NONE:
                    if (selectedTr == null)
                        selectedInfoUIData.panel_SelectedInfo.SetActive(false);
                    else
                    {
                        selectedInfoUIData.panel_SelectedInfo.SetActive(true);

                        var obj = selectedTr.GetComponent<BaseObject>();

                        if (obj is Unit)
                        {
                            switch ((obj as Unit).UnitType)
                            {
                                case UNIT_TYPE.TOWER:
                                    SetSelectedTowerInfoUI(obj as Tower);
                                    break;
                                case UNIT_TYPE.MONSTER:
                                    SetSelectedMonsterInfoUI(obj as Monster);
                                    break;
                            }
                        }
                        else
                        {
                            //SetSelectedItemInfoUI(obj);
                        }
                    }
                    break;
            }
        }
        private void BindPlayerInfoEvent()
        {
            for (int i = 0; i < 6; ++i)
            {
                if (player.Skills[i] != null)
                {
                    int index = i;
                    player.Skills[index].ASkill.OnChangedRemainCoolTime += () =>
                    {
                        var icon = bottomUIData.icons_TS[index];
                        var skill = player.Skills[index].ASkill;

                        if (!player.IsUsingTowerSwitch && skill.CurrentCombo == 0)
                            icon.fillAmount = 1 - skill.RemainCoolTime / skill.CoolTime;
                    };
                }
            }
            player.OnChangedCurrentHP += () => { bottomUIData.hpStatUI.BarUpdate(player.CurrentHP / player.MaxHP); };
            player.OnChangedMaxHP += () => { bottomUIData.hpStatUI.BarUpdate(player.CurrentHP / player.MaxHP); };
            player.OnChangedCurrentMP += () => { bottomUIData.mpStatUI.BarUpdate(player.CurrentMP / player.MaxMP); };
            player.OnChangedMaxMP += () => { bottomUIData.mpStatUI.BarUpdate(player.CurrentMP / player.MaxMP); };
            player.OnChangedCurrentExp += () => { bottomUIData.expStatUI.BarUpdate(player.CurrentExp / player.MaxExp); };
            player.OnChangedMaxExp += () => { bottomUIData.expStatUI.BarUpdate(player.CurrentExp / player.MaxExp); };
            player.OnChangedLevel += () => { bottomUIData.levelStatUI.StatUpdate(player.Level.ToString()); };
            player.OnChangedTotalPhysicsPower += () => { bottomUIData.physxPowerStatUI.StatUpdate(player.TotalPhysicsPower.ToString()); };
            player.OnChangedTotalMagicPower += () => { bottomUIData.magicPowerStatUI.StatUpdate(player.TotalMagicPower.ToString()); };
            player.OnChangedTotalArmor += () => { bottomUIData.armorStatUI.StatUpdate(player.TotalArmor.ToString()); };
            player.OnChangedTotalArmorPnt += () => { bottomUIData.armorPntStatUI.StatUpdate(player.TotalArmorPnt.ToString()); };
            player.OnChangedTotalAttackSpeed += () => { bottomUIData.atkSpdStatUI.StatUpdate(player.TotalAttackSpeed.ToString()); };
            player.mc.OnChangedTotalMoveSpeed += () => { bottomUIData.moveSpdStatUI.StatUpdate(player.mc.TotalMoveSpeed.ToString()); };
            player.OnChangedTotalCriticalRate += () => { bottomUIData.criticalRateStatUI.StatUpdate(player.TotalCriticalRate.ToString()); };
            player.OnChangedTotalCDR += () => { bottomUIData.cdrStatUI.StatUpdate(player.TotalCDR.ToString()); };
        }
        private void ReturnToTitle()
        {
            SceneManager.LoadScene(0);
            //Moru.SceneManager.WSceneManager.instance.TestSceneLoader(0);
        }
        private void ReturnToLobby()
        {
            // �κ�� �� �� �ֵ��� ������ ó��?
            SceneManager.LoadScene(0);
        }
        private void GoToNextStage()
        {
            // ���� ���������� ���� ���� �ҷ��� �� �� ��ε�?
            SceneManager.LoadScene(0);
        }
        private IEnumerator WaveStart()
        {
            bWaveDone = false;

            foreach (var timeline in stage.Waves[WaveIndex - 1].Timeline)
            {
                spawnPosRecord.Clear();

                if (timeline.spawnTime > 0.0f)
                    yield return CachedWaitForSeconds.Get(timeline.spawnTime);

                foreach (var monster in timeline.spawnMonsters)
                {
                    bool isLeftSide;
                    int zLaneNum;
                    bool bReset;

                    Vector3 pos = Vector3.zero;

                    do
                    {
                        if (monster.isRandomSide)
                            isLeftSide = Random.Range(0, 100) % 2 == 0;
                        else
                            isLeftSide = monster.isLeftSide;

                        bReset = false;
                        zLaneNum = monster.isRandomZLane ? Random.Range(0, tm.ZLaneCount) : monster.zLane;
                        pos.x = isLeftSide ? -HalfSizeField_X : HalfSizeField_X;
                        pos.z = tm.GetZLanePosZ(zLaneNum);
                        pos.z += monster.isRandomZOffset ? Random.Range(-monster.zRandomOffsetRange, monster.zRandomOffsetRange) : monster.zOffset;

                        if (spawnPosRecord.Count != 0)
                        {
                            foreach (Vector3 recordedPos in spawnPosRecord)
                            {
                                if ((pos - recordedPos).sqrMagnitude < timeline.OverlapDistanceSqr)
                                {
                                    bReset = true;
                                    break;
                                }
                            }
                        }
                    } while (bReset);

                    spawnPosRecord.Add(pos);
                    SpawnMonster(monster.monsterIndex, monster.monsterLevel, pos, zLaneNum, monster.isEpicMonster);
                }
            }

            bWaveDone = true;
        }

        public void ResetScene()
        {
            Moru.SceneManager.WSceneManager.instance.TestSceneLoader(2);
        }
        public void ChangeSwitchIcon(bool bToggleTS)
        {
            // Ÿ��
            if (bToggleTS)
            {
                bottomUIData.switch_TS_icon.sprite = bottomUIData.switch_T_img;
                for (int i = 0; i < 6; ++i)
                {
                    if (player.towerSet[i].ti == TOWER_INDEX.MAX)
                    {
                        bottomUIData.icons_TS[i].sprite = bottomUIData.lockIcon;
                        bottomUIData.tmp_Cost[i].text = null;
                    }
                    else
                    {
                        bottomUIData.icons_TS[i].sprite = UnitSO.TowerData[player.towerSet[i].ti].icon;
                        bottomUIData.tmp_Cost[i].text = UnitSO.TowerData[player.towerSet[i].ti].spawnCost.ToString();
                    }
                }
            }
            // ��ų
            else
            {
                bottomUIData.switch_TS_icon.sprite = bottomUIData.switch_S_img;
                for (int i = 0; i < 6; ++i)
                {
                    if (player.Skills[i] == null)
                    {
                        bottomUIData.icons_TS[i].sprite = bottomUIData.disableIcons_TS[i].sprite = bottomUIData.lockIcon;
                        bottomUIData.tmp_Cost[i].text = null;
                    }
                    else
                    {
                        bottomUIData.icons_TS[i].sprite = bottomUIData.disableIcons_TS[i].sprite = player.Skills[i].Icon;
                        bottomUIData.icons_TS[i].fillAmount = 1.0f;
                        bottomUIData.tmp_Cost[i].text = player.Skills[i].ASkill.ManaCost.ToString();
                    }
                }
            }
        }
        public void SetWarningIcon(bool active, bool isLeft, int zLane)
        {
            if (isLeft)
                warningLeftIcons[zLane].SetActive(active);
            else
                warningRightIcons[zLane].SetActive(active);
        }
        public void NotifyInfoText(string notify)
        {
            tmp_InfoText.text = notify;
            Color c = tmp_InfoText.color;
            c.a = 2.0f;
            tmp_InfoText.color = c;
        }
        private IEnumerator InfoTextAnimation()
        {
            while (true)
            {
                if (tmp_InfoText.color.a == 0.0f)
                    yield return CachedWaitForSeconds.Get(0.1f);
                else
                {
                    Color c = tmp_InfoText.color;
                    c.a = Mathf.Max(c.a - 0.05f, 0.0f);
                    tmp_InfoText.color = c;
                    yield return CachedWaitForSeconds.Get(0.05f);
                }
            }
        }
        private IEnumerator CameraTempFXUpdate()
        {
            while (true)
            {
                if (curTempRatio == aimTempRatio)
                    yield return CachedWaitForSeconds.Get(0.1f);
                else
                {
                    tempRatioTime += 0.025f;
                    curTempRatio = Mathf.Lerp(curTempRatio, aimTempRatio, tempRatioTime);
                    if (curTempRatio <= 0)
                    {
                        camColdDistortionFXMtrl.SetFloat("_DistortAmount", -curTempRatio);
                        camColdFXMtrl.SetFloat("_Ice", -curTempRatio);
                    }

                    if (curTempRatio >= 0)
                        camWarmFXMtrl.SetFloat("_Radial", curTempRatio);

                    yield return CachedWaitForSeconds.Get(0.025f);
                }
            }
        }
        #endregion

        [System.Serializable]
        public struct TopUIData
        {
            [LabelText("�µ� TMP")]
            public TMP_Text tmp_Temperature;
            [LabelText("�ð� TMP")]
            public TMP_Text tmp_Time;
            [LabelText("�������� TMP")]
            public TMP_Text tmp_Stage;
            [LabelText("���̺� TMP")]
            public TMP_Text tmp_Wave;
            [LabelText("�ǹ� TMP")]
            public TMP_Text tmp_Silver;
            public Button switchTSBtn;
            public Button menuBtn;
        }
        [System.Serializable]
        public struct WaveInfoUIData
        {
            public WaveInfoComponent[] icons;
        }
        [System.Serializable]
        public struct BottomUIData
        {
            [LabelText("ĳ���� ������ UI")]
            public Image charImg;
            [LabelText("Ÿ��/��ų ���� ������")]
            public Image switch_TS_icon;
            [LabelText("Ÿ�� ������")]
            public Sprite switch_T_img;
            [LabelText("��ų ������")]
            public Sprite switch_S_img;
            [LabelText("��� ������")]
            public Sprite lockIcon;
            [LabelText("Ÿ��/��ų �����ܵ�")]
            public Image[] icons_TS;
            [LabelText("Ÿ��/��ų ��Ȱ��ȭ �����ܵ�")]
            public Image[] disableIcons_TS;
            [LabelText("Ÿ��/��ų ������ ��� TMP")]
            public TMP_Text[] tmp_Cost;
            [LabelText("HP��")]
            public UIBarComponent hpStatUI;
            [LabelText("MP��")]
            public UIBarComponent mpStatUI;
            [LabelText("����ġ��")]
            public UIBarComponent expStatUI;
            [LabelText("���� ����")]
            public UIStatComponent levelStatUI;
            [LabelText("�������ݷ� ����")]
            public UIStatComponent physxPowerStatUI;
            [LabelText("�ֹ��� ����")]
            public UIStatComponent magicPowerStatUI;
            [LabelText("���� ����")]
            public UIStatComponent armorStatUI;
            [LabelText("��������� ����")]
            public UIStatComponent armorPntStatUI;
            [LabelText("���ݼӵ� ����")]
            public UIStatComponent atkSpdStatUI;
            [LabelText("�̵��ӵ� ����")]
            public UIStatComponent moveSpdStatUI;
            [LabelText("ġ��Ÿ�� ����")]
            public UIStatComponent criticalRateStatUI;
            [LabelText("��Ÿ�Ӱ��� ����")]
            public UIStatComponent cdrStatUI;
        }
        [System.Serializable]
        public struct SelectedInfoUIData
        {
            public GameObject panel_SelectedInfo;
            public TMP_Text tmp_SelectedInfoName;
            public TMP_Text tmp_SelectedInfoLv;
            public Image img_SelectedInfoIcon;
            public TMP_Text tmp_SelectedInfoTitle;
            public TMP_Text tmp_SelectedInfoDesc;
        }
        [System.Serializable]
        public struct SoulCardUIData
        {
            public GameObject panel_SoulCardUI;
            public RectTransform panel_SoulCard;
            public TMP_Text tmp_CountDown;
            public Button btn_Reroll;
            public TMP_Text tmp_Reroll;
            public Button btn_Confirm;
            public TMP_Text tmp_Confirm;
        }
        [System.Serializable]
        public struct ClearUIData
        {
            public TMP_Text tmp_GameDetailInfo;
            public Button returnTitleBtn;
            public Button returnLobbyBtn;
            public Button nextStageBtn;
        }
        [System.Serializable]
        public struct LoseUIData
        {
            public TMP_Text tmp_GameDetailInfo;
            public Button returnTitleBtn;
            public Button returnLobbyBtn;
        }
    }
}