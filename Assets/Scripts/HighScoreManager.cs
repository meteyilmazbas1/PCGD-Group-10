using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace UrbanNinja
{
    [System.Serializable]
    public class HighScoreManager
    {

        [System.Serializable]
        public class HighScoreEntry
        {
            public string Name;
            public int Score;
        }

        [System.Serializable]
        private class HighScoreList
        {
            /// <summary>
            /// This a wrapper class for HighScoreEntry list.
            /// The list will always remain sorted.
            /// </summary>
            public List<HighScoreEntry> _highScores = new List<HighScoreEntry>();
            public int Add(HighScoreEntry entry)
            {
                _highScores.Add(entry);
                _highScores.Sort(CompareScore);
                return _highScores.IndexOf(entry);
            }
            public IEnumerable<HighScoreEntry> GetHighScores()
            {
                return _highScores;
            }
            private int CompareScore(HighScoreEntry x, HighScoreEntry y)
            {
                if (x.Score > y.Score) return -1;
                if (x.Score == y.Score) return 0;
                if (x.Score < y.Score) return 1;
                return 0;
            }
        }
        const string HIGHSCORE_DATA_FILENAME = "urban_ninja_scoresheet.nnj";
        private HighScoreList _highScores;
        public HighScoreManager() 
        {
            if (File.Exists(GetPath()))
            {
                //TODO
                //Debug.Log("Highscore file found!");
                string json = File.ReadAllText(GetPath());
                _highScores = JsonUtility.FromJson<HighScoreList>(json);
            }
            else
            {
                //TODO
                //Debug.Log("Highscore not file found!");
                _highScores = new HighScoreList();
            }
        }
        private string GetPath()
        {
            string path = Path.Join(Application.persistentDataPath, HIGHSCORE_DATA_FILENAME);
            //Debug.Log("PATH: "+path);
            return path;
        }
        private void SaveToFile()
        {
            string json = JsonUtility.ToJson(_highScores);
            //Debug.Log("JSON: "+json);
            File.WriteAllText(GetPath(), json);
            //Debug.Log("Save Complete!");
        }
        private HighScoreEntry CreateNewEntry(string name, int score)
        {
            HighScoreEntry entry = new HighScoreEntry();
            entry.Score = score;
            entry.Name = name;
            return entry;
        }
        /// <summary>
        /// Send data for new score entry and save highscores.
        /// </summary>
        /// <param name="name">Player name.</param>
        /// <param name="score">Player's score</param>
        /// <returns>The rank of the newly added score entry.</returns>
        public int SendScore(string name, int score)
        {
            int rank = _highScores.Add(CreateNewEntry(name, score));
            SaveToFile();
            return rank;
        }
        /// <summary>
        /// Get a list of ScoreEntry.Entries are readily
        /// sorted in descending order by Score.
        /// </summary>
        /// <returns>A list of ScoreEntry</returns>
        public List<HighScoreEntry> GetHighScoresList()
        {
            List<HighScoreEntry> highscore = new List<HighScoreEntry>();
            foreach(HighScoreEntry entry in _highScores.GetHighScores())
            {
                highscore.Add(new HighScoreEntry() { Name = entry.Name, Score = entry.Score});
            }
            return highscore;
        }
    }

}
