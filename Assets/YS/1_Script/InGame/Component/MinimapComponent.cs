using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class MinimapComponent : MonoBehaviour
    {
        public float endOfMap_X;
        public float heightMap;
        public Transform cam;
        public GameObject iconPrefab;

        private GameManager gm;
        private RectTransform minimap;
        private Vector2 minimapSize;

        private List<Image> icons = new List<Image>();
        private int beforeCountIcons;

        private void Start()
        {
            gm = GameManager.Instance;
            minimap = GetComponent<RectTransform>();
            minimapSize.x = minimap.rect.width / 2;
            minimapSize.y = minimap.rect.height;
            beforeCountIcons = 0;
        }

        private void Update()
        {
            // 미니맵 위치 조정
            Vector3 newMinimapPos = minimap.anchoredPosition;
            newMinimapPos.x = -cam.position.x / endOfMap_X * minimapSize.x;
            minimap.anchoredPosition = newMinimapPos;

            var unitList = gm.GetAllUnits();

            InitializeIcons(unitList.Count);

            for (int i = 0; i < unitList.Count; ++i)
            {
                Vector3 unitPosInMinimap = unitList[i].transform.position;
                unitPosInMinimap.x = unitPosInMinimap.x / endOfMap_X * minimapSize.x;
                unitPosInMinimap.y = unitPosInMinimap.z / heightMap * minimapSize.y;
                unitPosInMinimap.z = unitPosInMinimap.y;

                icons[i].transform.localScale = new Vector3(unitList[i].IsLookingLeft ? 1.0f : -1.0f, 1.0f, 1.0f);
                icons[i].sprite = unitList[i].Icon;
                icons[i].rectTransform.anchoredPosition = unitPosInMinimap;
            }
        }

        private void InitializeIcons(int curCountIcons)
        {
            if (icons.Count < curCountIcons)
                for (int i = icons.Count; i < curCountIcons; ++i)
                    icons.Add(Instantiate(iconPrefab, transform).GetComponent<Image>());

            if (beforeCountIcons > curCountIcons)
                for (int i = curCountIcons; i < beforeCountIcons; ++i)
                    icons[i].gameObject.SetActive(false);
            else
                for (int i = beforeCountIcons; i < curCountIcons; ++i)
                    icons[i].gameObject.SetActive(true);

            beforeCountIcons = curCountIcons;
        }
    }
}