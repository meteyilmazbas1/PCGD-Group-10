
using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    /// <summary>
    /// This a pickup randomizing class that will detect
    /// Pickups childed to it and activate a random amount 
    /// of those.
    /// </summary>
    public class WeaponRandomizer : MonoBehaviour
    {
        [SerializeField] private int _minPickups = 1;
        [SerializeField] private int _maxPickups = 3;
        private List<Pickup> _worldPickups;
        private List<Pickup> _activePickups = new List<Pickup>();
        private void Awake()
        {
            _worldPickups = new List<Pickup>();
            foreach (Pickup pickup in GetComponentsInChildren<Pickup>())
            {
                _worldPickups.Add(pickup);
                pickup.gameObject.SetActive(false);
            }
            int max = _maxPickups > _worldPickups.Count ? _worldPickups.Count : _maxPickups;
            int toActivate = Random.Range(_minPickups, max+1);
            while (_activePickups.Count < toActivate)
            {
                int index = Random.Range(0, _worldPickups.Count);
                if (_worldPickups[index].isActiveAndEnabled) continue;
                Pickup pickup = _worldPickups[index];
                pickup.gameObject.SetActive(true);
                _activePickups.Add(pickup);
            }
        }
    }
}
