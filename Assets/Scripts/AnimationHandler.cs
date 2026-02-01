using UnityEngine;

namespace UrbanNinja
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        public delegate void OnAnimationTrigger();
        private OnAnimationTrigger _onAnimationEnd;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Request(string id, OnAnimationTrigger onAnimationEnd = null)
        {
            ResolveAnimation(id);
            _onAnimationEnd = onAnimationEnd;
        }
        /// <summary>
        /// Method to be called from an animation event
        /// defined in the animation clip.
        /// </summary>
        public void OnAnimationEnd()
        {
            _onAnimationEnd?.Invoke();
        }
        private void ResolveAnimation(string id)
        {
            switch (id)
            {
                case "walk":
                    _animator.SetBool("walk", true);
                    _animator.SetBool("idle", false);
                    break;
                case "idle":
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", true);
                    break;
                case "jump":
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("jump");
                    //_animator.ResetTrigger("jump");
                    break;
                case "punch":
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("punch");
                    //_animator.ResetTrigger("punch");
                    break;
                case "kick":
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("kick");
                    //_animator.ResetTrigger("kick");
                    break;
                default:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", true);
                    break;
            }
        }
    }
}
