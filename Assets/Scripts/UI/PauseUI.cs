using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class PauseUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button continueButton;
        [SerializeField] Button optionsButton;
        [SerializeField] Button backButton;

        [Header("OptionUI")]
        [SerializeField] GameObject optionsUIPrefab;
        [SerializeField] OptionsUI optionsUI;
        private void OnEnable()
        {
            //GlobalEvents.SendPauseGame(true);
        }
        void Awake()
        {
            continueButton.onClick.AddListener(ContinueButtonAction);
            optionsButton.onClick.AddListener(OptionsButtonAction);
            backButton.onClick.AddListener(BackButtonAction);

            optionsUI = Instantiate(optionsUIPrefab, null).GetComponent<OptionsUI>();
            optionsUI.gameObject.SetActive(false);
        }

        void ContinueButtonAction()
        {
            // Resume the game
            SoundManager.Instance.PlayButtonClick();
            //CursorManager.s_instance.ToggleCursor(false);
            Close();
        }

        void OptionsButtonAction()
        {
            // Open options menu
            SoundManager.Instance.PlayButtonClick();
            optionsUI.gameObject.SetActive(true);
        }

        void BackButtonAction()
        {
            // Go back to the main menu
            SoundManager.Instance.PlayButtonClick();
            Time.timeScale = 1;
            SceneNavigationManager.Instance.LoadScene(Scenes.MainMenu);
        }

        public void Close()
        {
            Time.timeScale = 1;
            //GlobalEvents.SendPauseGame(false);
            gameObject.SetActive(false);
        }
    }

}