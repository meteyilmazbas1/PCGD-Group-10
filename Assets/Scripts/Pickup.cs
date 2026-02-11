using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour
    {
        public enum PickupType
        {
            Health,
            Score
        }
        [SerializeField] private int _amount = 3;
        [SerializeField] private bool _useLayerCheck = true;
        [SerializeField] private int _playerLayer = 7; // Player layer index (Edit > Project Settings > Tags and Layers)
        [SerializeField] private AudioClip _pickupSound; // Optional: Sound to play when picked up
        [SerializeField] private GameObject _pickupEffect; // Optional: Particle effect or visual effect to spawn when picked up
        [SerializeField] private PickupType _pickupType;
        private void Awake()
        {
            // Ensure collider is set up correctly
            var col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
            else
            {
                //Debug.LogError($"HealthPickup: No Collider2D found on {gameObject.name}!");
            }
        }

        private void Reset()
        {
            var col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
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
                        PickupMessageUI.Instance.ShowHealMessage(_amount);
                        break;
                    case PickupType.Score:
                        PickupMessageUI.Instance.ShowScoreMessage(_amount);
                        break;
                }
            }
        }
    }
}
