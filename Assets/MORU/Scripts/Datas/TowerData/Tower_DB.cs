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
        #region #5. 타워 정보

        [SerializeField] private TOWER_INDEX current_TOWER_UNIT_INDEX;
        public TOWER_INDEX Current_TOWER_UNIT_INDEX { get => current_TOWER_UNIT_INDEX; set => current_TOWER_UNIT_INDEX = value; }
        public Tower_DB GetCurrentTowerDB => towers_Info[Current_TOWER_UNIT_INDEX];


        [Tooltip("타워 데이터묶음입니다.")]
        [SerializeField, ShowInInspector]
        public Dictionary<TOWER_INDEX, Tower_DB> towers_Info;


        /// <summary>
        /// 플레이어가 최대로 가질 수 있는 타워 프리셋의 개수입니다.
        /// </summary>
        [SerializeField] public const int MaxPresetCount = 3;
        /// <summary>
        /// 현재 타워 프리셋 넘버입니다.
        /// </summary>
        [SerializeField] private int current_preset_Adress = 0;
        /// <summary>
        /// 현재 타워 프리셋의 주소 프로퍼티입니다.
        /// </summary>
        public int Current_Preset_Adress { get => current_preset_Adress; set => current_preset_Adress = value; }

        [SerializeField, ShowInInspector, LabelText("프리셋별 슬로팅 에드레스")] 
        Dictionary<int, Dictionary<Define.Slot, Tower_DB>> tower_Preset = new Dictionary<int, Dictionary<Define.Slot, Tower_DB>>();
        public Dictionary<int, Dictionary<Define.Slot, Tower_DB>> Tower_Preset => tower_Preset;


        [Tooltip("현재 슬로팅된 타워 데이터 그릇입니다.")]
        [SerializeField, ShowInInspector] private Dictionary<Define.Slot, Tower_DB> currentSloting_Tower_DB;
        public Dictionary<Define.Slot, Tower_DB> CurrentSloting_Tower_DB => currentSloting_Tower_DB;


        #endregion

        [System.Serializable]
        public partial class Tower_DB
        {
            [SerializeField, LabelText("보유 여부"), ReadOnly]          private bool isHaving;
                                                                       public bool IsHaving => isHaving;

            [SerializeField] private int level = 0;
            public int Level { get => level; set => level = value; }

            [SerializeField, LabelText("유닛 인덱스"), ReadOnly]          private YS.TOWER_INDEX index;
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

            [SerializeField, LabelText("유닛 스켈레톤 데이터")] private SkeletonDataAsset skeletonDataAsset;
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
        /// 타워정보를 초기화 및 프리셋을 생성하여 최초정보를 초기화합니다.
        /// </summary>
        public void Init_Tower()
        {
            //타워 리소스들 초기화
            towers_Info = new Dictionary<TOWER_INDEX, Tower_DB>();
            for (int i = 0; i < (int)TOWER_INDEX.MAX; i++)
            {
                var TOWER_ENUM = (TOWER_INDEX)i;
                towers_Info.Add(TOWER_ENUM, new Tower_DB(TOWER_ENUM));
            }

            //타워 프리셋 생성
            for(int i =0; i < MaxPresetCount; i++)
            {
                var preset_value = new Dictionary<Define.Slot, Tower_DB>();
                tower_Preset.Add(i, preset_value);
                //해당 프리셋별 타워 기본값 할당
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
        /// 타워를 할당할 경우 업데이트합니다.
        /// </summary>
        /// <param name="slot_Index"></param>
        /// <param name="tower_DB"></param>
        public void UpdateTower(Define.Slot slot_Index, Tower_DB tower_DB)
        {
            //중복타워나 타워 자리바꾸기에 대한 처리
            var Preset_TowerSlot = Tower_Preset[current_preset_Adress];

            //중복
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