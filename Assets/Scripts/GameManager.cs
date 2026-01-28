
using UnityEngine;

namespace UrbanNinja
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerPrefab;

        private static GameManager _instance;
        private static PlayerController s_player;
        private static HighScoreManager s_highScoreManager;
        public static PlayerController GetPlayerController() { return s_player; }
        public static void SetPlayerController(PlayerController player) { s_player = player; }

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
        public static string GetHighScoreText()
        {
            return s_highScoreManager.GetHighScoreString();
        }
        public static void AddScore(int score)
        {
            currentScore += score;
        }
        /// <summary>
        /// TODO: Maybe we need this
        /// </summary>
        public void SpawnPlayer()
        {
            s_player = Instantiate<PlayerController>(_playerPrefab);
        }
    }
}
