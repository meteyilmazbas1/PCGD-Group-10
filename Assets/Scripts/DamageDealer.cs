using UnityEngine;
using UrbanNinja;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("TRIGGER");
        if (collision.gameObject.layer == LayerMask.GetMask("Player")) return;
        
        
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(_damage);
            Debug.Log(gameObject.name+" Deals damage to "+(collision.name));
        }
    }
}
