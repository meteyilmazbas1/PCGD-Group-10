using TMPro;
using UnityEngine;
using System.Collections;

namespace UrbanNinja
{
    /// <summary>
    /// Displays the combo counter and multiplier on screen.
    /// Shows combo count, tier name, and multiplier with visual feedback.
    /// </summary>
    public class ComboUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _comboCountText;
        [SerializeField] private TextMeshProUGUI _comboTierText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        
        [Header("Animation Settings")]
        [SerializeField] private float _punchScale = 1.3f;
        [SerializeField] private float _punchDuration = 0.15f;
        [SerializeField] private float _fadeOutDelay = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.3f;
        
        [Header("Container")]
        [SerializeField] private GameObject _comboContainer;
        
        private Vector3 _originalScale;
        private Coroutine _scaleCoroutine;
        private Coroutine _fadeCoroutine;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            // Get or create canvas group for fading
            _canvasGroup = _comboContainer?.GetComponent<CanvasGroup>();
            if (_canvasGroup == null && _comboContainer != null)
            {
                _canvasGroup = _comboContainer.AddComponent<CanvasGroup>();
            }
            
            // Store original scale
            if (_comboContainer != null)
            {
                _originalScale = _comboContainer.transform.localScale;
            }
            
            // Hide initially
            HideCombo();
        }
        
        private void OnEnable()
        {
            // Subscribe to combo events
            ComboManager.OnComboChanged += OnComboChanged;
            ComboManager.OnComboReset += OnComboReset;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from combo events
            ComboManager.OnComboChanged -= OnComboChanged;
            ComboManager.OnComboReset -= OnComboReset;
        }
        
        /// <summary>
        /// Called when combo changes (hit registered).
        /// </summary>
        private void OnComboChanged(int comboCount, float multiplier)
        {
            // Show container
            ShowCombo();
            
            // Update texts
            UpdateComboDisplay(comboCount, multiplier);
            
            // Play punch animation
            PlayPunchAnimation();
            
            // Cancel any pending fade out
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }
            
            // Reset alpha
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
            }
        }
        
        /// <summary>
        /// Called when combo resets (timer expired).
        /// </summary>
        private void OnComboReset()
        {
            // Start fade out animation
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeOutAndHide());
        }
        
        /// <summary>
        /// Update all combo display elements.
        /// </summary>
        private void UpdateComboDisplay(int comboCount, float multiplier)
        {
            // Update combo count
            if (_comboCountText != null)
            {
                _comboCountText.text = $"{comboCount}";
                _comboCountText.color = ComboManager.Instance?.GetComboColor() ?? Color.white;
            }
            
            // Update tier text
            if (_comboTierText != null)
            {
                string tierName = ComboManager.Instance?.GetComboTierName() ?? "";
                _comboTierText.text = tierName;
                _comboTierText.color = ComboManager.Instance?.GetComboColor() ?? Color.white;
                _comboTierText.gameObject.SetActive(!string.IsNullOrEmpty(tierName));
            }
            
            // Update multiplier text
            if (_multiplierText != null)
            {
                if (multiplier > 1f)
                {
                    _multiplierText.text = $"x{multiplier:F1}";
                    _multiplierText.color = ComboManager.Instance?.GetComboColor() ?? Color.white;
                    _multiplierText.gameObject.SetActive(true);
                }
                else
                {
                    _multiplierText.gameObject.SetActive(false);
                }
            }
        }
        
        /// <summary>
        /// Play scale punch animation for visual feedback.
        /// </summary>
        private void PlayPunchAnimation()
        {
            if (_comboContainer == null) return;
            
            if (_scaleCoroutine != null)
            {
                StopCoroutine(_scaleCoroutine);
            }
            _scaleCoroutine = StartCoroutine(PunchScaleAnimation());
        }
        
        private IEnumerator PunchScaleAnimation()
        {
            Transform containerTransform = _comboContainer.transform;
            Vector3 targetScale = _originalScale * _punchScale;
            float elapsed = 0f;
            float halfDuration = _punchDuration / 2f;
            
            // Scale up
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                containerTransform.localScale = Vector3.Lerp(_originalScale, targetScale, t);
                yield return null;
            }
            
            // Scale down
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                containerTransform.localScale = Vector3.Lerp(targetScale, _originalScale, t);
                yield return null;
            }
            
            containerTransform.localScale = _originalScale;
            _scaleCoroutine = null;
        }
        
        private IEnumerator FadeOutAndHide()
        {
            // Wait before starting fade
            yield return new WaitForSeconds(_fadeOutDelay);
            
            if (_canvasGroup != null)
            {
                float elapsed = 0f;
                float startAlpha = _canvasGroup.alpha;
                
                while (elapsed < _fadeOutDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / _fadeOutDuration;
                    _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
                    yield return null;
                }
                
                _canvasGroup.alpha = 0f;
            }
            
            HideCombo();
            _fadeCoroutine = null;
        }
        
        private void ShowCombo()
        {
            if (_comboContainer != null)
            {
                _comboContainer.SetActive(true);
            }
        }
        
        private void HideCombo()
        {
            if (_comboContainer != null)
            {
                _comboContainer.SetActive(false);
            }
        }
    }
}
