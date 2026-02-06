
using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerPrefab;

        private static GameManager _instance;
        private static PlayerController s_player;
        private static HighScoreManager s_highScoreManager;
        public delegate void ScoreChanged(int score);
        public static ScoreChanged OnScoreChanged;
        public static PlayerController GetPlayerController() { return s_player; }
        public static void SetPlayerController(PlayerController player) 
        {
            //Debug.Log("SET PLAYER");
            s_player = player; 
        }

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            if (_instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Init();
            }
        }
        private void Init()
        {
            s_highScoreManager = new HighScoreManager();
        }

        static string currentPlayer;
        static int currentScore;
        static int currentRank = -1;
        public static void StartNewGame(string playerName)
        {
            currentPlayer = playerName;
            currentScore = 0;
            currentRank = -1;
        }
        public static void EndRound()
        {
            currentRank = s_highScoreManager.SendScore(currentPlayer, currentScore);
        }
        public static string GetCurrentPlayerName()
        {
            return currentPlayer;
        }
        public static List<HighScoreManager.HighScoreEntry> GetHighscore()
        {
            return s_highScoreManager.GetHighScoresList();
        }
        public static void AddScore(int baseScore)
        {
            // Apply combo multiplier if ComboManager exists
            float multiplier = 1f;
            if (ComboManager.Instance != null)
            {
                multiplier = ComboManager.Instance.GetMultiplier();
            }
            
            int finalScore = Mathf.RoundToInt(baseScore * multiplier);
            currentScore += finalScore;
            OnScoreChanged?.Invoke(currentScore);
            
            //Debug.Log($"Score: +{finalScore} (base: {baseScore} x {multiplier}) = Total: {currentScore}");
        }
        
        public static int GetCurrentScore()
        {
            return currentScore;
        }
       
        public void SpawnPlayer()
        {
            if (s_player != null)
            {
                Debug.LogWarning("GameManager: Player already exists. Not spawning new player.");
                return;
            }
            
            if (_playerPrefab == null)
            {
                Debug.LogError("GameManager: _playerPrefab is not assigned! Cannot spawn player.");
                return;
            }
            
            s_player = Instantiate<PlayerController>(_playerPrefab);
            Debug.Log("GameManager: Player spawned successfully.");
        }
    }
}
