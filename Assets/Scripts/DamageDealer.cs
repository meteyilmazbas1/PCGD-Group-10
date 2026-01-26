using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("TRIGGER");
        if (collision.gameObject.layer == LayerMask.GetMask("Player")) return;
        Debug.Log(gameObject.name+" Deals damage to "+(collision.name));
    }
}
