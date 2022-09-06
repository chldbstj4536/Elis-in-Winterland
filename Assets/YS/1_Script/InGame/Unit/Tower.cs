using UnityEngine;
using Sirenix.OdinInspector;
using TypeReferences;

namespace YS
{
    public partial class Tower : Unit
    {
        public delegate void OnTowerTriggerEvent(Tower caller);
        #region Field
        private TOWER_INDEX index;

        protected TileComponent tile;

        public event OnTowerTriggerEvent OnTowerDestroyEvent;
        public event OnTowerTriggerEvent OnTowerSpawnBeforeEvent;
        public event OnTowerTriggerEvent OnTowerSpawnAfterEvent;
        #endregion

        #region Properties
        public TOWER_INDEX Index => index;
        public override UNIT_SIZE Size => UnitSO.TowerData[index].size;
        public override UNIT_ATTRIBUTE Attribute => UnitSO.TowerData[index].attribute;
        public override string Name => UnitSO.TowerData[index].name;
        public override string Description => UnitSO.TowerData[index].desc;
        public override Sprite Icon => UnitSO.TowerData[index].icon;
        public override AnimationTrackSet[] AnimationSet_Idle => UnitSO.TowerData[index].animSetIdle;
        public override AnimationTrackSet[] AnimationSet_Move => UnitSO.TowerData[index].animSetMove;
        public override AnimationTrackSet[] AnimationSet_Die => UnitSO.TowerData[index].animSetDie;
        public AnimationTrackSet[] AnimationSet_Spawn => UnitSO.TowerData[index].animSetSpawn;
        public TOWER_CATEGORY Category => UnitSO.TowerData[index].category;
        public int Cost => UnitSO.TowerData[index].spawnCost;
        #endregion

        #region Unity Methods
        protected override void OnDisable()
        {
            OnTowerDestroyEvent = null;

            base.OnDisable();
        }
        #endregion

        #region Methods
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            type = UNIT_TYPE.TOWER;
        }
        public void SpawnTower(TileComponent tile)
        {
            this.tile = tile;
            transform.parent = tile.transform;
            transform.localPosition = Vector3.zero;
            Flip(tile.transform.position.x < 0.0f);
            gameObject.SetActive(true);
        }
        protected override void OnDie(Unit killer)
        {
            OnTowerDestroyEvent?.Invoke(this);

            base.OnDie(killer);
        }
        #endregion

        [System.Serializable]
        public class TowerData : UnitData
        {
            [HideInInspector]
            public TOWER_INDEX index;
            [FoldoutGroup("Tower", true, 0)]
            [LabelText("Ÿ�� AI"), Tooltip("Ÿ���� �۵������ ���մϴ�."), Inherits(typeof(BaseTowerAI), Grouping = Grouping.None, IncludeBaseType = true, ShowNoneElement = false, ShortName = true)]
            public TypeReference towerAI;
            [FoldoutGroup("Tower")]
            [LabelText("Ÿ�� ī�װ�"), Tooltip("Ÿ���� ī�װ��� ���մϴ�.")]
            public TOWER_CATEGORY category;
            [FoldoutGroup("Tower")]
            [LabelText("Ÿ�� �ҿ�ī��"), Tooltip("�ش� Ÿ���� ������ �ִ� �ҿ�ī���Դϴ�.")]
            public SOULCARD_INDEX[] soulcards;
            [FoldoutGroup("Tower")]
            [LabelText("Ÿ�� ���� ���"), Tooltip("Ÿ���� ������ ���� ����� ���մϴ�.")]
            public int spawnCost;

            [BoxGroup("������/���� �ִϸ��̼�", true), HideLabel, Tooltip("���� �� ����� �ִϸ��̼� ������ �����մϴ�.")]
            public AnimationTrackSet[] animSetSpawn;

            public virtual Unit Instantiate(int lv, Vector3 spawnPos)
            {
                Tower tower = base.Instantiate(lv, spawnPos, true) as Tower;

                tower.index = index;

                tower.fsm = ObjectPool.GetObject(towerAI) as BaseTowerAI;
                (tower.fsm as BaseTowerAI).Initialize(tower);
                tower.fsm.Start();

                return tower;
            }
        }
    }
}