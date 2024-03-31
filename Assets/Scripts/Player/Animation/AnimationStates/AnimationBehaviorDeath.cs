using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public class AnimationBehaviorDeath : AnimationBehavior
    {
        public AnimationBehaviorDeath(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(AnimationDescription.PLAYER_DEATH, true);
        }

        public override void Exit()
        {
            Animator.SetBool(AnimationDescription.PLAYER_DEATH, false);
        }
    }
}