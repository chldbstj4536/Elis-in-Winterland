using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SDefenceTower2 : SoulCardEffect
            {
                private readonly SphereHitcast hitbox;
                private readonly PSDT2TauntData pTauntData;

                public SDefenceTower2(SDefenceTower2Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    hitbox = data.hitbox.Instantiate() as SphereHitcast;
                    pTauntData = data.pTauntData;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    caster.gm.OnSpawnTowerEvent += (Tower tower) => { tower.OnTowerSpawnAfterEvent += Gm_OnSpawnTowerEvent; };
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
                private void Gm_OnSpawnTowerEvent(Tower tower)
                {
                    if (tower.Category != TOWER_CATEGORY.DEFENSE)
                        return;

                    var hits = Utility.SweepUnit(hitbox, tower.transform.position, Quaternion.identity, true, (int)LAYER_MASK.TEAM2, UNIT_FLAG.MONSTER);

                    foreach (var hit in hits)
                        (pTauntData.Instantiate(tower) as PSDT2Taunt).AddPassiveSkillToUnit(hit.transform.GetComponent<Monster>());
                }
            }
            [System.Serializable]
            private class SDefenceTower2Data : SoulCardEffectData
            {
                [BoxGroup("방어 타워 (2)", true, true)]
                [BoxGroup("방어 타워 (2)/도발 범위", true, true)]
                [HideLabel]
                public SphereHitcast hitbox = new SphereHitcast();
                [BoxGroup("방어 타워 (2)/도발")]
                [HideLabel]
                public PSDT2TauntData pTauntData = new PSDT2TauntData();

#if UNITY_EDITOR
                public SDefenceTower2Data()
                {
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = false;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
                }

                public override void DrawGizmos(Vector3 origin, Quaternion rot)
                {
                    hitbox.SetRange(range);
                    hitbox.DrawGizmos(origin, rot);
                }
#endif
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SDefenceTower2(this, owner as PlayableUnit);
                }
            }
            private class PSDT2Taunt : PTaunt
            {
                public PSDT2Taunt(PSDT2TauntData data, Unit skillOwner) : base(data, skillOwner)
                {
                    
                }
                protected override int GetTotalDamage(Unit victim) { throw new System.NotImplementedException(); }
            }
            [System.Serializable]
            private class PSDT2TauntData : PTauntData
            {
                public PSDT2TauntData() : base() { }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new PSDT2Taunt(this, owner);
                }
            }
        }
    }
}