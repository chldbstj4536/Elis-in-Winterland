using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public partial class PlayableUnit : Unit
    {
        #region Field
        private PLAYABLE_UNIT_INDEX index;

        private readonly int[] PLAYER_EXP_TEMPLATE = { 10000, 15000, 20000, 40000, 60000 };
        private int maxExp = 10;
        private int curExp = 5;

        private int skillKeyFlag;

        private GameObject rangeFX;

        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        public EquipTowerInfo[] towerSet = new EquipTowerInfo[6];

        // true = Tower, false = Skill
        private bool bToggleTS;

        private INTERACT_STATE interactState;

        // Ÿ�� ���� ����
        private int chosenTowerIndex;
        // Ÿ�� ���� ����
        private int choosingTargetLayerMask;
        private UNIT_FLAG choosingTargetType;
        
        [ShowInInspector, DisableIf("@true"), BoxGroup("Debug")]
        private Dictionary<SOULCARD_INDEX, SoulCard> soulcards = new Dictionary<SOULCARD_INDEX, SoulCard>();
        public Dictionary<SOULCARD_INDEX, SoulCardData> deck = new Dictionary<SOULCARD_INDEX, SoulCardData>();
        #endregion

        #region Event
        public event Delegate_NoRetVal_NoParam OnInteract;
        public event OnChangedValue OnChangedMaxExp;
        public event OnChangedValue OnChangedCurrentExp;
        #endregion

        #region Properties
        public PLAYABLE_UNIT_INDEX Index => index;
        public override UNIT_ATTRIBUTE Attribute => UnitSO.PlayableUnitData[index].attribute;
        public override UNIT_SIZE Size => UnitSO.PlayableUnitData[index].size;
        public override string Name => UnitSO.PlayableUnitData[index].name;
        public override string Description => UnitSO.PlayableUnitData[index].desc;
        public override Sprite Icon => UnitSO.PlayableUnitData[index].icon;
        public override AnimationTrackSet[] AnimationSet_Idle => UnitSO.PlayableUnitData[index].animSetIdle;
        public override AnimationTrackSet[] AnimationSet_Move => UnitSO.PlayableUnitData[index].animSetMove;
        public override AnimationTrackSet[] AnimationSet_Die => UnitSO.PlayableUnitData[index].animSetDie;
        public Vector2 IconOffset => UnitSO.PlayableUnitData[index].iconOffset;
        public INTERACT_STATE InteractState => interactState;
        public int ChoosingTargetLayerMask => choosingTargetLayerMask;
        public UNIT_FLAG ChoosingTargetType => choosingTargetType;
        public bool IsUsingTowerSwitch => bToggleTS;
        public bool IsUsingSkillSwitch => !bToggleTS;
        public int CurrentExp
        {
            get { return curExp; }
            protected set
            {
                curExp = value;
                OnChangedCurrentExp?.Invoke();
            }
        }
        public int MaxExp
        {
            get { return maxExp; }
            protected set
            {
                maxExp = value;
                OnChangedMaxExp?.Invoke();
            }
        }
        public Dictionary<SOULCARD_INDEX, SoulCard> Soulcards => soulcards;
        #endregion

        #region Methods
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            type = UNIT_TYPE.PLAYER;

            // ���ӸŴ����� ���� �غ�, ������ ���۵� �� �̺�Ʈ ȣ�� �Լ�
            gm.OnBeginReadyForBattle += OnBeginReadyForBattle;
            gm.OnEndReadyForBattle += OnEndReadyForBattle;
            gm.OnBeginBattle += OnBeginBattle;
            gm.OnEndBattle += OnEndBattle;
            rangeFX = transform.GetChild(1).gameObject;

            #region Input Setting
            InputManager.AddKeyEvent(E_CONTROL_TYPE.CLOSE_UI, INPUT_STATE.DOWN, gm.CloseUI);
            InputManager.AddKeyEvent(E_CONTROL_TYPE.PAUSE, INPUT_STATE.DOWN, gm.PauseGame);
            InputManager.AddKeyEvent(E_CONTROL_TYPE.GUIDE, INPUT_STATE.DOWN, () => { });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.CANCLE, INPUT_STATE.DOWN, () => { Cancle(); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.MOVE, INPUT_STATE.HOLD, OnMove);
            InputManager.AddKeyEvent(E_CONTROL_TYPE.CANCLE_N_MOVE, INPUT_STATE.HOLD, OnCancleAndMove);
            InputManager.AddKeyEvent(E_CONTROL_TYPE.INTERACT, INPUT_STATE.DOWN, Interact);
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_1, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.Q); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_2, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.W); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_3, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.E); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_4, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.A); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_5, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.S); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_6, INPUT_STATE.DOWN, () => { OnTowerAndSkill(SKILL_KEY_TYPE.D); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_1, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.Q); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_2, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.W); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_3, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.E); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_4, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.A); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_5, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.S); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.TS_6, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.D); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.DEFAULT_ATTACK, INPUT_STATE.DOWN, () => { CastSkill(defaultAttack); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.DEFAULT_ATTACK, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.DEFAULT_ATTACK); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.DASH, INPUT_STATE.DOWN, () => { CastSkill(dashSkill); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.DASH, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.DASH_SKILL); });
            //InputManager.AddKeyEvent(E_CONTROL_TYPE.U_SKILL, INPUT_STATE.DOWN, () => { CastSkill(ultimateSkill); });
            //InputManager.AddKeyEvent(E_CONTROL_TYPE.U_SKILL, INPUT_STATE.UP, () => { OnAttackInteract(SKILL_KEY_FLAG.ULTIMATE_SKILL); });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.USE_ITEM, INPUT_STATE.DOWN, () => { });
            InputManager.AddKeyEvent(E_CONTROL_TYPE.QUICK_SHIFT, INPUT_STATE.DOWN, OnSwitchTS);
            #endregion
        }
        protected override SKILL_ERROR_CODE CastSkill(Skill newSkill)
        {
            var err = base.CastSkill(newSkill);

            switch (err)
            {
                case SKILL_ERROR_CODE.INVALID_SKILL:
                    gm.NotifyInfoText("��ȿ���� ���� ��ų�Դϴ�.");
                    break;
                case SKILL_ERROR_CODE.COOLTIME:
                    gm.NotifyInfoText("���� ���ð��� �����ֽ��ϴ�.");
                    break;
                case SKILL_ERROR_CODE.CANT_CANCEL:
                    gm.NotifyInfoText("��ų�� �ߴ��� �� �����ϴ�.");
                    break;
                case SKILL_ERROR_CODE.NOT_ENOUGH_MANA:
                    gm.NotifyInfoText("������ �����մϴ�.");
                    break;
                case SKILL_ERROR_CODE.IN_CC:
                    gm.NotifyInfoText("�����̻����� ���� ��ų�� ������ �� �����ϴ�.");
                    break;
                case SKILL_ERROR_CODE.NO_TARGET:
                    gm.NotifyInfoText("����� �����ϴ�.");
                    break;
                case SKILL_ERROR_CODE.OUT_OF_RANGE:
                    gm.NotifyInfoText("����� ���� �ۿ� �ֽ��ϴ�.");
                    break;
            }

            return err;
        }
        public HashSet<SOULCARD_INDEX> DrawSoulCard(int drawCount)
        {
            HashSet<SOULCARD_INDEX> drawedCard = new HashSet<SOULCARD_INDEX>();

            //if (drawCount >= deck.Count)
            //    foreach (var card in deck)
            //        drawedCard.Add(card.Value);
            //return ddrawedCard;

            int maxRange = 0;
            int curIndex = 0;
            int random;

            foreach (var data in deck.Values)
                maxRange += data.weight;

            for (int i = 0; i < drawCount; ++i)
            {
                curIndex = 0;
                random = Random.Range(1, maxRange + 1);
                foreach (var card in deck)
                {
                    curIndex += card.Value.weight;
                    if (random <= curIndex)
                    {
                        // �ߺ�ī���� �߰����� �ʱ⶧���� i���� �ϳ� ���ҽ��Ѽ� �ٽ� �̵��� �Ѵ�
                        if (!drawedCard.Add(card.Key))
                            --i;
                        break;
                    }
                }
            }

            return drawedCard;
        }
        public void AddSoulCard(SOULCARD_INDEX scIndex)
        {
            if (soulcards.ContainsKey(scIndex))
                soulcards[scIndex].AddStack();
            else
            {
                soulcards.Add(scIndex, new SoulCard(scIndex, this));

                // ���� ���� �� ���� ī�� ����
                var newCardData = SoulCard.SoulCardDatas[scIndex];
                foreach (var linkedCard in newCardData.linkedSoulCards)
                    deck.Add(linkedCard, SoulCard.SoulCardDatas[linkedCard]);
                foreach (var removedCard in newCardData.removedSoulCards)
                    deck.Remove(removedCard);
            }

            if (soulcards[scIndex].IsMaxLevel)
                deck.Remove(scIndex);
        }
        public void GetReward(int gainExp, int gainSilver)
        {
            CurrentExp += gainExp;
            if (CurrentExp >= MaxExp)
                LevelUp();

            gm.Silver += gainSilver;
        }
        public void SetRangeFX(bool isActive, Vector3 size)
        {
            rangeFX.SetActive(isActive);
            rangeFX.transform.localScale = size;
        }
        public void SetChoosingTarget(bool isChoosingTarget, int targetLayerMask, UNIT_FLAG targetType)
        {
            if (isChoosingTarget)
                interactState = INTERACT_STATE.CHOOSING_TARGET;
            else
                interactState = INTERACT_STATE.NONE;

            choosingTargetLayerMask = targetLayerMask;
            choosingTargetType = targetType;
        }
        private void OnCancleAndMove()
        {
            if (!Cancle())
                OnMove();
        }
        /// <summary>
        /// ���� �ൿ�� ����ϴ� �Լ�
        /// </summary>
        /// <returns>����ߴٸ� true, ����� �� ���ٸ� false</returns>
        private bool Cancle()
        {
            switch (interactState)
            {
                case INTERACT_STATE.CHOOSING_TILE:
                    interactState = INTERACT_STATE.NONE;
                    break;
                case INTERACT_STATE.CHOOSING_TARGET:
                    fsm.ChangeState(fsm.stateIdle);
                    break;
                case INTERACT_STATE.NONE:
                    if (curSkill == null || !curSkill.CanCancel)
                        return false;
                    break;
            }

            return true;
        }
        private void OnMove()
        {
            if (Utility.HasFlag((int)restrictionFlag, (int)RESTRICTION_FLAG.MOVE))
                return;

            if (Utility.GetMouseRaycast(out RaycastHit hit, (int)LAYER_MASK.FIELD))
            {
                Vector3 dest = hit.point;
                dest.y = 0.0f;
                mc.MoveToDestination(dest);
            }
        }
        private void Interact()
        {
            OnInteract?.Invoke();

            switch (interactState)
            {
                case INTERACT_STATE.CHOOSING_TILE:
                    gm.BuyAndBuildTower(towerSet[chosenTowerIndex]);
                    interactState = INTERACT_STATE.NONE;
                    break;
                case INTERACT_STATE.CHOOSING_TARGET:
                    var code = (curSkill as Skill.TargetingSkill).SetTargetForPlayer();
                    switch (code)
                    {
                        case SKILL_ERROR_CODE.OUT_OF_RANGE:
                            gm.NotifyInfoText("����� ���� �ۿ� �ֽ��ϴ�.");
                            fsm.ChangeState(fsm.stateIdle);
                            break;
                        case SKILL_ERROR_CODE.NO_TARGET:
                            gm.NotifyInfoText("����� �������� �ʽ��ϴ�.");
                            fsm.ChangeState(fsm.stateIdle);
                            break;
                        case SKILL_ERROR_CODE.NO_ERR:
                            // ��� : Ÿ��, ���� : Ÿ���� ���
                            if (fsm.CurrentStateIndex == FSM.STATE_INDEX.SELECTING_TARGET)
                                fsm.ChangeState(fsm.stateCasting);
                            
                            // ĳ���� : Ÿ�� �ΰ��
                            else
                                mainAnimPlayer.ExitLoopAllTrack();
                            break;
                    }
                    break;
                case INTERACT_STATE.NONE:
                    for (int i = 0; i < (int)SKILL_KEY_TYPE.MAX; ++i)
                        OnAttackInteract((SKILL_KEY_FLAG)(1 << i));
                    break;
            }
        }
        private void OnTowerAndSkill(SKILL_KEY_TYPE type)
        {
            if (IsUsingTowerSwitch)
            {
                var ti = towerSet[(int)type].ti;

                if (ti == TOWER_INDEX.MAX)
                    return;

                if (UnitSO.TowerData[ti].spawnCost > gm.Silver)
                {
                    gm.NotifyInfoText("�ǹ��� �����մϴ�.");
                    return;
                }

                chosenTowerIndex = (int)type;
                interactState = INTERACT_STATE.CHOOSING_TILE;
            }
            else if (CastSkill(skills[(int)type]) == SKILL_ERROR_CODE.NO_ERR && gm.Setting.isUsingFastSkill)
                skillKeyFlag |= 1 << (int)type;
        }
        private void OnAttackInteract(SKILL_KEY_FLAG flag)
        {
            if (!gm.Setting.isUsingFastSkill || curSkill == null)   return;

            if (Utility.HasFlag(skillKeyFlag, (int)flag))
            {
                skillKeyFlag ^= (int)flag;

                switch (fsm.CurrentStateIndex)
                {
                    case FSM.STATE_INDEX.SELECTING_TARGET:
                        curSkill.CompleteSelectingTarget();
                        break;
                    case FSM.STATE_INDEX.CASTING:
                        curSkill.CompleteCasting();
                        break;
                    case FSM.STATE_INDEX.ATTACK:
                        curSkill.CompleteTickAttack();
                        break;
                }
            }
        }
        private void OnSwitchTS()
        {
            bToggleTS = !bToggleTS;
            gm.ChangeSwitchIcon(bToggleTS);
        }
        private void OnBeginReadyForBattle()
        {
            // Select SoulCard
        }
        private void OnEndReadyForBattle()
        {

        }
        private void OnBeginBattle()
        {

        }
        private void OnEndBattle()
        {
            // EarnSilver
        }

        private void LevelUp()
        {
            Assert.IsFalse(CurrentExp < MaxExp, "CurExp < maxExp");

            CurrentExp -= MaxExp;
            MaxExp = PLAYER_EXP_TEMPLATE[(Level / 100)];
        }
        #endregion
        
        public enum INTERACT_STATE
        {
            NONE,
            CHOOSING_TILE,
            CHOOSING_TARGET
        }
        public enum SKILL_KEY_FLAG
        {
            Q = 0x01,
            W = 0x02,
            E = 0x04,
            A = 0x08,
            S = 0x10,
            D = 0x20,
            DEFAULT_ATTACK = 0x40,
            ULTIMATE_SKILL = 0x80,
            DASH_SKILL = 0x100
        }
        public enum SKILL_KEY_TYPE
        {
            Q,
            W,
            E,
            A,
            S,
            D,
            DEFAULT_ATTACK,
            ULTIMATE_SKILL,
            DASH_SKILL,
            MAX
        }
        [System.Serializable]
        public class PlayableUnitData : UnitData
        {
            [HideInInspector]
            public PLAYABLE_UNIT_INDEX index;

            [FoldoutGroup("�÷��̾�� ����")]
            [LabelText("������ ��ġ ������"), Tooltip("�ϴ� �߾� UI�� ������ �������� ��ġ�� �����մϴ�.")]
            public Vector2 iconOffset;
            [FoldoutGroup("�÷��̾�� ����")]
            [LabelText("�뽬 ��ų")]
            public SkillSO dashSkill;
            [FoldoutGroup("�÷��̾�� ����")]
            [LabelText("�ñر� ��ų")]
            public SkillSO ultimateSkill;

            public virtual PlayableUnit Instantiate(int lv, string skin, Vector3 spawnPos, EquipSkillInfo[] si, SOULCARD_INDEX[] deck)
            {
                skillDatas = new SkillSO[6];

                PlayableUnit unit = base.Instantiate(lv, spawnPos, true) as PlayableUnit;

                unit.index = index;
                unit.anim.skeleton.SetSkin(skin);
                unit.anim.skeleton.SetSlotsToSetupPose();

                unit.dashSkill = dashSkill.SkillData.Instantiate(unit, dashSkill, 0);
                unit.ultimateSkill = ultimateSkill.SkillData.Instantiate(unit, ultimateSkill, 0);

                for (int i = 0; i < 6; ++i)
                    if (si[i].sso != null)
                        unit.skills[i] = si[i].sso.SkillData.Instantiate(unit, si[i].sso, si[i].lv);

                foreach (var card in deck)
                    unit.deck.Add(card, SoulCard.SoulCardDatas[card]);

                unit.fsm = ObjectPool.GetObject<PlayerFSM>();
                (unit.fsm as PlayerFSM).Initialize(unit);
                unit.fsm.Start();

                return unit;
            }
        }
    }
}