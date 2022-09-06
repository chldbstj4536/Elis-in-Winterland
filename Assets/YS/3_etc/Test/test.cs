using UnityEngine;
using Spine.Unity;

namespace YS
{
    public class test : MonoBehaviour
    {
        public AnimationTrackSet[] anim1;
        public AnimationTrackSet[] anim2;
        public AnimationTrackSet[] anim3;

        private AnimationPlayer player;

        [SpineSlot]
        public string slotName;
        [SpineAttachment(slotField: "slotName")]
        public string attachmentName;

        private SkeletonAnimation spine;
        private Spine.AnimationState state;
        private Spine.TrackEntry curTE;

        private void Start()
        {
            spine = GetComponent<SkeletonAnimation>();
            state = spine.AnimationState;
            player = new AnimationPlayer(state);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                player.SetAnimationSets(anim1, true);
                player.Complete += () => { Debug.Log("Anim1 Complete!"); };
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                player.SetAnimationSets(anim2, true);
                player.Complete += () => { Debug.Log("Anim2 Complete!"); };
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.SetAnimationSets(anim3, true);
                player.Complete += () =>
                {
                    player.SetAnimationSets(anim1, true);
                    player.SetAnimationSets(anim2, true);
                };
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                player.ExitLoopAllTrack();
            }

            //Spine.PointAttachment attachment = spine.Skeleton.GetAttachment(slotName, attachmentName) as Spine.PointAttachment;
            //followTr.position = attachment.GetWorldPosition(spine.skeleton.FindSlot(slotName), spine.transform);
        }
    }
}