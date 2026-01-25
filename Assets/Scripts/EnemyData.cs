using UnityEngine;

namespace UrbanNinja
{
    /// <summary>
    /// This is data class for defining different enemies.
    /// Access it from asset menu's "Urban Ninja/EnemyData"
    /// to create a new enemy data.
    /// 
    /// Use a predefined enemy data scriptable objects for enemy prefabs.
    /// </summary>
    [CreateAssetMenu(menuName = "Urban Ninja/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float _attackPower;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _movementSpeedX;
        [SerializeField] private float _movementSpeedY;
        [SerializeField] private string _name;
        [SerializeField] private int _hitPoints;
        [SerializeField] private int _scoreYield;

        public float AttackPower => _attackPower;
        public float AttackDistance => _attackDistance;
        public float MovementSpeedY => _movementSpeedY;
        public float MovementSpeedX => _movementSpeedX;
        public string Name => _name;
        public int HitPoints => _hitPoints;
        public int ScoreYield => _scoreYield;
    }
}
