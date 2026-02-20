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
        private bool _dropped = false;
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
            if (_dropped) return;
            if (collision.gameObject == _owner) return;
            if (collision.gameObject.GetComponent<IPickUpTaker>() == null) return;
            if (!_isThrowing) return;

            _isThrowing = false;
            PlayHitSound();
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
            _dropped = true;
            transform.parent = null;
            _owner = null;
            if(_pickUpInstance == null)
            {
                SetPickUpInstance(Instantiate(_pickupPrefab));
            }
            _pickUpInstance.gameObject.SetActive(true);
            _pickUpInstance.gameObject.transform.position = transform.position;
            _pickUpInstance.DropEffect();
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