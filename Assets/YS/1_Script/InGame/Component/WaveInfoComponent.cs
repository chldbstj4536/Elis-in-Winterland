using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public class WaveInfoComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private TMP_Text count;

        [SerializeField] private GameObject panel_MonsterInfo;
        [SerializeField] private Image img_monsterIcon;
        [SerializeField] private TMP_Text tmp_MonsterName;
        [SerializeField] private TMP_Text tmp_MonsterDesc;

        private MONSTER_INDEX index;

        private bool isAllocated = false;
        private GameManager gm;

        private void Awake()
        {
            gm = GameManager.Instance;
        }
        public void OnPointerEnter(PointerEventData data)
        {
            if (!isAllocated) return;

            img_monsterIcon.sprite = UnitSO.MonsterData[index].icon;
            tmp_MonsterName.text = UnitSO.MonsterData[index].name;
            tmp_MonsterDesc.text = UnitSO.MonsterData[index].desc;
            panel_MonsterInfo.SetActive(true);
        }
        public void OnPointerExit(PointerEventData data)
        {
            if (!isAllocated) return;

            panel_MonsterInfo.SetActive(false);
        }
        public void Initialize(MONSTER_INDEX index, uint count)
        {
            this.index = index;
            icon.sprite = UnitSO.MonsterData[index].icon;
            this.count.text = count.ToString();
            isAllocated = true;
        }
        public void Remove()
        {
            icon.sprite = defaultIcon;
            isAllocated = false;
        }
        public void ChangeCount(uint count)
        {
            this.count.text = count.ToString();
        }
    }
}