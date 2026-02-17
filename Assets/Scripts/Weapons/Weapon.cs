
using UnityEngine;

namespace UrbanNinja
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _data;
        protected Pickup _pickUpInstance;
        protected GameObject _owner;
        private SpriteRenderer _spriteRenderer;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }
        public void SetPickUpInstance(Pickup pickup)
        {
            _pickUpInstance = pickup;
        }
        public void SetSortOrder(int order)
        {
            _spriteRenderer.sortingOrder = order;
        }
        protected abstract void DealDamage(Health damageTargetHealth);
        protected abstract void Animate();
        public abstract void Attack(Vector2 attackDirection);
        public abstract void Drop();
    }
}
