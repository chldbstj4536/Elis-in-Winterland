using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace Moru
{
    public partial class UserSaveData
    {
        #region #5. Ÿ�� ����

        [SerializeField] private TOWER_INDEX current_TOWER_UNIT_INDEX;
        public TOWER_INDEX Current_TOWER_UNIT_INDEX { get => current_TOWER_UNIT_INDEX; set => current_TOWER_UNIT_INDEX = value; }
        public Tower_DB GetCurrentTowerDB => towers_Info[Current_TOWER_UNIT_INDEX];


        [Tooltip("Ÿ�� �����͹����Դϴ�.")]
        [SerializeField, ShowInInspector]
        public Dictionary<TOWER_INDEX, Tower_DB> towers_Info;


        /// <summary>
        /// �÷��̾ �ִ�� ���� �� �ִ� Ÿ�� �������� �����Դϴ�.
        /// </summary>
        [SerializeField] public const int MaxPresetCount = 3;
        /// <summary>
        /// ���� Ÿ�� ������ �ѹ��Դϴ�.
        /// </summary>
        [SerializeField] private int current_preset_Adress = 0;
        /// <summary>
        /// ���� Ÿ�� �������� �ּ� ������Ƽ�Դϴ�.
        /// </summary>
        public int Current_Preset_Adress { get => current_preset_Adress; set => current_preset_Adress = value; }

        [SerializeField, ShowInInspector, LabelText("�����º� ������ ���巹��")] 
        Dictionary<int, Dictionary<Define.Slot, Tower_DB>> tower_Preset = new Dictionary<int, Dictionary<Define.Slot, Tower_DB>>();
        public Dictionary<int, Dictionary<Define.Slot, Tower_DB>> Tower_Preset => tower_Preset;


        [Tooltip("���� �����õ� Ÿ�� ������ �׸��Դϴ�.")]
        [SerializeField, ShowInInspector] private Dictionary<Define.Slot, Tower_DB> currentSloting_Tower_DB;
        public Dictionary<Define.Slot, Tower_DB> CurrentSloting_Tower_DB => currentSloting_Tower_DB;


        #endregion

        [System.Serializable]
        public partial class Tower_DB
        {
            [SerializeField, LabelText("���� ����"), ReadOnly]          private bool isHaving;
                                                                       public bool IsHaving => isHaving;

            [SerializeField] private int level = 0;
            public int Level { get => level; set => level = value; }

            [SerializeField, LabelText("���� �ε���"), ReadOnly]          private YS.TOWER_INDEX index;
                                                                        public YS.TOWER_INDEX Index { get => index; set => index = value; }

            [SerializeField] private YS.Tower.TowerData data;
            public YS.Tower.TowerData Data
            {
                get
                {
                    if (data == null)
                    { data = UserSaveData.TowerSO[index]; }
                    return data;
                }
            }

            [SerializeField, LabelText("���� ���̷��� ������")] private SkeletonDataAsset skeletonDataAsset;
            public SkeletonDataAsset skeleton
            {
                get
                {
                    if (skeletonDataAsset == null)
                    { skeletonDataAsset = UserSaveData.SkeletonSO.tower_Skeleton[(int)index]; }
                    return skeletonDataAsset;
                }
            }

            private int goldCost;
            public int GoldCost => goldCost;

            public Tower_DB(YS.TOWER_INDEX _index ,int _level = 0, bool _isHaving = true)
            {
                index = _index;
                this.level = _level;
                isHaving = _isHaving;
            }
        }

        /// <summary>
        /// Ÿ�������� �ʱ�ȭ �� �������� �����Ͽ� ���������� �ʱ�ȭ�մϴ�.
        /// </summary>
        public void Init_Tower()
        {
            //Ÿ�� ���ҽ��� �ʱ�ȭ
            towers_Info = new Dictionary<TOWER_INDEX, Tower_DB>();
            for (int i = 0; i < (int)TOWER_INDEX.MAX; i++)
            {
                var TOWER_ENUM = (TOWER_INDEX)i;
                towers_Info.Add(TOWER_ENUM, new Tower_DB(TOWER_ENUM));
            }

            //Ÿ�� ������ ����
            for(int i =0; i < MaxPresetCount; i++)
            {
                var preset_value = new Dictionary<Define.Slot, Tower_DB>();
                tower_Preset.Add(i, preset_value);
                //�ش� �����º� Ÿ�� �⺻�� �Ҵ�
                for(int j = 0; j < (int)Define.Slot.MAX; j++)
                {
                    if(towers_Info.TryGetValue((TOWER_INDEX)j, out Tower_DB value))
                    {
                        preset_value.Add((Define.Slot)j, value);
                    }
                    else
                    {
                        preset_value.Add((Define.Slot)j, null);
                    }
                }
            }
        }


        /// <summary>
        /// Ÿ���� �Ҵ��� ��� ������Ʈ�մϴ�.
        /// </summary>
        /// <param name="slot_Index"></param>
        /// <param name="tower_DB"></param>
        public void UpdateTower(Define.Slot slot_Index, Tower_DB tower_DB)
        {
            //�ߺ�Ÿ���� Ÿ�� �ڸ��ٲٱ⿡ ���� ó��
            var Preset_TowerSlot = Tower_Preset[current_preset_Adress];

            //�ߺ�
            if(Preset_TowerSlot.ContainsValue(tower_DB))
            {
                var PastSlotValue = Preset_TowerSlot[slot_Index];
                Define.Slot PastValue_Slot = Define.Slot.A;
                for (int i = 0; i < (int)Define.Slot.MAX; i++)
                {
                    if (Preset_TowerSlot[(Define.Slot)i] == tower_DB)
                    {
                        PastValue_Slot = (Define.Slot)i;
                        break;
                    }
                }
                Preset_TowerSlot[PastValue_Slot] = PastSlotValue;
            }
            Preset_TowerSlot[slot_Index] = tower_DB;
            TitleUI_Events.Del_Tower_Click?.Invoke();
        }
    }
}