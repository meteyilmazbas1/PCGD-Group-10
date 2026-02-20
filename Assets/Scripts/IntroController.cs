using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class IntroController : MonoBehaviour
    {
        public event EventHandler OnIntroStarted;
        public event EventHandler OnIntroFinished;

        [SerializeField] Image m_overlayerImage;
        [SerializeField] Image m_introImage;
        [SerializeField] float m_fadeRate;
        IEnumerator Start()
        {
            OnIntroStarted?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;

            yield return Fade(0f);
            yield return new WaitForSecondsRealtime(5f);
            yield return Fade(1f);

            gameObject.SetActive(false);
            //StartCoroutine(IntroSequence());
        }
        /*private IEnumerator IntroSequence()
        {
            yield return Fade(0f);
            yield return new WaitForSecondsRealtime(5f);
            yield return Fade(1f);
            
            gameObject.SetActive(false);
        }*/
        private void OnDisable()
        {
            Time.timeScale = 1f;
            OnIntroFinished?.Invoke(this, EventArgs.Empty);
        }
        private IEnumerator Fade(float targetAlpha)
        {
            float alpha = targetAlpha > 0 ? 0f : 1f;
            while (Mathf.Abs(alpha - targetAlpha) > 0.01f)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (targetAlpha < 1)
                {
                    alpha = Mathf.Clamp(m_overlayerImage.color.a - 0.1f * m_fadeRate, 0f, 1f);
                }
                else
                {
                    alpha = Mathf.Clamp(m_overlayerImage.color.a + 0.1f * m_fadeRate, 0f, 1f);
                }

                Color color = m_overlayerImage.color;
                color.a = alpha;
                m_overlayerImage.color = color;
            }
        }
    }
}
