using TMPro;
using UnityEngine;
using System.Collections;

namespace UrbanNinja
{
    /// <summary>
    /// Displays temporary pickup messages (e.g., "+3 HP") on screen.
    /// </summary>
    public class PickupMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _displayDuration = 2f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _moveSpeed = 50f; // Pixels per second upward
        [SerializeField] private Color _healColor = Color.green;
        [SerializeField] private Color _defaultColor = Color.white;

        private static PickupMessageUI _instance;
        public static PickupMessageUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PickupMessageUI>();
                }
                return _instance;
            }
        }

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

            // Auto-find message text if not assigned
            if (_messageText == null)
            {
                _messageText = GetComponentInChildren<TextMeshProUGUI>();
                if (_messageText == null)
                {
                    Debug.LogWarning("PickupMessageUI: No TextMeshProUGUI found. Creating one...");
                    CreateMessageText();
                }
            }

            // Hide message initially
            if (_messageText != null)
            {
                _messageText.gameObject.SetActive(false);
            }
        }

        private void CreateMessageText()
        {
            GameObject textObj = new GameObject("PickupMessageText");
            textObj.transform.SetParent(transform);
            
            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(0, 100); // Center, slightly above
            rectTransform.sizeDelta = new Vector2(300, 50);

            _messageText = textObj.AddComponent<TextMeshProUGUI>();
            _messageText.text = "+3 HP";
            _messageText.fontSize = 36;
            _messageText.alignment = TextAlignmentOptions.Center;
            _messageText.color = _healColor;
            _messageText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Display a pickup message (e.g., "+3 HP").
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="color">Optional color for the message. Defaults to heal color.</param>
        public void ShowMessage(string message, Color? color = null)
        {
            if (_messageText == null)
            {
                Debug.LogError("PickupMessageUI: _messageText is null! Cannot show message.");
                return;
            }

            _messageText.text = message;
            _messageText.color = color ?? _healColor;
            _messageText.gameObject.SetActive(true);

            // Stop any existing coroutine
            StopAllCoroutines();
            StartCoroutine(AnimateMessage());
        }

        /// <summary>
        /// Display a health pickup message with default formatting.
        /// </summary>
        /// <param name="healAmount">Amount of health restored.</param>
        public void ShowHealMessage(int healAmount)
        {
            ShowMessage($"+{healAmount} HP", _healColor);
        }

        private IEnumerator AnimateMessage()
        {
            if (_messageText == null) yield break;

            RectTransform rectTransform = _messageText.rectTransform;
            Vector2 startPosition = rectTransform.anchoredPosition;
            Color startColor = _messageText.color;
            float elapsed = 0f;

            // Move up and fade out
            while (elapsed < _displayDuration)
            {
                elapsed += Time.deltaTime;
                
                // Move upward
                float moveAmount = _moveSpeed * Time.deltaTime;
                rectTransform.anchoredPosition = new Vector2(
                    startPosition.x,
                    startPosition.y + (moveAmount * elapsed)
                );

                // Fade out in the last fadeOutDuration seconds
                if (elapsed >= _displayDuration - _fadeOutDuration)
                {
                    float fadeProgress = (elapsed - (_displayDuration - _fadeOutDuration)) / _fadeOutDuration;
                    startColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
                    _messageText.color = startColor;
                }

                yield return null;
            }

            // Hide message
            _messageText.gameObject.SetActive(false);
            
            // Reset position and color
            rectTransform.anchoredPosition = startPosition;
            startColor.a = 1f;
            _messageText.color = startColor;
        }
    }
}
