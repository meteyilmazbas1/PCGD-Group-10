
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
        private Collider2D _collider;
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
            if(_collider == null ) _collider = GetComponent<Collider2D>();
            _collider.Overlap(results);
            if (_isPlayerOwned)
            {
                foreach(Collider2D collider in results)
                {
                    Health health = collider.GetComponent<Health>();
                    if (health != null)
                    {
                        //Debug.Log($"{gameObject.name} HIT {collision.name} at {Time.time}");
                        health.TakeDamage(_damage);

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

                        if (_hitClips != null && _hitClips.Count > 0)
                        {
                            //_audioSource.clip = RandomClip();
                            //_audioSource.Play();
                            AudioSource.PlayClipAtPoint(RandomClip(), transform.position); //this seems to work better
                        }
                        //Debug.Log(gameObject.name+" Deals damage to "+(collision.name));
                    }
                }
            }
            else
            {
                if (results.Count > 0)
                {
                    List<Collider2D> player = results.FindAll(x => x.gameObject.layer == 7);
                    Health h = null;
                    foreach(Collider2D collider in player)
                    {
                        h = collider.GetComponent<Health>();
                    }
                    if(h != null)
                    {
                        h.TakeDamage(_damage);
                        if (_hitClips != null && _hitClips.Count > 0)
                        {
                            //_audioSource.clip = RandomClip();
                            //_audioSource.Play();
                            AudioSource.PlayClipAtPoint(RandomClip(), transform.position); //this seems to work better
                        }
                    }
                }
            }

        }
    }

}