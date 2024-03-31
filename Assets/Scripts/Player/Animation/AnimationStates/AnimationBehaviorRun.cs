using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public class AnimationBehaviorRun : AnimationBehavior
    {
        public AnimationBehaviorRun(Animator animator) : base(animator)
        {
        }
        
        public override void Enter()
        {
            Animator.SetBool(AnimationDescription.PLAYER_RUN, true);
        }
        
        public override void Exit()
        {
            Animator.SetBool(AnimationDescription.PLAYER_RUN, false);
        }
        
    }
}