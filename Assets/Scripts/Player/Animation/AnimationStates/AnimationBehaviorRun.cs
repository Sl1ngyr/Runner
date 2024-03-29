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
            Animator.SetBool("IsRunning", true);
        }
        
        public override void Exit()
        {
            Animator.SetBool("IsRunning", false);
        }
        
    }
}