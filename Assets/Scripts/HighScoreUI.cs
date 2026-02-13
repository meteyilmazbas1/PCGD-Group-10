
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace UrbanNinja
{
    public class HighScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_highScoreNamesText;
        [SerializeField] TextMeshProUGUI m_highScorePointsText;

        private void Awake()
        {
            StringBuilder sbNames = new StringBuilder();
            StringBuilder sbScores = new StringBuilder();
            List<HighScoreManager.HighScoreEntry> highScoreEntries = GameManager.GetHighscore();
            int counter = 1;
            foreach (HighScoreManager.HighScoreEntry entry in highScoreEntries)
            {
                sbNames.Append(counter.ToString()+". "+entry.Name+"\n");
                sbScores.Append(entry.Score.ToString() + "\n");
                counter++;
                if (counter > 10) break;
            }
            m_highScoreNamesText.text = sbNames.ToString();
            m_highScorePointsText.text = sbScores.ToString();
        }
    }

}