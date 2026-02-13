using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField] Image m_overlayerImage;
        [SerializeField] Image m_introImage;
        [SerializeField] float m_fadeRate;
        void Start()
        {
            Time.timeScale = 0f;
            StartCoroutine(IntroSequence());
        }
        private IEnumerator IntroSequence()
        {
            yield return Fade(0f);
            yield return new WaitForSecondsRealtime(5f);
            yield return Fade(1f);
            
            gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
        private IEnumerator Fade(float targetAlpha)
        {
            float alpha = targetAlpha > 0 ? 0f : 1f;
            while (alpha != targetAlpha)
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
