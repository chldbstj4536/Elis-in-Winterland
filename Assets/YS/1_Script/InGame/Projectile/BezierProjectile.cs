using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class BezierProjectile : Unit.Skill.BaseSkill.Projectile
    {
        #region Field
        [FoldoutGroup("Bezier Projectile", false), Min(0.0001f), SerializeField]
        [LabelText("������ ���� �ð� ���"), Tooltip("������ ����ü�� ���� �ð� �����Դϴ�.\n�����ð� = 1 / ������ ����ü ���� �ð� ���(sec)")]
        private float speed;
        [FoldoutGroup("Bezier Projectile")]
        [LabelText("������ ��ġ ������"), Tooltip("���۰� ������ ������ �������� ��ġ���鿡 ���� �����Դϴ�.")]
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
            [LabelText("���� ��ġ Ÿ��"), Tooltip("������ ��ġ�� �����ٶ� ������ �Ǵ� ��ġ�� ���մϴ�.\nWORLD : ���� ����\nSTART : ������ ����\nEND : ������ ����\nRATIO : �������� ���� ���� ������ ��ġ ����")]
            public PivotType pivotType;
            [LabelText("��ġ"), Tooltip("�������� ��ġ ���")]
            public Vector3 position;
            [ShowIf("pivotType", PivotType.RATIO), Range(0.0f, 1.0f)]
            [LabelText("����"), Tooltip("�������� ���� ������ ����\n0 = ������, 1 = ����")]
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