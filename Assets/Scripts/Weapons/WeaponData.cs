using System.Collections.Generic;
using UnityEngine;


namespace UrbanNinja
{
    [CreateAssetMenu(menuName = "Urban Ninja/Weapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private int _damage;

        public int Damage => _damage;
    }

    [CreateAssetMenu(menuName = "Weapons data")]
    public class AllWeaponsData: ScriptableObject
    {
        [SerializeField] List<GameObject> _prefabs;
    }
}