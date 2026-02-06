using System.Collections.Generic;
using UnityEngine;


namespace UrbanNinja
{
    public static class RandomLootService
    {
        private static LootData _lootData;
        private static Dictionary<int, GameObject> _lootTable = new();
        private static int _totalWeight;
        private static void InitLootService()
        {
            GetLootData();
            BuildLootTable();
        }
        private static void GetLootData()
        {
            _lootData = Resources.Load<LootData>("Data/data_loot");
        }
        private static void BuildLootTable()
        {
            _totalWeight = 0;
            foreach (Loot loot in _lootData._lootDrop_prefabs)
            {
                _totalWeight += loot.ProbabilityWeight;
                _lootTable.Add(_totalWeight, loot.Prefab);
            }
        }
        private static int Roll()
        {
            return Random.Range(0, _totalWeight);
        }
        private static GameObject GetRandomPrefab()
        {
            int roll = Roll();
            List<int> keys = new List<int>(_lootTable.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (roll <= keys[i])
                {
                    if (_lootTable.ContainsKey(keys[i])) return _lootTable[keys[i]];
                }
            }
            return null;
        }
        public static void RequestLoot(Vector2 position)
        {
            if (_lootData == null) InitLootService();

            GameObject prefab = GetRandomPrefab();
            if (prefab == null)
            {
                Debug.LogError("Loot prefab was null!");
                return;
            }
            GameObject loot = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}