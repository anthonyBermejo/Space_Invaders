using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Space_Invaders
{
    /// <summary>
    /// A class will allow the game to keep track of a highscore.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 31/07/2014 - v1.2
    /// </summary>
    class HighScore
    {
        private string[] highScoreArray;

        private const string FILENAME = @"highscore.txt";

        //Constructor
        public HighScore()
        {
            highScoreArray = new string[20];
        }


        public string[] ReadAllHighScores()
        {
            string line;
            int i = 0;

             using (StreamReader streamR = new StreamReader(new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.Read)))
             {
                 while((line = streamR.ReadLine()) != null)
                 {
                     highScoreArray[i] = line.Substring(0,3);
                     highScoreArray[i+1] = line.Substring(4);
                     i += 2;
                 }
             }
             return highScoreArray;
        }

        /// <summary>
        /// Reads the value of highscore from the file.
        /// </summary>
        /// <returns>Value of the highscore</returns>
        public int ReadHighScore()
        {
            int highScore = 0;

            using (StreamReader streamR = new StreamReader(new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.Read)))
            {
                String tempString = null;
                tempString = streamR.ReadLine();

                if (tempString != null)
                {
                    tempString = tempString.Substring(4);
                    Int32.TryParse(tempString, out highScore);
                }
            }

            return highScore;
        }

        /// <summary>
        /// Writes the highscore to the file.
        /// </summary>
        /// <param name="highScore">The highscore that will be written to the file</param>
        public void WriteHighScore(string initials, int highScore)
        {
            using (StreamWriter streamW = new StreamWriter(new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                streamW.WriteLine(initials + "," + highScore);
            }
        }
    }
}
