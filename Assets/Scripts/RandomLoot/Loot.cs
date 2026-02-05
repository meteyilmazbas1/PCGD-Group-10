using UnityEngine;

namespace UrbanNinja
{
    [System.Serializable]
    public class Loot
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _probablityWeight;

        public GameObject Prefab => _prefab;
        public int ProbabilityWeight => _probablityWeight;
    }
}