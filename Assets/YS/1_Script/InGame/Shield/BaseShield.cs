using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class BaseShield : PoolingComponent
    {
        #region Fields
        private int maxShieldAmount;
        private int curShieldAmount;
        private float maxDuration;
        private float curDuration;
        private int targetLayerMask;
        private UNIT_FLAG targetType;

        [BoxGroup("보호막", true, true), SerializeReference]
        [LabelText("보호막 히트박스")]
        private IHitBox[] hitboxes;

        private Unit caster;

        private HashSet<Unit> lastUnitsInShield = new HashSet<Unit>();
        private HashSet<Unit> unitsInShield = new HashSet<Unit>();

        public delegate void OnDestroy(BaseShield shield);
        public event OnChangedValue OnChangedCurrentShieldAmount;
        public event OnDestroy OnDestroyEvent;
        #endregion

        #region Properties
        public int MaxShieldAmount => maxShieldAmount;
        [ShowInInspector]
        public int CurrentShieldAmount
        {
            get { return curShieldAmount; }
            set
            {
                curShieldAmount = value;
                OnChangedCurrentShieldAmount?.Invoke();
                if (curShieldAmount <= 0)
                    DestroyShield();
            }
        }
        #endregion

        private void OnEnable()
        {
            StartCoroutine(UnitCheck());
        }

        private IEnumerator UnitCheck()
        {
            var wf100ms = CachedWaitForSeconds.Get(0.1f);

            while (maxDuration == 0.0f || curDuration > 0.0f)
            {
                foreach (var unit in unitsInShield)
                    lastUnitsInShield.Add(unit);
                unitsInShield.Clear();

                var hits = Utility.SweepUnit(hitboxes, transform.position, transform.rotation, true, (int)targetLayerMask, targetType);
                foreach (var hit in hits)
                {
                    var unit = hit.transform.GetComponent<Unit>();
                    if (lastUnitsInShield.Contains(unit))
                        lastUnitsInShield.Remove(unit);
                    unitsInShield.Add(unit);
                }

                foreach (var unit in lastUnitsInShield)
                    unit.RemoveShield(this);
                foreach (var unit in unitsInShield)
                    unit.AddShield(this);

                lastUnitsInShield.Clear();

                yield return wf100ms;

                curDuration -= 0.1f;
            }

            DestroyShield();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var hitbox in hitboxes)
                hitbox.DrawGizmos(transform.position, transform.rotation);
        }
#endif
        public void ClearEvent()
        {
            OnDestroyEvent = null;
        }

        public virtual void SetShield(Unit caster, int maxShieldAmount, float maxDuration, int targetLayerMask, UNIT_FLAG unitFlag)
        {
            this.caster = caster;
            curShieldAmount = this.maxShieldAmount = maxShieldAmount;
            curDuration = this.maxDuration = maxDuration;
            this.targetLayerMask = targetLayerMask;
            targetType = unitFlag;

            ClearEvent();
        }
        private void DestroyShield()
        {
            foreach (var unit in unitsInShield)
                unit.RemoveShield(this);

            curShieldAmount = maxShieldAmount;
            curDuration = maxDuration;
            OnDestroyEvent?.Invoke(this);
            Release();
        }
    }
}