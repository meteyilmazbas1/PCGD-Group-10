
using UnityEngine;

namespace UrbanNinja
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _data;
        protected Pickup _pickUpInstance;
        protected GameObject _owner;
        public void SetOwner(GameObject owner)
        {
            Debug.LogWarning("WEAPON OWNER IS "+owner.name);
            _owner = owner;
        }
        public void SetPickUpInstance(Pickup pickup)
        {
            _pickUpInstance = pickup;
        }
        protected abstract void DealDamage(Health damageTargetHealth);
        protected abstract void Animate();
        public abstract void Attack(Vector2 attackDirection);
        protected abstract void Drop();
    }
}
