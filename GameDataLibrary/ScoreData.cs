using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDataLibrary
{
    public class ScoreData
    {
        public int Level;
        public int Score;

        public List<int> HighScores;

        string fileName = "score.xml";

        public ScoreData Load()
        {
            ScoreData s = Storage.LoadXml<ScoreData>(fileName);

            if (s == null)
            {
                s = new ScoreData();
                s.LoadDefault();
            }

            return s;
        }

        void LoadDefault()
        {
            Level = 0;
            Score = 0;

            HighScores = new List<int>() { 3, 2, 1 };
        }

        public bool SetHighScore(int totalScore)
        {
            if (HighScores.Contains(totalScore)) return true;

            for (int i = 0; i < HighScores.Count; i++)
                if (totalScore > HighScores[i])
                {
                    HighScores.Insert(i, totalScore);
                    HighScores.RemoveAt(HighScores.Count - 1);
                    return true;
                }

            return false;
        }

        public void Save()
        {
            Storage.SaveXml<ScoreData>(fileName, this);
        }
    }
}
