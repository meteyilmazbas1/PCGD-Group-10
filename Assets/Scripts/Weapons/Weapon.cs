
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _data;
        [SerializeField] protected Pickup _pickupPrefab;
        protected Pickup _pickUpInstance;
        protected GameObject _owner;
        private SpriteRenderer _spriteRenderer;
        private AudioSource _audioSource;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
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
        public void Show(bool visilble)
        {
            _spriteRenderer.enabled = visilble;
        }
        protected void PlayHitSound()
        {
            //_audioSource.clip = _data.HitSound;
            //_audioSource.Play();
            AudioSource.PlayClipAtPoint(_data.HitSound, Camera.main.transform.position);
        }
        protected abstract void DealDamage(Health damageTargetHealth);
        protected abstract void Animate();
        public abstract void Attack(Vector2 attackDirection);
        public abstract void Drop();
    }
}
