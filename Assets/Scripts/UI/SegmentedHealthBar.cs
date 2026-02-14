
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class SegmentedHealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _segmentPrefab;
        private HorizontalLayoutGroup _segmentParent;

        private List<GameObject> _segments = new();
        private Health _playerHealth;
        private void OnEnable()
        {

        }
        private void OnDisable()
        {
            if (_playerHealth == null) return;
            _playerHealth.OnHealthChangedEvent -= OnHealthUpdate;
        }
        private void Start()
        {
            _segmentParent = GetComponent<HorizontalLayoutGroup>();
            
            var player = GameManager.GetPlayerController();
            if (player == null)
            {
                Debug.LogWarning("SegmentedHealthBar: Player not found yet.");
                return;
            }
            
            _playerHealth = player.GetPlayerHealth();
            if (_playerHealth == null)
            {
                Debug.LogWarning("SegmentedHealthBar: Player Health component not found.");
                return;
            }
            
            Setup(_playerHealth.MaxHealth);
            _playerHealth.OnHealthChangedEvent += OnHealthUpdate;
        }
        private void Setup(int healthAmount)
        {
            for (int i = 0; i < healthAmount; i++)
            {
                _segments.Add(Instantiate(_segmentPrefab, _segmentParent.transform));
            }
        }
        private void OnHealthUpdate(int health, bool isDamage = false)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                bool active = i < health;
                _segments[i].SetActive(active);
            }
        }
    }

}
