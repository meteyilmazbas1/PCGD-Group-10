
using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(AudioSource))]
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private List<AudioClip> _hitClips;
        private AudioSource _audioSource;
        private GameObject _owner;
        private bool _isPlayerOwned;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = false;
        }
        
        public void SetOwner(GameObject owner)
        {
            _owner = owner;
            // Check if owner is the player for combo tracking
            _isPlayerOwned = owner != null && owner.GetComponent<PlayerController>() != null;
        }
        public void SetDamage(int damage)
        {
            _damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _owner) return;
            Health health = collision.GetComponent<Health>();
            if (health != null)
            {
                //Debug.Log($"{gameObject.name} HIT {collision.name} at {Time.time}");
                health.TakeDamage(_damage);
                
                // Register hit for combo system (only for player attacks)
                if (_isPlayerOwned && ComboManager.Instance != null)
                {
                    ComboManager.Instance.RegisterHit();
                }
                
                if (_hitClips != null && _hitClips.Count > 0)
                {
                    _audioSource.clip = RandomClip();
                    _audioSource.Play();
                }
                //Debug.Log(gameObject.name+" Deals damage to "+(collision.name));
            }
        }
        private AudioClip RandomClip()
        {
            int random = Random.Range(0, _hitClips.Count);
            return _hitClips[random];
        }
    }

}