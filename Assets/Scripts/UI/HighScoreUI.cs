
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class HighScoreUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button m_menuButton;
        [SerializeField] private Button m_restartButton;
        [Header("High Score Display")]
        [SerializeField] TextMeshProUGUI m_highScoreNamesText;
        [SerializeField] TextMeshProUGUI m_highScorePointsText;

        private void Awake()
        {
            m_menuButton.onClick.AddListener(MainMenuButtonAction);
            m_restartButton.onClick.AddListener(RestartButtonAction);

            EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);
        }

        void Start()
        {
            ShowHighScores();
        }

        void ShowHighScores()
        {
            StringBuilder sbNames = new StringBuilder();
            StringBuilder sbScores = new StringBuilder();
            List<HighScoreManager.HighScoreEntry> highScoreEntries = GameManager.GetHighscore();
            int counter = 1;
            foreach (HighScoreManager.HighScoreEntry entry in highScoreEntries)
            {
                sbNames.Append(counter.ToString() + ". " + entry.Name + "\n");
                sbScores.Append(entry.Score.ToString() + "\n");
                counter++;
                if (counter > 10) break;
            }
            m_highScoreNamesText.text = sbNames.ToString();
            m_highScorePointsText.text = sbScores.ToString();
        }

        private void MainMenuButtonAction()
        {
            SceneNavigationManager.Instance.LoadScene(Scenes.MainMenu);
        }

        private void RestartButtonAction()
        {
            string name = GameManager.GetCurrentPlayerName();
            GameManager.StartNewGame(name);
            SceneNavigationManager.Instance.LoadScene(Scenes.GameScene);
        }
    }

}