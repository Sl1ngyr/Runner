using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public class AnimationBehaviorIdle : AnimationBehavior
    {
        public AnimationBehaviorIdle(Animator animator) : base(animator)
        {
        }
        
        public override void Enter()
        {
            Animator.SetBool(AnimationDescription.PLAYER_IDLE, true);
        }

        public override void Exit()
        {
            Animator.SetBool(AnimationDescription.PLAYER_IDLE, false);
        }
        
    }
}