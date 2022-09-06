using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    /// <summary>
    /// 타일들을 가져와 위치를 정렬합니다.
    /// </summary>
    public class TileManager : SingleToneMono<TileManager>
    {
        #region Field
        [SerializeField, Range(1, 10), OnValueChanged(nameof(GenerateTile))] private int zLaneCount = 5;
        [SerializeField, Range(1, 10), OnValueChanged(nameof(GenerateTile))] private int xLaneCount = 5;
        [SerializeField, OnValueChanged(nameof(Initialize))] private float xOffset = -1.21f;
        [SerializeField, OnValueChanged(nameof(Initialize))] private float zOffset = 0.0f;
        [SerializeField, OnValueChanged(nameof(Initialize))] private float xSpace = 1.26f;
        [SerializeField, OnValueChanged(nameof(Initialize))] private float zSpace = 1.42f;
        [OnValueChanged(nameof(Initialize))] public Vector3 center = new Vector3(0.0f, 0.0f, 0.47f);
        
        [SerializeField] private GameObject tilePrefab;

        private Transform[] tileSets = new Transform[2];
        private TileComponent[,] tiles;
        #endregion

        #region Properties
        public int ZLaneCount => zLaneCount;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        #endregion Unity Methods


        #region Helper Methods
        /// <summary>
        /// 특정 z라인의 z위치 얻기
        /// </summary>
        /// <param name="zLane">z라인 번호 (아래에서부터 0, 1 ... zLaneCount) [0, zLaneCount)</param>
        /// <returns>zLane의 z위치</returns>
        public float GetZLanePosZ(int zLane)
        {
            float zSpaceLevel = zLane - zLaneCount / 2;
            
            if (zLaneCount % 2 == 0)
                zSpaceLevel += 0.5f;
            else
                Mathf.Abs(zSpaceLevel);

            return center.z + zOffset + zSpaceLevel * zSpace;
        }
        /// <summary>
        /// 특정 z라인의 타일들 얻기
        /// </summary>
        /// <param name="zLane">z라인 번호 (아래에서부터 0, 1 ... zLaneCount) [0, zLaneCount)</param>
        /// <returns>zLane의 타일들</returns>
        public List<TileComponent> GetZLaneTiles(int zLane)
        {
            if (zLane > zLaneCount)
                return null;

            List<TileComponent> list = new List<TileComponent>();

            for (int i = 0; i < 2; ++i)
            {
                int index = zLane;
                while (index >= tiles.GetLength(1))
                {
                    list.Add(tiles[i, index]);
                    index += zLaneCount;
                }
            }

            return list;
        }
        /// <summary>
        /// 특정 z라인의 타워들 얻기
        /// </summary>
        /// <param name="zLane">z라인 번호 (아래에서부터 0, 1 ... zLaneCount) [0, zLaneCount)</param>
        /// <returns>zLane의 타일들</returns>
        public List<Tower> GetZLaneTowers(int zLane)
        {
            if (zLane > zLaneCount)
                return null;

            List<Tower> list = new List<Tower>();

            for (int i = 0; i < 2; ++i)
            {
                int index = zLane;
                while (index < tiles.GetLength(1))
                {
                    if (tiles[i, index].Tower != null)
                        list.Add(tiles[i, index].Tower);

                    index += zLaneCount;
                }
            }

            return list;
        }

        private void GenerateTile()
        {
            tileSets[0] = transform.GetChild(0);
            tileSets[1] = transform.GetChild(1);

            int totalTile = xLaneCount * zLaneCount;

            for (int i = 0; i < 2; ++i)
            {
                if (tileSets[i].childCount < totalTile)
                    while (totalTile != tileSets[i].childCount)
                        Instantiate(tilePrefab, tileSets[i]);
                else if (tileSets[i].childCount > totalTile)
                    while (totalTile != tileSets[i].childCount)
                        DestroyImmediate(tileSets[i].GetChild(0).gameObject);
            }
            Initialize();
        }

        /// <summary>
        /// 타일의 위치를 정렬합니다.
        /// </summary>
        [ContextMenu("Tile Position Setting")]
        private void Initialize()
        {
            tileSets[0] = transform.GetChild(0);
            tileSets[1] = transform.GetChild(1);
            tiles = new TileComponent[2, tileSets[0].childCount];

            for (int i = 0; i < 2; ++i)
            {
                for (int index = 0; index < tileSets[i].childCount; ++index)
                {
                    tiles[i, index] = tileSets[i].GetChild(index).GetComponent<TileComponent>();
                    int curTileZLane = index % zLaneCount;
                    int xSpaceLevel = index / zLaneCount;

                    //최종 위치값 지정
                    tiles[i, index].transform.localPosition = new Vector3(center.x + (i == 0 ? xOffset + xSpaceLevel * xSpace : -xOffset + xSpaceLevel * -xSpace), center.y + 0.0001f, GetZLanePosZ(curTileZLane));
                }
            }

        }
        #endregion Help Methods
    }
}