using System.Collections;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour
    {
        public enum PickupType
        {
            Health,
            Score,
            Weapon
        }
        [SerializeField] private int _amount = 3;
        [SerializeField] private bool _useLayerCheck = true;
        [SerializeField] private int _playerLayer = 7; // Player layer index (Edit > Project Settings > Tags and Layers)
        [SerializeField] private AudioClip _pickupSound; // Optional: Sound to play when picked up
        [SerializeField] private GameObject _pickupEffect; // Optional: Particle effect or visual effect to spawn when picked up
        [SerializeField] private PickupType _pickupType;
        [SerializeField] private Weapon _weapon; //If this is a weapon pickup!
        [SerializeField] private AudioClip _dropSound; //If this is a weapon pickup!

        private Collider2D _collider;
        private bool _isReDrop;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
        }
        private void OnEnable()
        {
            _collider.enabled = true;
            if (_isReDrop)
            {
                DropEffect();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(_pickupType == PickupType.Weapon)
            {
                Debug.Log("Picking up weapon");
                PlayerController  p=  collision.GetComponent<PlayerController>();
                if (p == null)
                {
                    p = collision.GetComponentInParent<PlayerController>();
                    if (p != null)
                    {
                        if (p.HasWeapon()) return;
                        _collider.enabled = false;
                        Weapon weapon = Instantiate(_weapon, transform.position, Quaternion.identity);
                        weapon.SetOwner(p.gameObject);
                        weapon.SetPickUpInstance(this);
                        p.AddWeapon(weapon);
                        _isReDrop = true;
                        gameObject.SetActive(false);
                    }
                    return;
                }
                else
                {
                    if (p.HasWeapon()) return;
                    _collider.enabled = false;
                    Weapon weapon = Instantiate(_weapon, transform.position, Quaternion.identity);
                    weapon.SetOwner(p.gameObject);
                    weapon.SetPickUpInstance(this);
                    p.AddWeapon(weapon);
                    _isReDrop = true;
                    gameObject.SetActive(false);
                }
                return;
            }
            Debug.Log("THIS IS BAD");
            // Layer check (optional but recommended for security)
            if (_useLayerCheck)
            {
                if (collision.gameObject.layer != _playerLayer)
                {
                    // Also check parent in case collider is on child object
                    if (collision.transform.parent == null ||
                        collision.transform.parent.gameObject.layer != _playerLayer)
                    {
                        return; // Not on Player layer, ignore
                    }
                }
            }
 

            bool flowControl = HandlePickup(collision);
            if (!flowControl)
            {
                return;
            }
            _collider.enabled = false;
            // Show UI message
            ShowPickupMessage();

            // Play sound effect if available
            PlayPickupSound();

            // Spawn visual effect if available
            SpawnPickupEffect();

            // Disable the pickup
            gameObject.SetActive(false);
        }

        private bool HandlePickup(Collider2D collision)
        {
            if(_pickupType == PickupType.Health)
            {
                // Try to get Health component
                var health = collision.GetComponent<Health>();
                if (health == null)
                {
                    health = collision.GetComponentInParent<Health>();
                }

                if (health == null)
                {
                    return false;
                }

                health.Heal(_amount);
                return true;
            }
            else
            {
                GameManager.AddScore(_amount);
                return true;
            }
        }

        private void PlayPickupSound()
        {
            if (SoundManager.Instance != null)
            {
                if (_pickupSound != null)
                {
                    SoundManager.Instance.PlaySound(_pickupSound);
                }
                else
                {
                    // Fallback: Use button click sound if no pickup sound assigned
                    SoundManager.Instance.PlayButtonClick();
                }
            }
        }

        private void SpawnPickupEffect()
        {
            if (_pickupEffect != null)
            {
                GameObject effect = Instantiate(_pickupEffect, transform.position, Quaternion.identity);
                // Auto-destroy effect after 2 seconds (adjust as needed)
                Destroy(effect, 2f);
            }
        }

        private void ShowPickupMessage()
        {
            if (PickupMessageUI.Instance != null)
            {
                switch (_pickupType)
                {
                    case PickupType.Health:
                        PickupMessageUI.Instance.ShowHealMessage(_amount, transform);
                        break;
                    case PickupType.Score:
                        PickupMessageUI.Instance.ShowScoreMessage(_amount, transform);
                        break;
                }
            }
        }
        public void DropEffect()
        {
            StartCoroutine(DropNBounce());
        }
        private IEnumerator DropNBounce()
        {
            Vector2 ground = transform.position - Vector3.up;
            int bounces = 4;
            Vector2 dir = Vector2.down;
            Vector2 velocity = Vector2.zero;
            
            while (bounces > 0)
            {
                yield return new WaitForFixedUpdate();
                velocity = velocity +  Vector2.up * (-200f * Time.fixedDeltaTime * Time.fixedDeltaTime);
                transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
                if (transform.position.y <= ground.y)
                {
                    bounces--;
                    velocity = -0.7f * velocity;
                    transform.position = ground;
                    SoundManager.Instance.PlaySound(_dropSound);
                }
            }
            transform.position = ground;

        }
    }
}
