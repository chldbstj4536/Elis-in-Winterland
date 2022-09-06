using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class RageOfFrostGen : CircleGen
    {
        public Unit.Skill.BaseSkill caster;

        [SerializeField]
        private GameObject targetRange;
        [SerializeField]
        private GameObject targetRangeEnd;
        [SerializeField]
        private float projectileSpeed;

        private readonly SphereHitcast hitbox = new SphereHitcast();

        public delegate void OnHit(Unit unit);
        private OnHit OnHitEvent;

        public void Initialize(Unit.Skill.BaseSkill caster, float rangeSize, float emitDuration, uint totalCount, OnHit onHit)
        {
            bUseTotalCount = true;
            duration = emitDuration + 1.0f;
            this.emitDuration = emitDuration;
            this.totalCount = totalCount;

            this.caster = caster;
            targetRange.transform.localScale = new Vector3(rangeSize, 1.0f, rangeSize);
            targetRangeEnd.transform.localScale = new Vector3(rangeSize, 1.0f, rangeSize);

            OnHitEvent = onHit;

            radius = rangeSize;
        }

        protected override void OnEmitGameObject(GameObject obj)
        {
            base.OnEmitGameObject(obj);

            var p = obj.GetComponent<LinearProjectile>();
            Vector3 sp, ep;
            sp = ep = p.transform.position;
            p.transform.localScale = Vector3.one;
            ep.y = 0.0f;

            p.SetProjectile(caster, sp, ep, 0, 0);
            p.Speed = projectileSpeed;
            p.OnArrive += OnArrive;
        }
        protected override void OnEmissionEnd()
        {
            base.OnEmissionEnd();

            targetRangeEnd.SetActive(true);
            targetRange.SetActive(false);
        }

        protected override void OnLifeTimeEnd()
        {
            base.OnLifeTimeEnd();

            targetRange.SetActive(true);
            targetRangeEnd.SetActive(false);
        }

        private void OnArrive(Vector3 arrivePos)
        {
            hitbox.offset = Vector3.zero;
            hitbox.direction = Vector3.down;
            hitbox.maxDistance = 1.0f;
            hitbox.radius = radius;

            var hits = Utility.SweepUnit(hitbox, transform.position, Quaternion.identity, true, caster.TargetLayerMask, caster.TargetType);

            foreach (var hit in hits)
                OnHitEvent(hit.transform.GetComponent<Unit>());
        }
    }
}