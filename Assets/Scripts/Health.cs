using UnityEngine;

namespace UrbanNinja
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth = 3;
        private int _currentHealth;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        
        public delegate void OnDeath();
        public event OnDeath OnDeathEvent;
        
        public delegate void OnHealthChanged(int currentHealth);
        public event OnHealthChanged OnHealthChangedEvent;

        /// <summary>
        /// Set max health here, because it might vary
        /// between spawns when the asset is reused by
        /// the object pool.
        /// </summary>
        /// <param name="hitPoints">Hitpoints from data.</param>
        public void SetMaxHealth(int hitPoints)
        {
            _maxHealth = hitPoints;
            ResetHealth();
        }

        /// <summary>
        /// Reset health here, because enemies etc.
        /// are being spawned from an object pool.
        /// They are not destroyed, but disabled and
        /// enabled repeatedly.
        /// </summary>
        private void OnEnable()
        {
            ResetHealth();
        }

        private void ResetHealth()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            //Debug.Log($"{gameObject.name} took {amount} damage. HP now: {_currentHealth}");
            OnHealthChangedEvent?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Restore health. Used by pickups etc.
        /// </summary>
        /// <param name="amount">Amount of health to restore.</param>
        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            OnHealthChangedEvent?.Invoke(_currentHealth);
        }

        /// <summary>
        /// Restore health. Used by pickups etc.
        /// Clamps health to max value.
        /// </summary>
        /// <param name="amount">Amount of health to restore.</param>
        public void TakeHealing(int amount)
        {
            if (amount <= 0 || _currentHealth <= 0) return;
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            OnHealthChangedEvent?.Invoke(_currentHealth);
        }

        private void Die()
        {
            //Debug.Log($"{gameObject.name} died");
            OnDeathEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
