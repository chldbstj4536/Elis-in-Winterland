using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class BezierProjectile : Unit.Skill.BaseSkill.Projectile
    {
        #region Field
        [FoldoutGroup("Bezier Projectile", false), Min(0.0001f), SerializeField]
        [LabelText("베지어 도착 시간 배속"), Tooltip("베지어 투사체의 도착 시간 정보입니다.\n도착시간 = 1 / 베지어 투사체 도착 시간 배속(sec)")]
        private float speed;
        [FoldoutGroup("Bezier Projectile")]
        [LabelText("베지어 위치 정보들"), Tooltip("시작과 끝점을 제외한 베지어곡선의 위치값들에 대한 정보입니다.")]
        public BezierPosition[] bezierPos = new BezierPosition[0];
        private float speedCoef = 1.0f;
        private Bezier bezier;
        private float t;
        #endregion

        #region Properties
        public override float Speed { get => speed; set => speed = value; }
        public override float SpeedCoef { get => speedCoef; set => speedCoef = value; }
        #endregion

        #region Unity Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            t = 0;
        }
        protected override void Update()
        {
            if (t < 1.0f)
            {
                if (isTargeting)
                {
                    bezier.bezierPos[bezier.bezierPos.Length - 1] = target.position;
                    
                    for (int i = 1; i < bezierPos.Length + 1; ++i)
                    {
                        switch (bezierPos[i - 1].pivotType)
                        {
                            case BezierPosition.PivotType.END:
                                bezier.bezierPos[i] = bezier.bezierPos[bezier.bezierPos.Length - 1] + bezierPos[i - 1].position;
                                break;
                            case BezierPosition.PivotType.RATIO:
                                bezier.bezierPos[i] = bezier.bezierPos[0] + (bezier.bezierPos[bezier.bezierPos.Length - 1] - bezier.bezierPos[0]) * bezierPos[i - 1].ratio + bezierPos[i - 1].position;
                                break;
                            default:
                                continue;
                        }
                        bezier.bezierPos[i] = Quaternion.AngleAxis(45.0f, Vector3.right) * bezier.bezierPos[i];
                    }
                }

                t += Time.deltaTime * TotalSpeed;
                if (t >= 1.0f)
                {
                    transform.position = bezier.GetBezierPosition(1.0f);
                    Arrive();
                }
                else
                    transform.position = bezier.GetBezierPosition(t);

                Flip(beforePos.x - transform.position.x > 0.0f);
            }

            base.Update();
        }
        public override Unit.Skill.BaseSkill.Projectile Instantiate(bool active = true)
        {
            BezierProjectile bp = base.Instantiate(active) as BezierProjectile;

            bp.Speed = Speed;
            bp.SpeedCoef = SpeedCoef;

            return bp;
        }
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Bezier gizmosBezier = bezier;

            if (gizmosBezier.bezierPos == null || gizmosBezier.bezierPos.Length != bezierPos.Length + 2)
                gizmosBezier.bezierPos = new Vector3[bezierPos.Length + 2];

            Vector3 startPos = gizmosBezier.bezierPos[0];

            if (isTargeting)
            {
                if (target != null) return;

                Vector3 targetPos = target.position;

                gizmosBezier.bezierPos[0] = startPos;
                for (int i = 1; i < bezierPos.Length + 1; ++i)
                {
                    switch (bezierPos[i - 1].pivotType)
                    {
                        case BezierPosition.PivotType.WORLD:
                            gizmosBezier.bezierPos[i] = bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.START:
                            gizmosBezier.bezierPos[i] = startPos + bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.END:
                            gizmosBezier.bezierPos[i] = targetPos + bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.RATIO:
                            gizmosBezier.bezierPos[i] = startPos + (targetPos - startPos) * bezierPos[i - 1].ratio + bezierPos[i - 1].position;
                            break;
                    }
                    gizmosBezier.bezierPos[i] = Quaternion.AngleAxis(45.0f, Vector3.right) * gizmosBezier.bezierPos[i];
                }
                gizmosBezier.bezierPos[gizmosBezier.bezierPos.Length - 1] = targetPos;
            }
            else
            {
                gizmosBezier.bezierPos[0] = startPos;
                for (int i = 1; i < bezierPos.Length + 1; ++i)
                {
                    switch (bezierPos[i - 1].pivotType)
                    {
                        case BezierPosition.PivotType.WORLD:
                            gizmosBezier.bezierPos[i] = bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.START:
                            gizmosBezier.bezierPos[i] = startPos + bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.END:
                            gizmosBezier.bezierPos[i] = dest + bezierPos[i - 1].position;
                            break;
                        case BezierPosition.PivotType.RATIO:
                            gizmosBezier.bezierPos[i] = startPos + (dest - startPos) * bezierPos[i - 1].ratio + bezierPos[i - 1].position;
                            break;
                    }
                    gizmosBezier.bezierPos[i] = Quaternion.AngleAxis(45.0f, Vector3.right) * gizmosBezier.bezierPos[i];
                }
                gizmosBezier.bezierPos[gizmosBezier.bezierPos.Length - 1] = dest;
            }
            gizmosBezier.DrawMovingLineGizmo();
        }
