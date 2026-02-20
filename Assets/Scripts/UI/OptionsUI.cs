
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class OptionsUI : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] Toggle musicToggle;
        [Header("Sound")]
        [SerializeField] Toggle soundToggle;
        [Header("BackButton")]
        [SerializeField] Button backButton;
        [Header("Controls Button")]
        [SerializeField] Button controlsButton;

        [Header("Controls Panel")]
        [SerializeField] GameObject controlsPanel;
        [SerializeField] Button controlsBackButton;

        void Awake()
        {
            backButton.onClick.AddListener(BackButtonAction);

            musicToggle.onValueChanged.AddListener(MusicToggleAction);
            soundToggle.onValueChanged.AddListener(SoundToggleAction);

            controlsButton.onClick.AddListener(ControlButtonAction);
            controlsBackButton.onClick.AddListener(ControlsPanelBackButtonAction);

            CheckValues();

            controlsPanel.SetActive(false);
        }

        void OnEnable()
        {
            CheckValues();
        }

        void Start()
        {
             SoundManager.Instance.OnMusicToggle += SoundManager_OnMusicToggle;
            SoundManager.Instance.OnSFXToggle += SoundManager_OnSFXToggle;
        }

        private void SoundManager_OnMusicToggle(object sender, SoundManager.OnMusicToggleEventArgs e)
        {
            //musicToggle.isOn = e.IsMusicOn;
        }

        private void SoundManager_OnSFXToggle(object sender, SoundManager.OnSFXToggleEventArgs e)
        {
            //soundToggle.isOn = e.IsSFXOn;
        }


        void OnDestroy()
        {
            SoundManager.Instance.OnMusicToggle -= SoundManager_OnMusicToggle;
            SoundManager.Instance.OnSFXToggle -= SoundManager_OnSFXToggle;
        }

        void CheckValues()
        {
            EventSystem.current.SetSelectedGameObject(backButton.gameObject);

            if (SoundManager.Instance == null) { return; }
            musicToggle.isOn = SoundManager.Instance.IsMusicOn;
            soundToggle.isOn = SoundManager.Instance.IsSFXOn;
        }

        private void MusicToggleAction(bool arg0)
        {
            SoundManager.Instance.ToggleMusic(arg0);
        }

        private void SoundToggleAction(bool arg0)
        {
            SoundManager.Instance.ToggleSFX(arg0);
        }

        private void ControlButtonAction()
        {
            EventSystem.current.SetSelectedGameObject(controlsBackButton.gameObject);
            controlsPanel.SetActive(true);
            SoundManager.Instance.PlayButtonClick();
        }

        private void ControlsPanelBackButtonAction()
        {
            EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
            controlsPanel.SetActive(false);
            SoundManager.Instance.PlayButtonClick();
        }

        private void BackButtonAction()
        {
            gameObject.SetActive(false);
            SoundManager.Instance.PlayButtonClick();
        }

    }
}