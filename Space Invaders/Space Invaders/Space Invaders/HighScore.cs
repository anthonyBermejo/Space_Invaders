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
    /// Version - 25/05/2013
    /// </summary>
    class HighScore
    {
        private const string FILENAME = @"highscore.txt";

        //Constructor
        public HighScore()
        {
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
        public void WriteHighScore(int highScore)
        {
            using (StreamWriter streamW = new StreamWriter(new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                streamW.WriteLine("AAA," + highScore);
            }
        }
    }
}
