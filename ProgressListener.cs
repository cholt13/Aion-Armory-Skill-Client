/*
 * Corey Holt
 * 4/19/2014
 * Aion Armory Skill Console Client
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AionArmorySkillClient
{
    class ProgressListener
    {
        private AionArmorySkillClient skillClient;
        int progressBarSize;
        char progressChar;
        char incompleteChar;
        // public on/off switch for progress bar
        public bool showProgressBar;

        /// <summary>
        /// ProgressListener constructor
        /// </summary>
        /// <param name="sc">References an AionArmorySkillClient</param>
        public ProgressListener(ref AionArmorySkillClient sc)
        {
            skillClient = sc;
            skillClient.Progress += new EventHandler(DrawProgress);
            progressBarSize = 30;
            progressChar = '+';
            incompleteChar = '-';
            showProgressBar = false;
        }

        /// <summary>
        /// Draws a progress/loading bar in the console to show how
        /// far along the skill client is in performing its current
        /// task
        /// </summary>
        /// <param name="sender">The AionArmorySkillClient object</param>
        /// <param name="e">The EventArgs for the Progress event</param>
        private void DrawProgress(object sender, EventArgs e)
        {
            // Do not print the progress bar if console is not ready for it
            if (!showProgressBar)
            {
                return;
            }

            Console.CursorVisible = false;
            ConsoleColor currentColor = Console.ForegroundColor;
            int cursorPos = Console.CursorLeft;
            float percentage = (float)skillClient.StepsCompleted /
                               (float)skillClient.TotalSteps;
            int progressNum = (int)Math.Floor(percentage / 
                              (1.0 / progressBarSize));
            string completedStr = new String(progressChar, progressNum);
            string incompleteStr = new String(incompleteChar,
                                              progressBarSize - progressNum);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[" + completedStr);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(incompleteStr + "]");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" {0}%", (percentage * 100).ToString("N2"));
            // Reset cursor position to overwrite bar upon next progress
            Console.CursorLeft = cursorPos;
            // Reset console color
            Console.ForegroundColor = currentColor;

            if (percentage == 1.0)
            {
                // Once loading has completed, clear the loading bar
                //  from console and reset cursor position to start of line
                for (int i = 0; i < Console.WindowWidth; ++i)
                {
                    Console.Write(" ");
                }
                Console.CursorLeft = cursorPos;
                Console.CursorVisible = true;
            }
        }

        /// <summary>
        /// Removes the event handler from the Progress event
        /// </summary>
        public void Remove()
        {
            skillClient.Progress -= new EventHandler(DrawProgress);
            skillClient = null;
        }
    }
}
