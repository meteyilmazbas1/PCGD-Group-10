
using System;
using UnityEngine;
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
            backButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                SoundManager.Instance.PlayButtonClick();
            });

            musicToggle.onValueChanged.AddListener(MusicToggleAction);
            soundToggle.onValueChanged.AddListener(SoundToggleAction);

            controlsButton.onClick.AddListener(ControlButtonAction);

            CheckValues();

            controlsBackButton.onClick.AddListener(() =>
            {
                controlsPanel.SetActive(false);
                SoundManager.Instance.PlayButtonClick();
            });
            controlsPanel.SetActive(false);
        }

        void OnEnable()
        {
            CheckValues();
        }

        void CheckValues()
        {
            if (SoundManager.Instance == null) { return; }
            musicToggle.isOn = SoundManager.Instance.IsMusicOn;
            soundToggle.isOn = SoundManager.Instance.IsSFXOn;
        }

        private void MusicToggleAction(bool arg0)
        {
            print("Music Toggled");
            SoundManager.Instance.ToggleMusic(arg0);
            //GlobalEvents.SendMusicToggle(arg0);
        }

        private void SoundToggleAction(bool arg0)
        {
            print("SoundToggled   " + arg0);
            //GlobalEvents.SendSFXToggle(arg0);
        }

        private void ControlButtonAction()
        {
            controlsPanel.SetActive(true);
            SoundManager.Instance.PlayButtonClick();
        }
    }

}