using System.Collections;
using UnityEngine;

namespace UrbanNinja
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledSFX : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Coroutine _playAndDisableRoutine;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void Init(AudioClip clip)
        {
            _audioSource.clip = clip;
            if(_playAndDisableRoutine != null)
            {
                StopCoroutine(_playAndDisableRoutine);
            }
            _playAndDisableRoutine = StartCoroutine(PlayAndDisable());
        }
        private IEnumerator PlayAndDisable()
        {
            _audioSource.Play();
            while (_audioSource.isPlaying)
            {
                yield return null;
            }
            _playAndDisableRoutine = null;
            _audioSource.clip = null;
            gameObject.SetActive(false);
        }
    }

}