
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
            s_highScoreManager.SendScore("James1", 100);
            s_highScoreManager.SendScore("James2", 10000);
            s_highScoreManager.SendScore("James3", 1000);
            Debug.Log("HIGH SCORES: ");
            Debug.Log(s_highScoreManager.GetHighScoreString());
        }


        public static void StartNewGame(string playerName)
        {
            //TODO
        }
        public static void EndRound()
        {
            //TODO: Add an entry to highscore
        }
        public void SpawnPlayer()
        {
            s_player = Instantiate<PlayerController>(_playerPrefab);
        }
    }
}
