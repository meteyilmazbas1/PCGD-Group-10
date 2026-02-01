using TMPro;
using UnityEngine;

namespace UrbanNinja
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerName;
        [SerializeField] TextMeshProUGUI _playerScore;

        private void OnEnable()
        {
            GameManager.OnScoreChanged += OnScoreUpdate;
        }
        private void OnDisable()
        {
            GameManager.OnScoreChanged -= OnScoreUpdate;
        }
        private void Start()
        {
            string name = GameManager.GetCurrentPlayerName();
            Debug.Log("PLAYER NAME: "+name);
            _playerName.text = name;
            _playerScore.text = "0";
        }
        private void OnScoreUpdate(int score)
        {
            _playerScore.text = score.ToString();
        }
    }
}
