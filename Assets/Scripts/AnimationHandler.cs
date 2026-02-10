using UnityEngine;

namespace UrbanNinja
{
    public enum AnimationType
    {
        Walk,
        Idle,
        Jump,
        Punch,
        Kick,
        Damage,
        Death
    }
    public class AnimationHandler : MonoBehaviour
    {

        private Animator _animator;
        public delegate void OnAnimationTrigger();
        private OnAnimationTrigger _onAnimationEnd;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Request(AnimationType type, OnAnimationTrigger onAnimationEnd = null)
        {
            ResolveAnimation(type);
            if(onAnimationEnd != null)
            {
                //if (_onAnimationEnd != null) _onAnimationEnd.Invoke();
                _onAnimationEnd = onAnimationEnd;
            }
        }
        private void OnDisable()
        {
            _onAnimationEnd = null;
        }
        /// <summary>
        /// Method to be called from an animation event
        /// defined in the animation clip.
        /// </summary>
        public void OnAnimationEnd()
        {
            _onAnimationEnd?.Invoke();
            _onAnimationEnd = null;
        }

        private void ResolveAnimation(AnimationType type)
        {
            switch (type)
            {
 
                case AnimationType.Walk:
                    _animator.SetBool("walk", true);
                    _animator.SetBool("idle", false);
                    break;
                case AnimationType.Idle:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", true);
                    break;
                case AnimationType.Jump:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("jump");
                    //_animator.ResetTrigger("jump");
                    break;
                case AnimationType.Punch:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("punch");
                    //_animator.ResetTrigger("punch");
                    break;
                case AnimationType.Kick:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("kick");
                    //_animator.ResetTrigger("kick");
                    break;
                case AnimationType.Damage:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("damage");
                    //_animator.ResetTrigger("kick");
                    break;
                case AnimationType.Death:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("die");
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
