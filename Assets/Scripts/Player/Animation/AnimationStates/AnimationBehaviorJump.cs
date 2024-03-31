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
            Animator.SetBool(AnimationDescription.PLAYER_JUMP, true);
        }
        
        public override void Exit()
        {
            Animator.SetBool(AnimationDescription.PLAYER_JUMP, false);
        }
    }
}