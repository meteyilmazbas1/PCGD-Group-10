using UnityEngine;

namespace UrbanNinja
{
    [CreateAssetMenu(menuName = "Urban Ninja/Music data")]
    public class MusicData : ScriptableObject
    {
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _inGameMusic;

        public AudioClip MenuMusic => _menuMusic;
        public AudioClip InGameMusic => _inGameMusic;
    }
}