#endif
        #endregion

        public override void SetProjectile(Unit.Skill.BaseSkill shooter, Vector3 startPos, Transform target, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
        {
            base.SetProjectile(shooter, startPos, target, layerMaskOnCol, unitFlagOnCol);

            Vector3 targetPos = target.position;

            bezier.bezierPos = new Vector3[bezierPos.Length + 2];
            bezier.bezierPos[0] = startPos;
            for (int i = 1; i < bezierPos.Length + 1; ++i)
            {
                switch (bezierPos[i - 1].pivotType)
                {
                    case BezierPosition.PivotType.WORLD:
                        bezier.bezierPos[i] = bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.START:
                        bezier.bezierPos[i] = startPos + bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.END:
                        bezier.bezierPos[i] = targetPos + bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.RATIO:
                        bezier.bezierPos[i] = startPos + (targetPos - startPos) * bezierPos[i - 1].ratio + bezierPos[i - 1].position;
                        break;
                }
                bezier.bezierPos[i] = Quaternion.AngleAxis(45.0f, Vector3.right) * bezier.bezierPos[i];
            }
            bezier.bezierPos[bezier.bezierPos.Length - 1] = targetPos;
        }

        public override void SetProjectile(Unit.Skill.BaseSkill shooter, Vector3 startPos, Vector3 dest, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
        {
            base.SetProjectile(shooter, startPos, dest, layerMaskOnCol, unitFlagOnCol);

            bezier.bezierPos = new Vector3[bezierPos.Length + 2];
            bezier.bezierPos[0] = startPos;
            for (int i = 1; i < bezierPos.Length + 1; ++i)
            {
                switch (bezierPos[i - 1].pivotType)
                {
                    case BezierPosition.PivotType.WORLD:
                        bezier.bezierPos[i] = bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.START:
                        bezier.bezierPos[i] = startPos + bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.END:
                        bezier.bezierPos[i] = dest + bezierPos[i - 1].position;
                        break;
                    case BezierPosition.PivotType.RATIO:
                        bezier.bezierPos[i] = startPos + (dest - startPos) * bezierPos[i - 1].ratio + bezierPos[i - 1].position;
                        break;
                }
                bezier.bezierPos[i] = Quaternion.AngleAxis(45.0f, Vector3.right) * bezier.bezierPos[i];
            }
            bezier.bezierPos[bezier.bezierPos.Length - 1] = dest;
        }

        [System.Serializable]
        public struct BezierPosition
        {
            [LabelText("기준 위치 타입"), Tooltip("베지어 위치를 정해줄때 기준이 되는 위치를 정합니다.\nWORLD : 월드 기준\nSTART : 시작점 기준\nEND : 목적지 기준\nRATIO : 시작점과 끝점 사이 비율의 위치 기준")]
            public PivotType pivotType;
            [LabelText("위치"), Tooltip("베지어의 위치 요소")]
            public Vector3 position;
            [ShowIf("pivotType", PivotType.RATIO), Range(0.0f, 1.0f)]
            [LabelText("비율"), Tooltip("시작점과 끝점 사이의 비율\n0 = 시작점, 1 = 끝점")]
            public float ratio;

            [System.Serializable]
            public enum PivotType
            {
                WORLD,
                START,
                END,
                RATIO
            }
        }
    }
}