using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Collider2D))]
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private int _healAmount = 3;
        [SerializeField] private bool _useLayerCheck = true;
        [SerializeField] private int _playerLayer = 7; // Player layer index (Edit > Project Settings > Tags and Layers)
        [SerializeField] private AudioClip _pickupSound; // Optional: Sound to play when picked up
        [SerializeField] private GameObject _pickupEffect; // Optional: Particle effect or visual effect to spawn when picked up

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
                Debug.LogError($"HealthPickup: No Collider2D found on {gameObject.name}!");
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
            if (collision == null)
            {
                Debug.LogWarning("HealthPickup: Collision is null!");
                return;
            }

            // Layer check (optional but recommended for security)
            if (_useLayerCheck)
            {
                if (collision.gameObject.layer != _playerLayer)
                {
                    // Also check parent in case collider is on child object
                    if (collision.transform.parent == null || collision.transform.parent.gameObject.layer != _playerLayer)
                    {
                        return; // Not on Player layer, ignore
                    }
                }
            }

            // Try to get PlayerController first
            var playerController = collision.GetComponent<PlayerController>();
            if (playerController == null)
            {
                // Also check parent in case collider is on child object
                playerController = collision.GetComponentInParent<PlayerController>();
            }
            
            if (playerController == null)
            {
                return; // Not a player, ignore
            }

            // Try to get Health component
            var health = collision.GetComponent<Health>();
            if (health == null)
            {
                // Also check parent in case collider is on child object
                health = collision.GetComponentInParent<Health>();
            }
            
            if (health == null)
            {
                Debug.LogWarning($"HealthPickup: Player found but Health component not found on {collision.gameObject.name}!");
                return;
            }

            // Heal the player
            int oldHealth = health.CurrentHealth;
            health.Heal(_healAmount);
            int newHealth = health.CurrentHealth;
            
            Debug.Log($"HealthPickup: Player healed! {oldHealth} -> {newHealth} (healed {_healAmount})");
            
            // Show UI message
            ShowPickupMessage();
            
            // Play sound effect if available
            PlayPickupSound();
            
            // Spawn visual effect if available
            SpawnPickupEffect();
            
            // Disable the pickup
            gameObject.SetActive(false);
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
                PickupMessageUI.Instance.ShowHealMessage(_healAmount);
            }
        }
    }
}
