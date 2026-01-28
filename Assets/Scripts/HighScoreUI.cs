using TMPro;
using UnityEngine;

namespace UrbanNinja
{
    public class HighScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_highScoreText;

        private void Awake()
        {
            m_highScoreText.text = GameManager.GetHighScoreText();
        }
    }

}