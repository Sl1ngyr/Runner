using UnityEngine;

namespace Player.AnimationStates
{
    public class AnimationBehaviorSlide : AnimationBehavior
    {
        public AnimationBehaviorSlide(Animator animator) : base(animator)
        {
        }
        
        public override void Enter()
        {
            Animator.SetBool("IsSliding", true);
        }

        public override void Exit()
        {
            Animator.SetBool("IsSliding", false);
        }
    }
}