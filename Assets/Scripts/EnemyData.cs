using UnityEngine;
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
    [SerializeField] private float _movementSpeed;
    [SerializeField] private string _name;

    public float AttackPower => _attackPower;
    public float AttackDistance => _attackDistance;
    public float MovementSpeed => _movementSpeed;
    public string Name => _name;
}
