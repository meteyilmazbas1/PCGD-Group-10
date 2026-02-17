using UnityEngine;
using System.Collections;
namespace UrbanNinja
{
    public class ThrowingWeapon : Weapon
    {
        [SerializeField] private float _angularVelocity = 30f;
        [SerializeField] private float _throwSpeed = 3f;
        private bool _isThrowing;
        private Coroutine _spinRoutine;
        private Vector2 _attackDirection;
        public override void Attack(Vector2 attackDirection)
        {
            transform.parent = null;
            _isThrowing = true;
            _attackDirection = attackDirection;
            Animate();
        }
        protected override void Animate()
        {
            _spinRoutine = StartCoroutine(FlySpin());
        }

        protected override void DealDamage(Health damageTargetHealth)
        {
            damageTargetHealth.TakeDamage(_data.Damage);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _owner) return;
            if (collision.gameObject.GetComponent<IPickUpTaker>() == null) return;
            if (!_isThrowing) return;
            _isThrowing = false;
            StopCoroutine(_spinRoutine);
            DealDamage(FindHealthComponentInTarget(collision.gameObject));
            Drop();
        }
        private Health FindHealthComponentInTarget(GameObject target)
        {
            Health health = target.GetComponent<Health>();
            if (health == null)
            {
                health = target.GetComponentInParent<Health>();
            }
            return health;
        }

        public override void Drop()
        {
            transform.parent = null;
            _pickUpInstance.gameObject.transform.position = transform.position;
            _pickUpInstance.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        private IEnumerator FlySpin()
        {
            float multiplier = _attackDirection.x < 0 ? _angularVelocity : -_angularVelocity;
            while (_isThrowing)
            {
                yield return null;
                transform.position += (Vector3)(_attackDirection * _throwSpeed * Time.deltaTime);
                transform.Rotate(0, 0, multiplier * Time.deltaTime);
            }
        }
    }
}