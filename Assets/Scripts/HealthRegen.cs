using System.Collections;
using UnityEngine;

namespace UrbanNinja
{

    public class HealthRegen : MonoBehaviour
    {
        [SerializeField] int _tickRate;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }
        private IEnumerator Start()
        {
            while (_health.CurrentHealth > 0)
            {
                yield return new WaitForSeconds(_tickRate);
                _health.Heal(1);
            }
        }
    }

}