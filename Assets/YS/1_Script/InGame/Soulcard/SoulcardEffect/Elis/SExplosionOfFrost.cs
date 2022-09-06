using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SExplosionOfFrost : SoulCardEffect
            {
                private readonly float areaDuration;
                private readonly float areaTickCycle;
                private readonly int areaDamage;
                private readonly PoolingComponent areaFXPrefab;
                private readonly IHitBox areaHitbox;
                private readonly AExplosionOfFrost baseSkill;

                public SExplosionOfFrost(SExplosionOfFrostData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    areaDuration = data.areaDuration;
                    areaTickCycle = data.areaTickCycle;
                    areaDamage = data.areaDamage;
                    areaFXPrefab = data.areaFXPrefab;
                    areaHitbox = data.areaHitbox.Instantiate();

                    foreach (var skill in skillOwner.Skills)
                    {
                        if (skill.ASkill is AExplosionOfFrost)
                        {
                            baseSkill = skill.ASkill as AExplosionOfFrost;
                            break;
                        }
                    }
                    baseSkill.OnExplosionEvent += (Vector3 explosionPos) =>
                    {
                        caster.StartCoroutine(TickAttack(explosionPos));
                    };
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override int GetTotalDamage(Unit victim)
                {
                    return areaDamage;
                }
                private IEnumerator TickAttack(Vector3 origin)
                {
                    float curDuration = areaDuration;
                    var fx = PrefabPool.GetObject(areaFXPrefab);
                    fx.transform.position = origin;
                    fx.transform.localScale = new Vector3(3.0f, 1.0f, 3.0f);

                    while (curDuration > 0.0f)
                    {
                        var hits = Utility.SweepUnit(areaHitbox, origin, Quaternion.identity, true, baseSkill.TargetLayerMask, baseSkill.TargetType);

                        foreach (var hit in hits)
                            Attack(hit.transform.GetComponent<Unit>(), DAMAGE_TYPE.NORMAL, hit.transform.position);

                        yield return CachedWaitForSeconds.Get(areaTickCycle);
                        curDuration -= areaTickCycle;
                    }

                    fx.GetComponent<PoolingComponent>().Release();
                }
            }
            [System.Serializable]
            private class SExplosionOfFrostData : SoulCardEffectData
            {
                [BoxGroup("������ ���� �ҿ�ī��", true, true)]
                [LabelText("���� ���ӽð�")]
                public float areaDuration;
                [BoxGroup("������ ���� �ҿ�ī��")]
                [LabelText("���� ƽ �ֱ�")]
                public float areaTickCycle;
                [BoxGroup("������ ���� �ҿ�ī��")]
                [LabelText("���� ƽ�� ���ط�")]
                public int areaDamage;
                [BoxGroup("������ ���� �ҿ�ī��")]
                [LabelText("���� ����Ʈ ������")]
                public PoolingComponent areaFXPrefab;
                [BoxGroup("������ ���� �ҿ�ī��/���� ��Ʈ�ڽ�", true, true), SerializeReference]
                [HideLabel]
                public IHitBox areaHitbox;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SExplosionOfFrost(this, owner as PlayableUnit);
                }
            }
        }
    }
}