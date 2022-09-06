using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class TileComponent : MonoBehaviour
    {
        private Tower tower;

        public Tower Tower => tower;

        public Tower BuildTower(TOWER_INDEX ti, int lv)
        {
            gameObject.layer = 0;

            tower = UnitSO.TowerData[ti].Instantiate(lv, Vector3.zero) as Tower;
            tower.OnTowerDestroyEvent += OnTowerDestroy;
            tower.SpawnTower(this);

            return tower;
        }
        public void OnTowerDestroy(Tower tower)
        {
            gameObject.layer = (int)LAYER_NUMBER.TILE;

            this.tower = null;
        }
    }
}