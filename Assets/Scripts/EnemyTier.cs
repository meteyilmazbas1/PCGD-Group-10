using UnityEngine;

namespace UrbanNinja
{
    public class EnemyTier
    {
        public enum MultiplierType
        {
            Health,
            Speed,
            Attack
        }
        private int _tierLevel = 0;
        const float c_healtMultiplier = 1.1f;
        const float c_movementSpeedMultiplier = 1.05f;
        const float c_attackPowerMultiplier = 1.1f;
        public int TierLevel => _tierLevel;
        public EnemyTier() { }
        public EnemyTier(int tierLevel)
        {
            _tierLevel = tierLevel;
        }

        public void IncreaseTier()
        {
            _tierLevel++;
        }
        /// <summary>
        /// Get the current multiplier based on tier level.
        /// </summary>
        /// <returns>A float value: multiplier^level. </returns>
        public float GetMultiplier(MultiplierType multiplierType)
        {
            switch (multiplierType)
            {
                case MultiplierType.Health:
                    return Mathf.Pow(c_healtMultiplier, _tierLevel);
                case MultiplierType.Speed:
                    return Mathf.Pow(c_movementSpeedMultiplier, _tierLevel);
                case MultiplierType.Attack:
                    return Mathf.Pow(c_attackPowerMultiplier, _tierLevel);
                default: return 1f;
            }
        }
    }
}
