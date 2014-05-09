Corey Holt
4/19/2014
Aion Armory Skill Console Client

	This program provides a simple, console-only interface for 
searching the character skills portion of the database website 
Aion Armory for the MMORPG Aion Online from NCSOFT. The client 
first prompts the user for a character class and then a character 
level to search on. The client will accept any of the classes 
currently listed on Aion Armory. These include: Assassin, Chanter, 
Cleric, Gladiator, Ranger, Sorcerer, Spiritmaster, Templar, 
Gunslinger, Songweaver. It also accepts the common shorthand 
notation for these classes that players commonly use. If there are 
new character skills available for the specified level and class, 
then those skills will be displayed in the console window on 
successive lines. If there are no new skills available for a 
specified level, the client reports this and asks the user if they 
would like to perform a new search. After displaying the list of 
skills for the specified level, the client asks the user if they 
would also like to view the descriptions for each of the skills. 
Answering "yes" will tell the program to print out the skill 
descriptions it has retrieved from Aion Armory, including usage 
details such as target, cast time, cooldown time, and cost (in 
MP or DP). The user is then asked if they would like to perform 
another search.

	The client connects to the various Aion Armory website pages 
via C#'s WebClient, and it extracts the skill data from the HTML 
code of the pages using regular expression matching. The WebClient 
connection that retrieves the initial skill data (skill names) for 
a class + level is done on the main thread, and skill description + 
usage detail retrieval is done in a background thread as soon as 
the initial list of skills has been displayed, but before the user 
has been asked if they would like to get the skill descriptions. 
This is done to minimize the delay of retrieving the higher volume 
of data when the user answers "yes" to wanting to see the 
descriptions.
