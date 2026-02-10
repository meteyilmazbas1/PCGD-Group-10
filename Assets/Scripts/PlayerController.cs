using UnityEngine;
using UrbanNinja.Input;

namespace UrbanNinja
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private float _jumpImpulse = 0.1f;
        [SerializeField] private GameObject _fist;
        [SerializeField] private GameObject _foot;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _hurtSound;

        private GameplayInput _inputActions;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveVector;
        private Vector2 _left = new Vector2(-1, 1);
        private Vector2 _right = new Vector2(1, 1);
        private Collider2D _collider;
        private bool isGrounded;
        private float _jumpLevel;
        private AnimationHandler _animationHandler;

        private Health _playerHealth;

        public bool Alive => !_isDead;

        private void Awake()
        {
            GetReferences();
            InitInput();
            InitDamageDealers();
            DisableFistAndFoot();

            if (_playerHealth != null)
            {
                _playerHealth.SetMaxHealth(20);  // Player has 20 HP
                _playerHealth.OnDeathEvent += OnPlayerDeath;
                _playerHealth.OnHealthChangedEvent += OnHealthChanged;
            }
        }
        
        /// <summary>
        /// Initialize damage dealers with owner reference for combo tracking.
        /// </summary>
        private void InitDamageDealers()
        {
            if (_fist != null)
            {
                DamageDealer fistDealer = _fist.GetComponent<DamageDealer>();
                if (fistDealer != null) fistDealer.SetOwner(gameObject);
            }
            
            if (_foot != null)
            {
                DamageDealer footDealer = _foot.GetComponent<DamageDealer>();
                if (footDealer != null) footDealer.SetOwner(gameObject);
            }
        }

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                InitInput();
            }
            if (_inputActions != null)
        {
            _inputActions.Enable();
            }
            
            if (_playerHealth == null)
            {
                GetReferences();
            }
            
            GameManager.SetPlayerController(this);
        }
        private void OnDisable()
        {
            // Safely disable input actions
            if (_inputActions != null)
            {
                try
        {
            _inputActions.Disable();
                }
                catch
                {
                    // Input system may be destroyed already
                }
            }

            // Unsubscribe when disabled to prevent memory leaks
            // Note: -= operator is safe even if OnDeathEvent is null
            if (_playerHealth != null)
            {
                _playerHealth.OnDeathEvent -= OnPlayerDeath;
                _playerHealth.OnHealthChangedEvent -= OnHealthChanged;
            }
        }
        private void OnHealthChanged(int value, bool isDamage = false)
        {
            if (_isDead) return;
            if (isDamage)
            {
                _animationHandler.Request("damage");
            }
        }
        private void FixedUpdate()
        {
            Move();
        }
        /// <summary>
        /// Get position relative to jump level.
        /// </summary>
        /// <returns>Vector2 position based on the jump level.</returns>
        public Vector2 GetPositionRelativeToJump()
        {
            if (isGrounded) return transform.position;
            return new Vector2(transform.position.x, _jumpLevel);
        }

        /// <summary>
        /// Get the player's Health component.
        /// Used by UI elements like SegmentedHealthBar.
        /// </summary>
        /// <returns>The Health component attached to the player.</returns>
        public Health GetPlayerHealth()
        {
            return _playerHealth;
        }

        /// <summary>
        /// Get the required component references.
        /// </summary>
        private void GetReferences()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animationHandler = GetComponent<AnimationHandler>();
            _playerHealth = GetComponent<Health>();
        }

        /// <summary>
        /// Create a new GamePlayInput instance
        /// and subscribe to the callbacks.
        /// </summary>
        private void InitInput()
        {
            _inputActions = new GameplayInput();
            _inputActions.PlayerInput.move.performed += ctx =>
            {
                _moveVector = ctx.ReadValue<Vector2>();
                FlipTransform();
            };
            _inputActions.PlayerInput.move.canceled += ctx => _moveVector = Vector2.zero;
            _inputActions.PlayerInput.jump.performed += ctx => Jump();
            _inputActions.PlayerInput.punch.performed += ctx => Punch();
            _inputActions.PlayerInput.kick.performed += ctx => Kick();
        }
        /// <summary>
        /// Faces the transform to the direction of movement
        /// by flipping the transform based on _moveVector.x.
        /// </summary>
        private void FlipTransform()
        {
            if (_moveVector.x > 0)
            {
                transform.localScale = _right;
            }
            else
            {
                transform.localScale = _left;
            }
        }

        /// <summary>
        /// Callback for Jump input. Need to set isGrounded to false,
        /// register the level where jump begun for later reference,
        /// set rigidbody velocity and gravity scale.
        /// </summary>
        private void Jump()
        {
            if (!CanJump()) return;
            _animationHandler.Request("jump");
            isGrounded = false;
            _collider.enabled = false;
            _jumpLevel = _rigidbody2D.position.y;
            _rigidbody2D.linearVelocityY = _jumpImpulse;
            _rigidbody2D.gravityScale = 5f;

            if (_jumpSound != null) {
                SoundManager.Instance.PlaySound(_jumpSound);
            }
                
            //DEBUG DEATH
            //OnPlayerDeath();
        }
        /// <summary>
        /// Handle movement according to the
        /// move vector and jump state of the player.
        /// </summary>
        private void Move()
        {
            if (_isDead) return;
            if (!isGrounded && _rigidbody2D.position.y < _jumpLevel)
            {
                _collider.enabled = true;
                isGrounded = true;
                _rigidbody2D.gravityScale = 0f;
                _rigidbody2D.position = new Vector2(_rigidbody2D.position.x, _jumpLevel);
            }
            if (!isGrounded)
            {
                _rigidbody2D.linearVelocity = new Vector2(_moveVector.x * _moveSpeed * Time.deltaTime, _rigidbody2D.linearVelocity.y);
            }
            else if(!_movementBlocked)
            {
                DisableFistAndFoot();
                _rigidbody2D.linearVelocity = _moveVector * _moveSpeed * Time.deltaTime;
                if (isGrounded && _rigidbody2D.linearVelocity.magnitude > 0)
                {
                    _animationHandler.Request("walk");
                }
                else
                {
                    _animationHandler.Request("idle");
                }
            }
            else
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
            }
 
        }
        private bool _movementBlocked;
        /// <summary>
        /// Callback for Punch input.
        /// </summary>
        private void Punch()
        {
            if (_movementBlocked || !isGrounded) return;
            //Debug.Log("PUNCH!");
            _movementBlocked = true;
            _animationHandler.Request("punch", onAnimationEnd: UnBlockMovement);
        }
        /// <summary>
        /// Callback for Kick input.
        /// </summary>
        private void Kick()
        {
            if (_movementBlocked || !isGrounded) return;
            //Debug.Log("KICK!");
            _movementBlocked = true;
            _animationHandler.Request("kick", onAnimationEnd: UnBlockMovement);
        }
        /// <summary>
        /// Check if the player can jump
        /// based on isGrounded.
        /// </summary>
        /// <returns>True if player can jump.</returns>
        private bool CanJump()
        {
            return isGrounded && !_movementBlocked;
        }
        /// <summary>
        /// Movement is blocked during attacks.
        /// Unblocking also enables dealing damage again
        /// so the foot and fist must be disabled here
        /// for accidental damage.
        /// </summary>
        private void UnBlockMovement()
        {
            DisableFistAndFoot();
            _movementBlocked = false;
        }
        /// <summary>
        /// Disabling fist and foot also
        /// disables damage dealing.
        /// </summary>
        private void DisableFistAndFoot()
        {
            _fist.SetActive(false);
            _foot.SetActive(false);
        }

        /// <summary>
        /// This is method to be called from an
        /// animation event to time dealing damage
        /// correctly.
        /// </summary>
        public void ActivateFist()
        {
            //Debug.Log("Fist ACTIVE");
            _fist.SetActive(true);
        }
        /// <summary>
        /// This is method to be called from an
        /// animation event to time dealing damage
        /// correctly.
        /// </summary>
        public void ActivateFoot()
        {
            //Debug.Log("FOOT ACTIVE");
            _foot.SetActive(true);
        }
        private bool _isDead = false;
        private void OnPlayerDeath()
        {
            if (_isDead) return;
            Debug.Log("Player death");
            _isDead = true;
            _animationHandler.Request("death", onAnimationEnd: () =>
            {
                Debug.Log("Player death on animation end");
                GameManager.EndRound();
                SceneNavigationManager.Instance.LoadScene(Scenes.Highscore);
            });

        }
        public AudioClip HurtSound => _hurtSound;
    }

}
