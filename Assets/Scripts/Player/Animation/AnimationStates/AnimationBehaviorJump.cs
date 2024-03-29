using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public class AnimationBehaviorJump : AnimationBehavior
    {
        public AnimationBehaviorJump(Animator animator) : base(animator)
        {
        }
        
        public override void Enter()
        {
            Animator.SetBool("IsJumping", true);
        }
        
        public override void Exit()
        {
            Animator.SetBool("IsJumping", false);
        }
    }
}