using UnityEngine;
using UrbanNinja.Input;

namespace UrbanNinja
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private float _jumpImpulse = 0.1f;

        private GameplayInput _inputActions;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveVector;
        private Vector2 _left = new Vector2(-1, 1);
        private Vector2 _right = new Vector2(1, 1);
        private Collider2D _collider;
        private bool isGrounded;
        private float _jumpLevel;

        private void Awake()
        {
            GetReferences();
            InitInput();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            GameManager.SetPlayerController(this);
        }
        private void OnDisable()
        {
            _inputActions.Disable();
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
        /// Get the required component references.
        /// </summary>
        private void GetReferences()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
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
            isGrounded = false;
            _collider.enabled = false;
            _jumpLevel = _rigidbody2D.position.y;
            _rigidbody2D.linearVelocityY = _jumpImpulse;
            _rigidbody2D.gravityScale = 5f;
        }
        /// <summary>
        /// Handle movement according to the
        /// move vector and jump state of the player.
        /// </summary>
        private void Move()
        {

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
            else
            {
                _rigidbody2D.linearVelocity = _moveVector * _moveSpeed * Time.deltaTime;
            }
        }
        /// <summary>
        /// Callback for Punch input.
        /// </summary>
        private void Punch()
        {
            Debug.Log("PUNCH!");
        }
        /// <summary>
        /// Callback for Kick input.
        /// </summary>
        private void Kick()
        {
            Debug.Log("KICK!");
        }
        /// <summary>
        /// Check if the player can jump
        /// based on isGrounded.
        /// </summary>
        /// <returns>True if player can jump.</returns>
        private bool CanJump()
        {
            return isGrounded;
        }
    }

}