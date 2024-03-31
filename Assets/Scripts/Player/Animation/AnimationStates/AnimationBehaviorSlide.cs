using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public class AnimationBehaviorSlide : AnimationBehavior
    {
        public AnimationBehaviorSlide(Animator animator) : base(animator)
        {
        }
        
        public override void Enter()
        {
            Animator.SetBool(AnimationDescription.PLAYER_SLIDE, true);
        }

        public override void Exit()
        {
            Animator.SetBool(AnimationDescription.PLAYER_SLIDE, false);
        }
    }
}