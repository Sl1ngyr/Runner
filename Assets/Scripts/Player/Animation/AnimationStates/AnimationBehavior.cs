using UnityEngine;

namespace Player.Animation.AnimationStates
{
    public abstract class AnimationBehavior
    {
        protected Animator Animator;

        public AnimationBehavior(Animator animator)
        {
            this.Animator = animator;
        }
        
        public abstract void Enter();

        public abstract void Exit();
    }
}