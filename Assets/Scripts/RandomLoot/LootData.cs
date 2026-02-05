using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    [CreateAssetMenu(menuName = "Urban Ninja/LootData")]
    public class LootData : ScriptableObject
    {
        [SerializeField] public  List<Loot> _lootDrop_prefabs;
    }
}