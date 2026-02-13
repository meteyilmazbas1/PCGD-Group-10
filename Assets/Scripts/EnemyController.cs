using System.Collections;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private DamageDealer _fist;   // ADDED
        [SerializeField] private DamageDealer _foot;   // ADDED
        
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
        private EnemyTier _tier;
        private int _maxHealth;
        private float _XmovementSpeed;
        private int _attackPower;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private Blinker _blinker;
        private void Awake()
        {
            _tier = new EnemyTier();
            _fist.SetOwner(gameObject);
            _foot.SetOwner(gameObject);
            GetReferences();
            Randomize();
            //DisableFistAndFoot();  // ADDED
            SetStats();
        }

        private void SetStats()
        {
            _maxHealth = Mathf.RoundToInt(_enemyData.HitPoints * _tier.GetMultiplier(EnemyTier.MultiplierType.Health));
            _enemyHealth.SetMaxHealth(_maxHealth);
            _XmovementSpeed = _enemyData.MovementSpeedX * _tier.GetMultiplier(EnemyTier.MultiplierType.Speed);
            _attackPower = Mathf.RoundToInt(_enemyData.AttackPower * _tier.GetMultiplier(EnemyTier.MultiplierType.Attack));
            _fist.SetDamage(_attackPower);
            _foot.SetDamage(_attackPower);
        }
        private void OnEnable()
        {
            UnStun();
            if(_enemyHealth == null)
            {
                GetReferences();
            }
            _enemyHealth.OnDeathEvent += OnDeath;
            _enemyHealth.OnHealthChangedEvent += OnHealthChaged;
            //DisableFistAndFoot();
            SetStats();
            _spriteRenderer.color = Color.white;
            //Debug.Log($"Enemy spawn tier: {_tier.TierLevel}");
        }
        private void OnDisable()
        {
            _tier.IncreaseTier();
            _enemyHealth.OnDeathEvent -= OnDeath;
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
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _blinker = GetComponent<Blinker>();
            _blinker.SetSpriteRenderer(_spriteRenderer);

            _rb.gravityScale = 0f;
        }
        private bool _stunned = false;
        private void OnHealthChaged(int value, bool isDamage = false)
        {
            _stunned = true;
            //DisableFistAndFoot();
            if (_enemyHealth.CurrentHealth <= 0) return;
            _blinker.BlinkDamage();
            _animationHandler.Request(AnimationType.Damage, onAnimationEnd: UnStun);
        }
        private void UnStun()
        {
            //DisableFistAndFoot();
            _stunned = false;
        }
        void FixedUpdate()
        {
            if (_stunned) return;
            MoveToPlayer();
            FlipTransform();
            Attack();
        }
        
        private void MoveToPlayer()
        {
            if (!_playerController.Alive)
            {
                _animationHandler.Request(AnimationType.Idle);
                _stunned = true;
            }
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
                _animationHandler.Request(AnimationType.Walk);
            }
            else
            {
                _animationHandler.Request(AnimationType.Idle);
            }
            transform.Translate(_movementDirection.normalized * _XmovementSpeed * _movementRandomizer * Time.deltaTime);
        }
        
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
        
        private void Attack()
        {
            if (!_canAttack) return;
            if (_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
                return;
            }
            _attackCooldown = _baseCooldown;
            int select = Random.Range(0,2);
            AnimationType attack = select == 1 ? AnimationType.Punch : AnimationType.Kick;
            _animationHandler.Request(attack);  // CHANGED
            //Debug.Log($"Enemy {_enemyData.Name} attacking player");
        }

        // ADDED: These three methods
        private void DisableFistAndFoot()
        {
            if (_fist != null)
            {
                _fist.gameObject.SetActive(false);
                //Debug.Log($"Enemy {name} FIST DISABLED at {Time.time}");
            }

            if (_foot != null)
            {
                _foot.gameObject.SetActive(false);
                //Debug.Log($"Enemy {name} FOOT DISABLED at {Time.time}");
            }
        }

        public void ActivateFist()
        {
            _fist.Activate();
            /*
            if (_fist != null && !_fist.gameObject.activeSelf)
            {
                _fist.gameObject.SetActive(true);
                //Debug.Log($"Enemy {name} FIST ACTIVATED at {Time.time}");
            }*/
        }

        public void ActivateFoot()
        {
            _foot.Activate();
            /*
            if (_foot != null && !_foot.gameObject.activeSelf)
            {
                _foot.gameObject.SetActive(true);
                //Debug.Log($"Enemy {name} FOOT ACTIVATED at {Time.time}");
            }*/
        }
        private void OnDeath()
        {
            if(Random.Range(0,1f)<0.4f) RandomLootService.RequestLoot(transform.position);
            GameManager.AddScore(_enemyData.ScoreYield * _tier.TierLevel);
            StartCoroutine(KnockBack());
        }
        public EnemyTier GetTier()
        {
            return new EnemyTier(_tier.TierLevel);
        }

        private IEnumerator KnockBack()
        {
            float yPos = _rb.position.y;
            Vector2 force = new Vector2(-transform.localScale.x * 5f, 10f);
            bool animationDone = false;
            _animationHandler.Request(AnimationType.Death, onAnimationEnd: () => animationDone = true );
            _blinker.BlinkDeath();
            _rb.AddForce(force, ForceMode2D.Impulse);
            _rb.gravityScale = 5f;
            bool notBack = true;
            while (notBack)
            {
                yield return new WaitForEndOfFrame();
                if (_rb.position.y < yPos)
                {
                    notBack = false;
                }
            }
            _rb.gravityScale = 0f;
            _rb.linearVelocity = Vector2.zero;
            _rb.position = new Vector2(_rb.position.x, yPos);
            while (!animationDone)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSecondsRealtime(2f);
            gameObject.SetActive(false);
        }
    }

}