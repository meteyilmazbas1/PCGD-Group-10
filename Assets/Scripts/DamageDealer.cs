
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(AudioSource))]
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private List<AudioClip> _hitClips;
        [SerializeField] private AudioClip _missHit;
        private AudioSource _audioSource;
        private GameObject _owner;
        private bool _isPlayerOwned;
        private Collider2D _collider;
        private delegate void QueuedSound();
        private Queue<QueuedSound> _SFXQueue = new Queue<QueuedSound>();
        private Weapon _weapon;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = false;
            _collider = GetComponent<Collider2D>();
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

        private AudioClip RandomClip()
        {
            int random = Random.Range(0, _hitClips.Count);
            return _hitClips[random];
        }
        public void Activate()
        {
            List<Collider2D> results = new List<Collider2D>();
            if (_collider == null) _collider = GetComponent<Collider2D>();
            _collider.Overlap(results);
            List<Health> healthList = new List<Health>();
            foreach (Collider2D collider in results)
            {
                if (collider.gameObject == _owner) continue;
                Health health = collider.GetComponent<Health>();
                if (health != null)
                {
                    healthList.Add(health);
                }
            }
            if (healthList.Count == 0) return;
            if (_isPlayerOwned)
            {
                foreach (Health health in healthList)
                {
                    health.TakeDamage(_damage);

                }
                _SFXQueue.Enqueue(PlayRandomDamageSound);
                // Register hit for combo system (only for player attacks)
                if (ComboManager.Instance != null)
                {
                    ComboManager.Instance.RegisterHit();

                    // Screen shake based on combo level
                    if (ScreenShake.Instance != null)
                    {
                        if (ComboManager.Instance.CurrentCombo >= 15)
                            ScreenShake.Instance.ShakeHeavy();
                        else if (ComboManager.Instance.CurrentCombo >= 5)
                            ScreenShake.Instance.ShakeMedium();
                        else
                            ScreenShake.Instance.ShakeLight();
                    }
                }
            }
            else
            {
                Health health = healthList.Find(x => x.gameObject.layer == 7);
                if (health == null) return;
                health.TakeDamage(_damage);
                _SFXQueue.Enqueue(PlayRandomDamageSound);
            }
        }

        private void PlayRandomDamageSound()
        {
            _audioSource.clip = RandomClip();
            _audioSource.Play();
        }

        public void PlaySwoosh()
        {
            _SFXQueue.Enqueue(Swoosh);
        }
        private void Swoosh()
        {
            
            _audioSource.clip = _missHit;
            _audioSource.Play();
        }
        private IEnumerator SoundEffectQueueRoutine()
        {
            while (true) 
            {
                yield return null;
                if (_SFXQueue.Count == 0) continue;
                if (_audioSource.isPlaying) continue;
                var dle = _SFXQueue.Dequeue();
                dle.Invoke();
            }
        }
        private Coroutine _sfxQueueRoutine;
        private void OnEnable()
        {
            if (_sfxQueueRoutine != null)
            {
                StopCoroutine(_sfxQueueRoutine);
            }
            _sfxQueueRoutine=StartCoroutine(SoundEffectQueueRoutine());
        }
        private void OnDisable()
        {
            StopCoroutine(_sfxQueueRoutine);
            _sfxQueueRoutine = null;
        }
        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon; 
        }
        public void ClearWeapon()
        {
            _weapon = null;
        }
        public void ShowWeapon(bool visible)
        {
            if (_weapon == null) return;
            _weapon.Show(visible);
        }
    }

}