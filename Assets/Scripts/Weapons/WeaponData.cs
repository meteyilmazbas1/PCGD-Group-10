using System.Collections.Generic;
using UnityEngine;


namespace UrbanNinja
{
    [CreateAssetMenu(menuName = "Urban Ninja/Weapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private AudioClip _hitSound;

        public int Damage => _damage;
        public AudioClip HitSound => _hitSound;
    }

    [CreateAssetMenu(menuName = "Weapons data")]
    public class AllWeaponsData: ScriptableObject
    {
        [SerializeField] List<GameObject> _prefabs;
    }
}