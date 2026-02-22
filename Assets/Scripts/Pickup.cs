using System.Collections;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour
    {
        public enum PickupType
        {
            Health,
            Score,
            Weapon
        }
        [SerializeField] private int _amount = 3;
        [SerializeField] private bool _useLayerCheck = true;
        [SerializeField] private int _playerLayer = 7; // Player layer index (Edit > Project Settings > Tags and Layers)
        [SerializeField] private AudioClip _pickupSound; // Optional: Sound to play when picked up
        [SerializeField] private GameObject _pickupEffect; // Optional: Particle effect or visual effect to spawn when picked up
        [SerializeField] private PickupType _pickupType;
        [SerializeField] private Weapon _weapon; //If this is a weapon pickup!
        [SerializeField] private AudioClip _dropSound; //If this is a weapon pickup!
        private Blinker _blinker;//If this is a weapon pickup!

        private Collider2D _collider;
        //private bool _isReDrop;
        private bool _taken;
        public PickupType Type => _pickupType;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
            _blinker = GetComponent<Blinker>();
        }
        private void OnBlinkEnd()
        {
            Destroy(gameObject);
        }
        private SpriteRenderer _spriteRenderer;
        private void OnEnable()
        {
            _taken = false;
            if (_pickupType == PickupType.Weapon) _collider.enabled = false;
            if(_blinker != null)
            {
                if(_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                }
                _blinker.SetBlinkEndCallback(OnBlinkEnd);
                _blinker.SetSpriteRenderer(_spriteRenderer);
                _blinker.DelayedBlinkerDeath();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandlePickingUp(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            HandlePickingUp(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            HandlePickingUp(collision);
        }
        private void HandlePickingUp(Collider2D collision)
        {
            IPickUpTaker pickUpTaker = collision.gameObject.GetComponent<IPickUpTaker>();
            if (pickUpTaker == null) return;
            if (!pickUpTaker.CanTake(this)) return;
            if (_taken) return;
            _taken = true;
            _collider.enabled = false;
            if (_pickupType == PickupType.Weapon)
            {
                Weapon weapon = Instantiate(_weapon, transform.position, Quaternion.identity);
                weapon.SetOwner(collision.gameObject);
                weapon.SetPickUpInstance(this);
                pickUpTaker.TakeWeapon(weapon);
                //_isReDrop = true;
                PlayPickupSound();
                gameObject.SetActive(false);
                return;
            }
            else if (_pickupType == PickupType.Health)
            {
                pickUpTaker.TakeHealth(_amount);
            }
            else if (_pickupType == PickupType.Score)
            {
                pickUpTaker.TakeScore(_amount);
            }
            ShowPickupMessage();
            PlayPickupSound();
            SpawnPickupEffect();
            gameObject.SetActive(false);
        }

        private void PlayPickupSound()
        {
            if (SoundManager.Instance != null)
            {
                if (_pickupSound != null)
                {
                    SoundManager.Instance.PlaySound(_pickupSound);
                }
                else
                {
                    // Fallback: Use button click sound if no pickup sound assigned
                    SoundManager.Instance.PlayButtonClick();
                }
            }
        }

        private void SpawnPickupEffect()
        {
            if (_pickupEffect != null)
            {
                GameObject effect = Instantiate(_pickupEffect, transform.position, Quaternion.identity);
                // Auto-destroy effect after 2 seconds (adjust as needed)
                Destroy(effect, 2f);
            }
        }

        private void ShowPickupMessage()
        {
            if (PickupMessageUI.Instance != null)
            {
                switch (_pickupType)
                {
                    case PickupType.Health:
                        PickupMessageUI.Instance.ShowHealMessage(_amount, transform);
                        break;
                    case PickupType.Score:
                        PickupMessageUI.Instance.ShowScoreMessage(_amount, transform);
                        break;
                }
            }
        }
        public void DropEffect()
        {
            //if (!_isReDrop) return;
            StartCoroutine(DropNBounce());
        }
        /// <summary>
        /// This routine makes the pickup bounce
        /// a few times.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DropNBounce()
        {
            Vector2 ground = transform.position - Vector3.up *.5f;
            int bounces = 4;
            Vector2 dir = Vector2.down;
            Vector2 velocity = new Vector2(0, 3f);
            yield return RandomBounceOff(ground.y);

            while (bounces > 0)
            {
                yield return new WaitForFixedUpdate();
                velocity = velocity +  Vector2.up * (-50f * Time.fixedDeltaTime);
                transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
                if (transform.position.y <= ground.y)
                {
                    bounces--;
                    velocity = -0.5f * velocity;
                    transform.position = new Vector2(transform.position.x, ground.y);
                    SoundManager.Instance.PlaySound(_dropSound);
                }
            }
            transform.position = new Vector2(transform.position.x, ground.y);
            _collider.enabled = true;
        }
        /// <summary>
        /// This routine will "throw" the pickup in a 
        /// random direction.
        /// </summary>
        /// <param name="groundY"></param>
        /// <returns></returns>
        private IEnumerator RandomBounceOff(float groundY)
        {
            _collider.enabled = false;
            float xVelocity = Random.Range(-1f, 1f) < 0 ? -1f: 1f;
            float yVelocity = 3f;
            Vector2 velocity = new Vector2(xVelocity, yVelocity);
            while(transform.position.y > groundY)
            {
                yield return new WaitForFixedUpdate();
                velocity = velocity + Vector2.down * (50 * Time.fixedDeltaTime);
                transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
                if (transform.position.y <= groundY)
                {
                    transform.position = new Vector2(transform.position.x, groundY);
                    SoundManager.Instance.PlaySound(_dropSound);
                    break;
                }
            }
            transform.position = new Vector2(transform.position.x, groundY);
        }
    }
}
