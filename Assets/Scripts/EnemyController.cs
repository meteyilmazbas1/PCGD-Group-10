using UnityEngine;
using UrbanNinja;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    private PlayerController _playerController;
    private bool _canAttack;
    private Vector2 _movementDirection;
    private Vector2 _left = new Vector2(-1, 1);
    private Vector2 _right = new Vector2(1, 1);
    void Start()
    {
        _playerController = GameManager.GetPlayerController();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
        FlipTransform();
        Attack();
    }
    /// <summary>
    /// A very basic AI movement.
    /// </summary>
    private void MoveToPlayer()
    {
        Vector2 positionDifference = _playerController.transform.position - transform.position;
        if (positionDifference.magnitude > _enemyData.AttackDistance)
        {
            _movementDirection = positionDifference.normalized;
            transform.Translate(_movementDirection * _enemyData.MovementSpeed * Time.deltaTime);
            _canAttack = false;
        }
        else
        {
            _canAttack = true;
        }
    }
    /// <summary>
    /// Face movement direction.
    /// </summary>
    private void FlipTransform()
    {
        transform.localScale = _movementDirection.x > 0 ? _right : _left;
    }
    /// <summary>
    /// Attack player if possible.
    /// </summary>
    private void Attack()
    {
        if (!_canAttack) return;
        Debug.Log($"Enemy {_enemyData.Name} attacking player");
    }
}
