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
    public class Skill
    {
        string skillName;
        string skillPageURL;
        string skillDescription;
        string castTime;
        string skillTarget;
        string cooldownTime;
        string mpCost;
        bool isActiveSkill = false;
        bool hasDescription = false;

        /// <summary>
        /// Skill constructor
        /// </summary>
        /// <param name="sn">Contains the skill name</param>
        /// <param name="url">Contains the skill's page URL</param>
        public Skill(string sn, string url)
        {
            skillName = String.Copy(sn);
            skillPageURL = String.Copy(url);
        }

        /// <summary>
        /// IsActive property
        /// Indicates if skill is active or passive
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActiveSkill;
            }
            set
            {
                isActiveSkill = value;
            }
        }

        /// <summary>
        /// Name property
        /// The skill's name string
        /// </summary>
        public string Name
        {
            get
            {
                return skillName;
            }
            set
            {
                skillName = value;
            }
        }

        /// <summary>
        /// PageURL property
        /// The skill's description page URL
        /// </summary>
        public string PageURL
        {
            get
            {
                return skillPageURL;
            }
            set
            {
                skillPageURL = value;
            }
        }

        /// <summary>
        /// Description property
        /// The skill's description string
        /// </summary>
        public string Description
        {
            get
            {
                return skillDescription;
            }
            set
            {
                skillDescription = value;
            }
        }

        /// <summary>
        /// CastTime property
        /// The skill's cast time string
        /// </summary>
        public string CastTime
        {
            get
            {
                return castTime;
            }
            set
            {
                castTime = value;
            }
        }

        /// <summary>
        /// Target property
        /// The skill's target string
        /// </summary>
        public string Target
        {
            get
            {
                return skillTarget;
            }
            set
            {
                skillTarget = value;
            }
        }

        /// <summary>
        /// Cooldown property
        /// The skill's cooldown string
        /// </summary>
        public string Cooldown
        {
            get
            {
                return cooldownTime;
            }
            set
            {
                cooldownTime = value;
            }
        }

        /// <summary>
        /// MPCost property
        /// The skill's MP/DP/HP cost string
        /// </summary>
        public string MPCost
        {
            get
            {
                return mpCost;
            }
            set
            {
                mpCost = value;
            }
        }

        /// <summary>
        /// HasDescription property
        /// Indicates if the skill has a description
        /// </summary>
        public bool HasDescription
        {
            get
            {
                return hasDescription;
            }
            set
            {
                hasDescription = value;
            }
        }
    }
}
