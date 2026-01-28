
using System.Collections.Generic;
using UnityEngine;
using UrbanNinja;

[RequireComponent(typeof(AudioSource))]
public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private List<AudioClip> _hitClips;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogWarning("TRIGGER");
        if (collision.gameObject.layer == LayerMask.GetMask("Player")) return;
        
        
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(_damage);
            _audioSource.clip = RandomClip();
            _audioSource.Play();
            Debug.Log(gameObject.name+" Deals damage to "+(collision.name));
        }
    }
    private AudioClip RandomClip()
    {
        int random = Random.Range(0, _hitClips.Count);
        return _hitClips[random];
    }
}
