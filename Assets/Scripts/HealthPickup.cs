using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Collider2D))]
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private int _healAmount = 3;

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
            if (collision == null) return;

            var playerController = collision.GetComponent<PlayerController>();
            if (playerController == null) return;

            var health = collision.GetComponent<Health>();
            if (health == null) return;

            health.Heal(_healAmount);
            gameObject.SetActive(false);
        }
    }
}