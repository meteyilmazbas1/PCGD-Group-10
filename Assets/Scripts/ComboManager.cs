using UnityEngine;

namespace UrbanNinja
{
    /// <summary>
    /// Manages the combo system for the game.
    /// Tracks consecutive hits and provides score multipliers.
    /// </summary>
    public class ComboManager : MonoBehaviour
    {
        #region Singleton
        private static ComboManager _instance;
        public static ComboManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ComboManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Settings
        [Header("Combo Settings")]
        [SerializeField] private float _comboResetTime = 2f;
        
        [Header("Multiplier Thresholds")]
        [SerializeField] private int _tier1Threshold = 5;   // 1.5x
        [SerializeField] private int _tier2Threshold = 10;  // 2.0x
        [SerializeField] private int _tier3Threshold = 15;  // 2.5x
        [SerializeField] private int _tier4Threshold = 20;  // 3.0x
        #endregion

        #region State
        private int _currentCombo;
        private float _comboTimer;
        private bool _comboActive;
        #endregion

        #region Events
        public delegate void ComboChanged(int comboCount, float multiplier);
        public static event ComboChanged OnComboChanged;
        
        public delegate void ComboReset();
        public static event ComboReset OnComboReset;
        #endregion

        #region Properties
        public int CurrentCombo => _currentCombo;
        public bool IsComboActive => _comboActive && _currentCombo > 0;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (_comboActive)
            {
                _comboTimer -= Time.deltaTime;
                
                if (_comboTimer <= 0f)
                {
                    ResetCombo();
                }
            }
        }

        private void OnEnable()
        {
            // Reset combo when a new game starts
            ResetCombo();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Register a successful hit. Call this when the player deals damage.
        /// </summary>
        public void RegisterHit()
        {
            _currentCombo++;
            _comboTimer = _comboResetTime;
            _comboActive = true;
            
            float multiplier = GetMultiplier();
            OnComboChanged?.Invoke(_currentCombo, multiplier);
            
            //Debug.Log($"Combo: {_currentCombo}x (Multiplier: {multiplier})");
        }

        /// <summary>
        /// Get the current score multiplier based on combo count.
        /// </summary>
        /// <returns>Score multiplier (1.0 to 3.0)</returns>
        public float GetMultiplier()
        {
            if (_currentCombo >= _tier4Threshold) return 3.0f;
            if (_currentCombo >= _tier3Threshold) return 2.5f;
            if (_currentCombo >= _tier2Threshold) return 2.0f;
            if (_currentCombo >= _tier1Threshold) return 1.5f;
            return 1.0f;
        }

        /// <summary>
        /// Reset the combo counter. Called when timer expires or player takes damage.
        /// </summary>
        public void ResetCombo()
        {
            if (_currentCombo > 0)
            {
                //Debug.Log($"Combo Reset! Final combo was: {_currentCombo}");
                OnComboReset?.Invoke();
            }
            
            _currentCombo = 0;
            _comboTimer = 0f;
            _comboActive = false;
        }

        /// <summary>
        /// Get the current combo tier name for UI display.
        /// </summary>
        /// <returns>Tier name string</returns>
        public string GetComboTierName()
        {
            if (_currentCombo >= _tier4Threshold) return "ULTRA!";
            if (_currentCombo >= _tier3Threshold) return "SUPER!";
            if (_currentCombo >= _tier2Threshold) return "GREAT!";
            if (_currentCombo >= _tier1Threshold) return "NICE!";
            return "";
        }

        /// <summary>
        /// Get a color based on current combo tier for UI.
        /// </summary>
        /// <returns>Color for the combo display</returns>
        public Color GetComboColor()
        {
            if (_currentCombo >= _tier4Threshold) return new Color(1f, 0.2f, 0.2f);    // Red
            if (_currentCombo >= _tier3Threshold) return new Color(1f, 0.5f, 0f);      // Orange
            if (_currentCombo >= _tier2Threshold) return new Color(1f, 0.8f, 0f);      // Yellow
            if (_currentCombo >= _tier1Threshold) return new Color(0.5f, 1f, 0.5f);    // Light Green
            return Color.white;
        }
        #endregion
    }
}
