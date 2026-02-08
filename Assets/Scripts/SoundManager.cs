using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UrbanNinja
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        const string MusicPrefKey = "MusicOn";
        const string SFXPrefKey = "SFXOn";

        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [Header("Audio Clips")]
        [SerializeField] private AudioClip buttonClickClip;

        private MusicData _musicData;
        private Dictionary<int, AudioClip> _music;
        public bool IsMusicOn
        {
            get => PlayerPrefsBool.GetBool(MusicPrefKey);
            private set => PlayerPrefsBool.SetBool(MusicPrefKey, value);
        }

        public bool IsSFXOn
        {
            get => PlayerPrefsBool.GetBool(SFXPrefKey);
            private set => PlayerPrefsBool.SetBool(SFXPrefKey, value);
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            _musicData = Resources.Load<MusicData>("Data/data_music");
            _music = new Dictionary<int, AudioClip>();
            _music.Add(0, _musicData.MenuMusic);
            _music.Add(1, _musicData.MenuMusic);
            _music.Add(2, _musicData.InGameMusic);
            _music.Add(3, _musicData.MenuMusic);
            if (!PlayerPrefs.HasKey(MusicPrefKey))
                IsMusicOn = true;
            if (!PlayerPrefs.HasKey(SFXPrefKey))
                IsSFXOn = true;
            JukeBox(0);
        }

        void Start()
        {
            //GlobalEvents.onMusicToggle += ToggleMusic;
            //GlobalEvents.onSFXToggle += ToggleSFX;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        private void OnActiveSceneChanged(Scene scene1, Scene scene2)
        {
            JukeBox(scene2.buildIndex);
        }
        private int _activeSongIndex = -1;
        private void JukeBox(int next)
        {
            //Debug.Log($"Prev: {_activeSongIndex} Next: {next}");
            if (!IsMusicOn) return;
            if (_activeSongIndex < 0)
            {
                musicSource.clip = _music[0];
                _activeSongIndex = 0;
                musicSource.Play();
                return;
            }
            if (_activeSongIndex == 2)
            {
                musicSource.clip = _music[next];
                _activeSongIndex = next;
                musicSource.Play();
            }
            else if(next == 2)
            {
                musicSource.clip = _music[next];
                _activeSongIndex = next;
                musicSource.Play();
            }
            
        }

        public void ToggleMusic(bool isOn) => IsMusicOn = isOn;
        private void ToggleSFX(bool isOn) => IsSFXOn = isOn;


        void OnDestroy()
        {
            //GlobalEvents.onMusicToggle -= ToggleMusic;
            //GlobalEvents.onSFXToggle -= ToggleSFX;
        }

        public void PlaySound(AudioClip clip)
        {
            if (!IsSFXOn || clip == null) return;

            sfxSource.PlayOneShot(clip);
        }

        public void PlayButtonClick()
        {
            PlaySound(buttonClickClip);
        }
    }


    public static class PlayerPrefsBool
    {
        public static bool GetBool(string key)
            => PlayerPrefs.GetInt(key) == 1;

        public static void SetBool(string key, bool value)
            => PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}