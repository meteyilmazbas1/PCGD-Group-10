using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button playButton;
        [SerializeField] Button optionButton;
        [SerializeField] Button creditButton;
        [SerializeField] Button quitButton;
        [Header("Modal Prefabs")]
        [SerializeField] GameObject optionPanelPrefab;
        [SerializeField] GameObject creditPanelPrefab;
        [SerializeField] GameObject quitPanelPrefab;
        [Header("Modals")]
        [SerializeField] OptionsUI optionsUI;
        [SerializeField] CreditsUI creditUI;
        [SerializeField] QuitUI quitUI;
        [SerializeField] private GameObject _menuPanel; //Added this so it can also be disabled


        void Awake()
        {
            playButton.onClick.AddListener(PlayButtonAction);
            optionButton.onClick.AddListener(OptionButtonAction);
            creditButton.onClick.AddListener(CreditButtonAction);
            quitButton.onClick.AddListener(QuitButtonAction);

            optionsUI = Instantiate(optionPanelPrefab, null).GetComponent<OptionsUI>();
            creditUI = Instantiate(creditPanelPrefab, null).GetComponent<CreditsUI>();
            quitUI = Instantiate(quitPanelPrefab, null).GetComponent<QuitUI>();

            optionsUI.gameObject.SetActive(false);
            creditUI.gameObject.SetActive(false);
            quitUI.gameObject.SetActive(false);
        }

        void PlayButtonAction()
        {
            //TODO: go to game scene
            SceneNavigationManager.Instance.LoadScene(Scenes.NameSelection);
            SoundManager.Instance.PlayButtonClick();
        }

        void OptionButtonAction()
        {
            _menuPanel.SetActive(false);
            optionsUI.gameObject.SetActive(true);
            SoundManager.Instance.PlayButtonClick();
        }

        void CreditButtonAction()
        {
            _menuPanel.SetActive(false);
            creditUI.gameObject.SetActive(true);
            SoundManager.Instance.PlayButtonClick();
        }

        void QuitButtonAction()
        {
            _menuPanel.SetActive(false);
            quitUI.gameObject.SetActive(true);
            SoundManager.Instance.PlayButtonClick();
        }

        private void FixedUpdate()
        {
            if (optionsUI.gameObject.activeInHierarchy || quitUI.gameObject.activeInHierarchy || creditUI.gameObject.activeInHierarchy)
            {
                return;
            }
            if (!_menuPanel.gameObject.activeInHierarchy)
            {
                _menuPanel.gameObject.SetActive(true);
            }
        }
    }

}
