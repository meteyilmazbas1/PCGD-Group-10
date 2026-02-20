using System.Linq;
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
        private AnimationData _animationData;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        public void SetAnimationData(AnimationData data)
        {
            _animationData = data;
        }
        public void Request(AnimationType type, OnAnimationTrigger onAnimationEnd = null)
        {
            if(onAnimationEnd != null)
            {
                _onAnimationEnd = onAnimationEnd;
            }
            ResolveAnimation(type);
        }
        private void OnDisable()
        {
            _onAnimationEnd = null;
        }
        private void OnEnable()
        {
            if (_animator == null) return;
            _animator.SetBool("isDead", false);
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
        public void CancelAll()
        {
            _animator.SetBool("walk", false);
            _animator.SetBool("idle", false);
            _animator.ResetTrigger("jump");
            _animator.ResetTrigger("roll");
            _animator.ResetTrigger("kick");
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
                    _animator.SetTrigger("roll");
                    break;
                case AnimationType.Punch:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    int randomPunchIndex = Random.Range(0, _animationData.Punches.Count());
                    _animator.SetTrigger(_animationData.Punches.ElementAt(randomPunchIndex));
                    break;
                case AnimationType.Kick:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    int randomkickIndex = Random.Range(0, _animationData.Kicks.Count());
                    string kick = _animationData.Kicks.ElementAt(randomkickIndex);
                    _animator.SetTrigger(kick);
                    break;
                case AnimationType.Damage:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", false);
                    _animator.SetTrigger("damage");
                    break;
                case AnimationType.Death:
                    CancelAll();
                    _animator.SetBool("isDead", true);
                    _animator.SetTrigger("die");
                    break;
                default:
                    _animator.SetBool("walk", false);
                    _animator.SetBool("idle", true);
                    break;
            }
        }
    }
}
