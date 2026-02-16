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
        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject == _owner) return;
            if (!_isThrowing) return;
            _isThrowing = false;
            StopCoroutine(_spinRoutine);
            DealDamage(FindHealthComponentInTarget(collision.gameObject));
            Debug.Log("COLLISION HIT " + collision.gameObject.name);
            Destroy(gameObject);

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"COLLISION: owner= {_owner} collision= {collision.gameObject}");
            if (collision.gameObject == _owner) return;
            if (collision.gameObject.GetComponent<DamageDealer>() != null) return;
            if (!_isThrowing) return;
            _isThrowing = false;
            StopCoroutine(_spinRoutine);
            DealDamage(FindHealthComponentInTarget(collision.gameObject));
            Debug.Log("TRIGGER HIT " + collision.gameObject.name);
            _pickUpInstance.gameObject.transform.position = transform.position;
            _pickUpInstance.gameObject.SetActive(true);
            
            Destroy(gameObject);
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

        protected override void Drop()
        {
            throw new System.NotImplementedException();
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