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
            Animator.SetBool("IsDeath", true);
        }

        public override void Exit()
        {
            Animator.SetBool("IsDeath", false);
        }
    }
}