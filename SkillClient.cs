/*
 * Corey Holt
 * 4/19/2014
 * Aion Armory Skill Console Client
 */

using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace AionArmorySkillClient
{
    public class AionArmorySkillClient
    {
        WebClient armory;
        string skillsURL;
        string skillsPageData;
        string charClass;
        int charLevel;
        List<Skill> skillsList;
        // Information retrieval progress event
        public event EventHandler Progress;
        // Counters to quantify progress
        int stepNum;
        int totalSteps;

        /// <summary>
        /// AionArmorySkillClient constructor
        /// </summary>
        AionArmorySkillClient()
        {
            armory = new WebClient();
            charLevel = -1;
            skillsList = new List<Skill>();
            stepNum = 0;
            totalSteps = 4;
        }

        /// <summary>
        /// StepsCompleted property - for tracking incremental
        /// progess
        /// </summary>
        public int StepsCompleted
        {
            get
            {
                return stepNum;
            }
        }

        /// <summary>
        /// TotalSteps property - for progress completion goal
        /// </summary>
        public int TotalSteps
        {
            get
            {
                return totalSteps;
            }
        }

        /// <summary>
        /// Method to trigger progress event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnProgress(EventArgs e)
        {
            if (Progress != null)
            {
                Progress(this, e);
            }
        }

        /// <summary>
        /// Gets the search info from the user: character class and
        /// level; builds the GET request URL string according to input
        /// </summary>
        public void GetSearchInfo()
	    {
		    bool invalidClass = false;
		    bool invalidLevel = false;
		
		    Console.Write("Enter the class you would like to search on: ");
		
		    do
		    {
			    charClass = Console.ReadLine();
			    charClass = charClass.ToLower();
				invalidClass = false;
		
			    if (charClass == "assassin" || charClass == "sin")
			    {
			    	charClass = "Assassin";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.7";
			    }
			    else if (charClass == "chanter")
			    {
				    charClass = "Chanter";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.11";
			    }
			    else if (charClass == "cleric")
			    {
				    charClass = "Cleric";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.4";
			    }
			    else if (charClass == "gladiator" || charClass == "glad")
			    {
				    charClass = "Gladiator";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.5";
			    }
			    else if (charClass == "ranger")
			    {
				    charClass = "Ranger";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.8";
			    }
			    else if (charClass == "sorcerer" || charClass == "sorc")
			    {
				    charClass = "Sorcerer";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.9";
			    }
			    else if (charClass == "spiritmaster" || charClass == "sm")
			    {
				    charClass = "Spiritmaster";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.10";
			    }
			    else if (charClass == "templar" || charClass == "temp")
			    {
				    charClass = "Templar";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.6";
			    }
			    else if (charClass == "gunslinger" || charClass == "gs" || 
				    	 charClass == "gunner")
			    {
				    charClass = "Gunslinger";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.14";
			    }
			    else if (charClass == "songweaver" || charClass == "sw" ||
					     charClass == "bard")
		    	{
				    charClass = "Songweaver";
				    skillsURL = "http://www.aionarmory.com/search.aspx?browse=6.15";
			    }
			    else
			    {
			    	invalidClass = true;
				    Console.Write("Invalid class entered. Please try again: ");
			    }
		    } while (invalidClass);
		
		    Console.Write("Enter the character level (9-65) you would like " +
						  "to check for skills: ");
		    string levelInput;
		
		    do
		    {
			    levelInput = Console.ReadLine();
				invalidLevel = false;
			    try
			    {
				    charLevel = Convert.ToInt32(levelInput);
				    if ((charLevel < 9) || (charLevel > 65))
				    {
				    	Console.Write("Please enter a level within the range 9-65: ");
                                        invalidLevel = true;
				    }
			    }
                catch (Exception exc)
                {
                    if (exc is FormatException || exc is InvalidCastException)
                    {
                        invalidLevel = true;
                        Console.Write("Invalid level. Please try again: ");
                    }
                    else
                    {
                        throw;
                    }
                }

		    } while (invalidLevel);
		
		    // Build GET request URL based on selected level
		    skillsURL += "&filters=501=" + levelInput + ";502=" + 
                         levelInput + ";504=2";
	    }

        /// <summary>
        /// Downloads the HTML returned by the GET request to Aion Armory
        /// </summary>
        public void EstablishArmoryConnection()
        {
            try
            {
                // Increment progress to show loading bar
                ++stepNum;
                OnProgress(EventArgs.Empty);

                skillsPageData = armory.DownloadString(skillsURL);
                // Increment progress
                ++stepNum;
                OnProgress(EventArgs.Empty);
            }
            catch (WebException)
            {
                Console.WriteLine("Could not connect to Aion Armory.");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Uses Regex matches to extract skill names and description
        /// links from the skill page HTML code
        /// </summary>
        public void ExtractSkillData()
        {
            string skillName;
            string skillPageURL;
            string skillPattern = ".*?<a\\shref=\"http://www.aionarmory.com/" +
                                  "([a-z0-9.?=]+?)\">(.*?)</a>.*?";

            MatchCollection skillMatches = Regex.Matches(skillsPageData,
                                                         skillPattern);
            // Increment progress
            ++stepNum;
            OnProgress(EventArgs.Empty);

            foreach (Match match in skillMatches)
            {
                skillName = match.Groups[2].Value;
                skillPageURL = "http://www.aionarmory.com/" +
                               match.Groups[1].Value;
                skillsList.Add(new Skill(skillName, skillPageURL));
            }

            // Increment progress
            ++stepNum;
            OnProgress(EventArgs.Empty);

            // Reset step counter
            stepNum = 0;
            // Prepare new progress goal for skill description retrieval
            totalSteps = skillsList.Count;
        }

        /// <summary>
        /// Print the skill names found in the search on separate
        /// lines, or report that no new skills are available for
        /// the specified class and level
        /// </summary>
        /// <returns>true if skills found, false if not</returns>
        public bool PrintSkillNames()
		{
			Console.WriteLine(charClass + " skills for level " + 
							  charLevel.ToString() + ":");
			Console.WriteLine();
		
			if (skillsList.Count == 0)
			{
				Console.WriteLine("No new skills available.\n");
				return false;
			}
			else
			{
				for (int i = 0; i < skillsList.Count; ++i)
				{
					Console.WriteLine(skillsList[i].Name);
				}
				Console.WriteLine();
				return true;
			}
		}

        /// <summary>
        /// Use the skill description links acquired in ExtractSkillData()
        /// to download the HTML code from the skill description page for
        /// each skill
        /// </summary>
        public void RetrieveSkillDescriptions()
        {
            string skillURL;
            string skillPageData;

            for (int i = 0; i < skillsList.Count; ++i)
            {
                skillURL = skillsList[i].PageURL;
                try
                {
                    skillPageData = armory.DownloadString(skillURL);
                    skillsList[i].Description =
                        ExtractSkillDescription(ref skillPageData);
                    ExtractSkillUsageData(ref skillPageData, i);
                    skillsList[i].HasDescription = true;
                    // Increment progress
                    ++stepNum;
                    OnProgress(EventArgs.Empty);
                }
                catch (WebException)
                {
                    Console.WriteLine("Could not connect to Aion Armory.");
                    Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// Uses Regex matching to extract a skill's description and 
        /// optional special information from its description page code
        /// </summary>
        /// <param name="pageData">References the string containing 
        /// the skill's description page HTML code</param>
        /// <returns>A string containing the skill's description</returns>
        private string ExtractSkillDescription(ref string pageData)
        {
            string skillDescription = "";
            string specialInfoLine;
            Match skillDescMatch;
            Match skillSpecialInfoMatch;
            Match skillSpecialAddInfoMatch;

            string skillDescPattern =
                ".*?<td\\sid=\"detailsTooltipText\">.*?<td>([^<].*?)<.*";

            string skillSpecialInfoPattern =
                ".*?<td\\sid=\"detailsTooltipText\">.*?<td>.*?br>(.*?)<.*?/td>";

            string skillSpecialAddInfoPattern = "br>.*?br>(.*?)</td>";

            skillDescMatch = Regex.Match(pageData, skillDescPattern);

            if (skillDescMatch.Success)
            {
                skillDescription = skillDescMatch.Groups[1].Value;
            }

            skillSpecialInfoMatch = Regex.Match(pageData, skillSpecialInfoPattern);

            if (skillSpecialInfoMatch.Success)
            {
                skillDescription += "\n";
                skillDescription += skillSpecialInfoMatch.Groups[1].Value;

                specialInfoLine = skillSpecialInfoMatch.Groups[0].Value;

                skillSpecialAddInfoMatch = Regex.Match(specialInfoLine,
                                                       skillSpecialAddInfoPattern);
                if (skillSpecialAddInfoMatch.Success)
                {
                    skillDescription += "\n";
                    skillDescription += skillSpecialAddInfoMatch.Groups[1].Value;
                }
            }

            return skillDescription;
        }

        /// <summary>
        /// Uses Regex matching to extract a skill's various usage 
        /// details, including: target, cast time, cooldown time, 
        /// mp/dp/hp cost
        /// </summary>
        /// <param name="pageData">References the string containing the 
        /// skill's description page HTML code</param>
        /// <param name="skillIndex">Indicates which index the skill 
        /// occupies in the skillsList List</param>
        private void ExtractSkillUsageData(ref string pageData, int skillIndex)
        {
            string skillUsageString;
            string skillTarget = "None";
            string skillCastTime = "None";
            string skillCooldownTime = "None";
            string skillMPCost = "None";

            string skillUsagePattern =
                ".*?<td\\sid=\"detailsTooltipText\">.*?</table><table style=\"" +
                "width: 100%;\">(.*?)</table>.*";

            Match skillUsageMatch = Regex.Match(pageData, skillUsagePattern);

            if (skillUsageMatch.Success)
            {
                skillUsageString = skillUsageMatch.Groups[1].Value;

                string skillUsageGroupsPattern =
                    "^.*?Target</td><td\\sclass=\"spellCell\">(.*?)</td>.*?" +
                    "Cast\\sTime</td><td\\sclass=\"spellCell\">(.*?)</td>.*Cool" +
                    "-time</td><td\\sclass=\"spellCell\">(.*?)</td></tr>(<tr>" +
                    ".*?Cost</td>.*?(%?[MDH]P\\s\\d*)</td></tr>)*$";

                Match skillUsageGroupsMatch =
                    Regex.Match(skillUsageString, skillUsageGroupsPattern);

                if (skillUsageGroupsMatch.Success)
                {
                    skillTarget = skillUsageGroupsMatch.Groups[1].Value;
                    skillCastTime = skillUsageGroupsMatch.Groups[2].Value;
                    skillCooldownTime = skillUsageGroupsMatch.Groups[3].Value;

                    if (skillUsageGroupsMatch.Groups.Count == 6)
                    {
                        if (skillUsageGroupsMatch.Groups[5].Value != "")
                        {
                            skillMPCost = skillUsageGroupsMatch.Groups[5].Value;
                        }
                    }

                    skillsList[skillIndex].IsActive = true;
                }
            }

            skillsList[skillIndex].Target = skillTarget;
            skillsList[skillIndex].Cooldown = skillCooldownTime;
            skillsList[skillIndex].CastTime = skillCastTime;
            skillsList[skillIndex].MPCost = skillMPCost;
        }

        /// <summary>
        /// Prints skill names and their associated descriptions and 
        /// usage details one after another, separated by dashes
        /// </summary>
        public void PrintSkillDescriptions()
        {
            Console.WriteLine("--Skill Descriptions--\n");

            for (int i = 0; i < skillsList.Count; ++i)
            {
                Skill currentSkill = skillsList[i];

                Console.WriteLine(currentSkill.Name + ":\n");

                if (currentSkill.HasDescription)
                {
                    Console.WriteLine(currentSkill.Description);

                    if (currentSkill.IsActive)
                    {
                        PrintSkillUsageDetails(ref currentSkill);
                    }
                }
                else
                {
                    Console.WriteLine("Could not access skill page.");
                }

                Console.WriteLine();
                
                // If not the last skill, print a '-----------' separator
                if (i < skillsList.Count - 1)
                {
                    Console.WriteLine("------------");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Prints a block of usage details for a skill
        /// </summary>
        /// <param name="s">References the skill object</param>
        private void PrintSkillUsageDetails(ref Skill s)
        {
            Console.WriteLine("Target:\t\t" + s.Target);
            Console.WriteLine("Cast time:\t" + s.CastTime);
            Console.WriteLine("Cooldown:\t" + s.Cooldown);
            Console.WriteLine("Cost:\t\t" + s.MPCost);
        }

        public static void Main()
        {
            string inputString;
            bool skillsAtLevel = false;
            bool doAnotherSearch = false;

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine("\n-++- Aion Armory Skill Search Client " +
                              "-++-\n");
            do
            {
                AionArmorySkillClient skillClient = new AionArmorySkillClient();
                ProgressListener listener = new ProgressListener(ref skillClient);

                skillClient.GetSearchInfo();
                Console.WriteLine("\nRetrieving skills info from Aion Armory...\n");
                listener.showProgressBar = true;
                skillClient.EstablishArmoryConnection();
                skillClient.ExtractSkillData();
                if (skillClient.PrintSkillNames())
                {
                    skillsAtLevel = true;
                }
                else
                {
                    skillsAtLevel = false;
                }
                listener.showProgressBar = false;

                if (skillsAtLevel)
                {
                    // Retrieve skill descriptions now in separate thread to
                    //  expedite displaying if the user chooses to see them
                    ThreadStart descRetrievalRef =
                        new ThreadStart(skillClient.RetrieveSkillDescriptions);
                    Thread descRetrievalThread = new Thread(descRetrievalRef);
                    descRetrievalThread.Start();

                    Console.Write("See skill descriptions? Enter Y/N: ");
                    inputString = Console.ReadLine();
                    inputString = inputString.ToLower();
                    while (inputString != "y" && inputString != "n" &&
                           inputString != "yes" && inputString != "no")
                    {
                        Console.Write("Invalid choice. Enter Y or N: ");
                        inputString = Console.ReadLine();
                        inputString = inputString.ToLower();
                    }

                    if (inputString == "y" || inputString == "yes")
                    {
                        Console.WriteLine("\nRetrieving skill descriptions from " +
                                          "Aion Armory...\n");
                        listener.showProgressBar = true;

                        while (descRetrievalThread.IsAlive)
                        {
                            // Wait for retrieval thread to finish
                        }
                        skillClient.PrintSkillDescriptions();
                    }
                }

                Console.Write("Perform a new search? Enter Y/N: ");
                inputString = Console.ReadLine();
                inputString = inputString.ToLower();

                while (inputString != "y" && inputString != "n" &&
                       inputString != "yes" && inputString != "no")
                {
                    Console.Write("Invalid choice. Enter Y or N: ");
                    inputString = Console.ReadLine();
                    inputString = inputString.ToLower();
                }

                if (inputString == "y" || inputString == "yes")
                {
                    Console.WriteLine();
                    doAnotherSearch = true;
                }
                else
                {
                    doAnotherSearch = false;
                }

                listener.Remove();
            } while (doAnotherSearch);

            Console.WriteLine("\nThank you for using the Aion Armory " +
                              "Skill Search Client!");
        }
    }
}
