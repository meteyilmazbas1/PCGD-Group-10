using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] Button restartButton;
        [SerializeField] Button mainMenuButton;

        [SerializeField] Image backgroundImage;

        [SerializeField] TMP_Text titleText;

        [SerializeField] TMP_Text subtitleText;

        [Header("BG Sprites")]
        [SerializeField] Sprite bgGameOver;
        [SerializeField] Sprite bgGameSuccess;

        [Header("Titles and Subtitles")]
        [SerializeField] string gameOverTitle = "Game Over";
        [SerializeField] string gameSuccessTitle = "You Win!";
        [SerializeField] string gameOverSubtitle = "Better luck next time.";
        [SerializeField] string gameSuccessSubtitle = "Congratulations on your victory!";

        void Awake()
        {
            restartButton.onClick.AddListener(RestartButtonAction);
            mainMenuButton.onClick.AddListener(MainMenuButtonAction);
        }

        void RestartButtonAction()
        {
            SoundManager.Instance.PlayButtonClick();
            Time.timeScale = 1;
            SceneNavigationManager.Instance.LoadScene(Scenes.GameScene);
        }

        void MainMenuButtonAction()
        {
            SoundManager.Instance.PlayButtonClick();
            Time.timeScale = 1;
            SceneNavigationManager.Instance.LoadScene(Scenes.MainMenu);
        }

        public void SetGameOverUI(bool isSuccess)
        {
            if (isSuccess)
            {
                backgroundImage.sprite = bgGameSuccess;
                titleText.text = gameSuccessTitle;
                subtitleText.text = gameSuccessSubtitle;
                subtitleText.color = Color.green;
            }
            else
            {
                backgroundImage.sprite = bgGameOver;
                titleText.text = gameOverTitle;
                subtitleText.text = gameOverSubtitle;
                subtitleText.color = Color.red;

                Color bgColor = backgroundImage.color;
                bgColor.a = 0.9803922f;
                backgroundImage.color = bgColor;
            }

            //GlobalEvents.SendGameFinished(isSuccess);
        }
    }

}