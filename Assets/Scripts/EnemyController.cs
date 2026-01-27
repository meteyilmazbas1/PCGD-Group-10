using UnityEngine;

namespace UrbanNinja
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        private PlayerController _playerController;
        private bool _canAttack;
        private Vector2 _movementDirection;
        private Vector2 _left = new Vector2(-1, 1);
        private Vector2 _right = new Vector2(1, 1);
        private float _movementRandomizer;
        private float _yThreshold = 0.05f;
        private float _xThreshold = 0.2f;
        private AnimationHandler _animationHandler;
        private float _attackCooldown  = 0f;
        private float _baseCooldown = 1f;
        private Health _enemyHealth;
        void Start()
        {
            GetReferences();
            Randomize();
            _enemyHealth.SetMaxHealth(_enemyData.HitPoints);
        }

        private void Randomize()
        {
            _movementRandomizer = Random.Range(0.8f, 1.2f);
            _baseCooldown *= _movementRandomizer;
        }

        private void GetReferences()
        {
            _playerController = GameManager.GetPlayerController();
            _animationHandler = GetComponent<AnimationHandler>();
            _enemyHealth = GetComponent<Health>();
        }

        void FixedUpdate()
        {
            MoveToPlayer();
            FlipTransform();
            Attack();
        }
        /// <summary>
        /// A very basic AI movement. Position is applied with a random offset
        /// based on _movementRandomizer. Position also is offset from the player
        /// position such that enemies don't pile up on player.
        /// </summary>
        private void MoveToPlayer()
        {
            Vector2 positionDifference = _playerController.GetPositionRelativeToJump() - (Vector2)transform.position;
            _canAttack = true;
            _movementDirection = Vector2.zero;
            if (Mathf.Abs(positionDifference.x) < _enemyData.AttackDistance * _movementRandomizer - _xThreshold)
            {
                _movementDirection = _movementRandomizer < 1 ? Vector2.left : Vector2.right;
                _canAttack = false;
            }
            if (Mathf.Abs(positionDifference.y) > _yThreshold)
            {
                _movementDirection += positionDifference.y < 0 ? Vector2.down : Vector2.up;
                _canAttack = false;
            }
            if (Mathf.Abs(positionDifference.x) > _enemyData.AttackDistance * _movementRandomizer + _xThreshold)
            {
                _movementDirection += positionDifference.x < 0 ? Vector2.left : Vector2.right;
                _canAttack = false;
            }
            if(_movementDirection != Vector2.zero)
            {
                _animationHandler.Request("walk");
            }
            else
            {
                _animationHandler.Request("idle");
            }
            transform.Translate(_movementDirection.normalized * _enemyData.MovementSpeedX * _movementRandomizer * Time.deltaTime);
        }
        /// <summary>
        /// Face movement direction.
        /// </summary>
        private void FlipTransform()
        {
            if (_movementDirection == Vector2.zero)
            {
                float playerDirection = _playerController.GetPositionRelativeToJump().x - transform.position.x;
                transform.localScale = playerDirection > 0 ? _right : _left;
            }
            else
            {
                transform.localScale = _movementDirection.x > 0 ? _right : _left;
            }

        }
        /// <summary>
        /// Attack player if possible.
        /// </summary>
        private void Attack()
        {
            if (!_canAttack) return;
            if (_attackCooldown > 0)
            {
                _attackCooldown-= Time.deltaTime;
                return;
            }
            _attackCooldown = _baseCooldown;
            int select = Random.Range(0,2);
            string attack = select == 1 ? "punch" : "kick";
            _animationHandler.Request(attack);
            //Debug.Log($"Enemy {_enemyData.Name} attacking player");
        }
    }
}
