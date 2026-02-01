using UnityEngine;

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

            if (!PlayerPrefs.HasKey(MusicPrefKey))
                IsMusicOn = true;
            if (!PlayerPrefs.HasKey(SFXPrefKey))
                IsSFXOn = true;
        }

        void Start()
        {
            //GlobalEvents.onMusicToggle += ToggleMusic;
            //GlobalEvents.onSFXToggle += ToggleSFX;
        }


        private void ToggleMusic(bool isOn) => IsMusicOn = isOn;
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