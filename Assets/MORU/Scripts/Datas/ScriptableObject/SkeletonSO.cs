using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YS;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace Moru
{
    [CreateAssetMenu(fileName = "SkeletonSO", menuName = "Moru/SkeletonSO")]
    public class SkeletonSO : ScriptableObject
    {
        [ShowInInspector]
        public List<SkeletonDataAsset> character_Skeleton;

        [ShowInInspector]
        public List<SkeletonDataAsset> tower_Skeleton;

        [ShowInInspector]
        public List<SkeletonDataAsset> montser_Skeleton;
    }
}
