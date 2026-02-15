using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UrbanNinja
{
    /// <summary>
    /// Displays temporary pickup messages (e.g., "+3 HP") on screen.
    /// </summary>
    public class PickupMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _prefab;
        [SerializeField] private List<TextMeshProUGUI> _messageTextPool;
        [SerializeField] private float _displayDuration = 2f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _moveSpeed = 50f; // Pixels per second upward
        [SerializeField] private Color _healColor = Color.green;
        [SerializeField] private Color _scoreColor = new Color(0.8f, .6f, .5f);
        [SerializeField] private Color _defaultColor = Color.white;

        private static PickupMessageUI _instance;
        public static PickupMessageUI Instance => _instance;
        private Dictionary<GameObject, Coroutine> _coroutinesDict = new Dictionary<GameObject, Coroutine>();
        private Canvas _canvas;

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
        private void Start()
        {
            InitPool();
        }
        private void InitPool()
        {
            _canvas = GetComponentInChildren<Canvas>();
            _messageTextPool = new List<TextMeshProUGUI>();
            for (int i = 0; i < 10; i++)
            {
                TextMeshProUGUI text = Instantiate<TextMeshProUGUI>(_prefab, transform.position,
                    Quaternion.identity, _canvas.transform as RectTransform);
                _messageTextPool.Add(text);
                text.gameObject.SetActive(false);
            }
        }
        private TextMeshProUGUI GetTextFromPool()
        {
            TextMeshProUGUI instance = _messageTextPool.Find(txt => !txt.isActiveAndEnabled);
            if(instance == null)
            {
                instance = Instantiate<TextMeshProUGUI>(_prefab, transform.position,
                    Quaternion.identity, transform as RectTransform);
                _messageTextPool.Add(instance);
            }
            instance.gameObject.SetActive(true);
            return instance;
        }
        /*
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
        }*/

        /// <summary>
        /// Display a pickup message (e.g., "+3 HP").
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="color">Optional color for the message. Defaults to heal color.</param>
        public void ShowMessage(string message, Color? color = null)
        {
            TextMeshProUGUI messageText = GetTextFromPool();

            messageText.text = message;
            messageText.color = color ?? _healColor;
            messageText.gameObject.SetActive(true);


            Coroutine coroutine = StartCoroutine(AnimateMessage(messageText));
            _coroutinesDict.Add(messageText.gameObject, coroutine);
        }

        /// <summary>
        /// Display a health pickup message with default formatting.
        /// </summary>
        /// <param name="healAmount">Amount of health restored.</param>
        public void ShowHealMessage(int healAmount, Transform requester=null)
        {
            if (requester != null) _requester = requester;
            ShowMessage($"+{healAmount} HP", _healColor);
        }
        /// <summary>
        /// Display a score pickup message with default formatting.
        /// </summary>
        /// <param name="amount">Amount of score.</param>
        public void ShowScoreMessage(int amount, Transform requester = null)
        {
            if (requester != null) _requester = requester;
            ShowMessage($"+{amount} score!", _scoreColor);
        }
        private Transform _requester; 
        /*
        private IEnumerator AnimateMessage()
        {
            if (_messageText == null) yield break;

            RectTransform rectTransform = _messageText.rectTransform;
            if(_requester != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform.parent as RectTransform, RectTransformUtility.WorldToScreenPoint(
                        Camera.main, _requester.position),
                    Camera.main, out Vector2 point);
                rectTransform.anchoredPosition = point;
            }
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

                yield return new WaitForEndOfFrame();
            }

            // Hide message
            _messageText.gameObject.SetActive(false);
            
            // Reset position and color
            rectTransform.anchoredPosition = startPosition;
            startColor.a = 1f;
            _messageText.color = startColor;
        }*/
        private IEnumerator AnimateMessage(TextMeshProUGUI messageText)
        {
            RectTransform rectTransform = messageText.rectTransform;
            if (_requester != null)
            {
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvas.transform as RectTransform, RectTransformUtility.WorldToScreenPoint(
                        Camera.main, _requester.position + Vector3.up),
                    _canvas.worldCamera, out Vector2 point);
                rectTransform.anchoredPosition = point;
            }
            Vector2 startPosition = rectTransform.anchoredPosition;
            Color startColor = messageText.color;
            float elapsed = 0f;

            // Move up and fade out
            while (elapsed < _displayDuration)
            {
                elapsed += Time.deltaTime;

                // Move upward
                float moveAmount = _moveSpeed * Time.deltaTime;
                
                rectTransform.anchoredPosition = new Vector2(startPosition.x,
                    rectTransform.anchoredPosition.y + moveAmount
                    );

                // Fade out in the last fadeOutDuration seconds
                startColor = FadeText(startColor, elapsed, messageText);

                yield return new WaitForEndOfFrame();
            }

            // Hide message
            messageText.gameObject.SetActive(false);

            // Reset position and color
            rectTransform.anchoredPosition = startPosition;
            startColor.a = 1f;
            messageText.color = startColor;
            _coroutinesDict.Remove(messageText.gameObject);
        }

        private Color FadeText(Color startColor, float elapsed, TextMeshProUGUI messageText)
        {
            if (elapsed >= _displayDuration - _fadeOutDuration)
            {
                float fadeProgress = (elapsed - (_displayDuration - _fadeOutDuration)) / _fadeOutDuration;
                startColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
                messageText.color = startColor;
            }

            return startColor;
        }
    }
}
