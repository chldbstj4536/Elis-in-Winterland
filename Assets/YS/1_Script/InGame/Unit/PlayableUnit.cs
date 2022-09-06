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

        // 타일 선택 정보
        private int chosenTowerIndex;
        // 타겟 선택 정보
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

            // 게임매니저에 전투 준비, 전투가 시작될 때 이벤트 호출 함수
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
                    gm.NotifyInfoText("유효하지 않은 스킬입니다.");
                    break;
                case SKILL_ERROR_CODE.COOLTIME:
                    gm.NotifyInfoText("재사용 대기시간이 남아있습니다.");
                    break;
                case SKILL_ERROR_CODE.CANT_CANCEL:
                    gm.NotifyInfoText("스킬을 중단할 수 없습니다.");
                    break;
                case SKILL_ERROR_CODE.NOT_ENOUGH_MANA:
                    gm.NotifyInfoText("마나가 부족합니다.");
                    break;
                case SKILL_ERROR_CODE.IN_CC:
                    gm.NotifyInfoText("상태이상으로 인해 스킬을 시전할 수 없습니다.");
                    break;
                case SKILL_ERROR_CODE.NO_TARGET:
                    gm.NotifyInfoText("대상이 없습니다.");
                    break;
                case SKILL_ERROR_CODE.OUT_OF_RANGE:
                    gm.NotifyInfoText("대상이 범위 밖에 있습니다.");
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
                        // 중복카드라면 추가되지 않기때문에 i값을 하나 감소시켜서 다시 뽑도록 한다
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

                // 덱에 연관 및 삭제 카드 적용
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
        /// 현재 행동을 취소하는 함수
        /// </summary>
        /// <returns>취소했다면 true, 취소할 수 없다면 false</returns>
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
                            gm.NotifyInfoText("대상이 범위 밖에 있습니다.");
                            fsm.ChangeState(fsm.stateIdle);
                            break;
                        case SKILL_ERROR_CODE.NO_TARGET:
                            gm.NotifyInfoText("대상이 존재하지 않습니다.");
                            fsm.ChangeState(fsm.stateIdle);
                            break;
                        case SKILL_ERROR_CODE.NO_ERR:
                            // 즉발 : 타겟, 지속 : 타겟인 경우
                            if (fsm.CurrentStateIndex == FSM.STATE_INDEX.SELECTING_TARGET)
                                fsm.ChangeState(fsm.stateCasting);
                            
                            // 캐스팅 : 타겟 인경우
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
                    gm.NotifyInfoText("실버가 부족합니다.");
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

            [FoldoutGroup("플레이어블 유닛")]
            [LabelText("아이콘 위치 오프셋"), Tooltip("하단 중앙 UI에 나오는 아이콘의 위치를 조정합니다.")]
            public Vector2 iconOffset;
            [FoldoutGroup("플레이어블 유닛")]
            [LabelText("대쉬 스킬")]
            public SkillSO dashSkill;
            [FoldoutGroup("플레이어블 유닛")]
            [LabelText("궁극기 스킬")]
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