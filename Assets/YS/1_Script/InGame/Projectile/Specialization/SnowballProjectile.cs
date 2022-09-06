using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class SnowballProjectile : LinearProjectile
    {
        private float   veloDmg;
        private int     maxDmg;
        private float   veloScale;
        private float   maxScale;

        private float curDmg;
        private float curScale;

        protected override void Update()
        {
            base.Update();

            if (curScale != maxScale)
            {
                curScale += veloScale * Time.deltaTime;
                if (curScale >= maxScale) curScale = maxScale;

                transform.localScale = Vector3.one * curScale;

                SphereHitcast hitbox = hitboxesCol[0] as SphereHitcast;
                hitbox.offset.y = hitbox.radius = curScale / 2.0f;
            }
            if ((int)curDmg != maxDmg)
            {
                curDmg += veloDmg * Time.deltaTime;
                if ((int)curDmg >= maxDmg) curDmg = maxDmg;
            }
        }

        public void SetSnowballProjectile(int baseDmg, float baseDmgMagicPowerRate, float veloDmg, int maxDmg, float veloScale, float maxScale, int maxHitCount)
        {
            this.veloDmg = veloDmg;
            this.maxDmg = maxDmg;
            this.veloScale = veloScale;
            this.maxScale = maxScale;

            transform.localScale = Vector3.one;
            curScale = 1.0f;
            curDmg = baseDmg * baseDmgMagicPowerRate;
            TotalDamageCalcOnHit += TotalDamageCalc;
            TotalDamageCalcOnArriveHit += TotalDamageCalc;

            curCountPiercing = countPiercing = maxHitCount;
        }
        private int TotalDamageCalc(Unit victim)
        {
            return (int)curDmg;
        }
    }
}