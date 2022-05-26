using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var continueLoop = "Initial Value";         // Creates a value which will loop the menu if it does not equal "Quit".
            var teamNameList = new List<string>();      // This will hold the team names later on.
            var singleNameList = new List<string>();    // Likewise, this will hold the individual names later on.
            var teamMembers = new string[4, 5];         // Creates the multi-dimensional array that will hold team member info. 4 wide, 5 long.
            var teamEvents = new string[5, 2];          // Creates the multi-dimensional array that will hold all team events, as well as their category.
            var singleEvents = new string[5, 2];        // Creates the multi-dimensional array that will hold all individual events, as well as their category.
            string teamNamePath = @"..\\..\..\Program Text Files\Team Name List.txt";
            string teamMemberPath = @"..\\..\..\Program Text Files\Team Members List.txt";
            string singleNamePath = @"..\\..\..\Program Text Files\Single Name List.txt";
            string teamEventPath = @"..\\..\..\Program Text Files\Team Event List.txt";
            string singleEventPath = @"..\\..\..\Program Text Files\Single Event List.txt";
            string teamTournamentPath = @"..\\..\..\Program Text Files\Team Tournament Results.txt";
            string singleTournamentPath = @"..\\..\..\Program Text Files\Single Tournament Results.txt";
            while (continueLoop.ToLower() != "quit")    // This begins the menu loop, checking that the user does not want to exit.
            {
                Console.WriteLine("\n--------- Sheffield College Tournament Program ----------\n");
                Console.WriteLine("Please select an option:\n");
                Console.WriteLine("1. Add/Edit Team Name Information");
                Console.WriteLine("2. Add/Edit Team Member Information");
                Console.WriteLine("3. Add/Edit Individual Name Information\n");
                Console.WriteLine("4. Add/Edit Event Information");
                Console.WriteLine("5. Input New Team/Individual Tournament\n");
                Console.WriteLine("----------------------------------------------------------\n");
                Console.WriteLine("Write an associated number to enter. Write 'Quit' to quit.\n");
                continueLoop = Console.ReadLine();      // This is where the user will choose their menu options, or exit the program.
                switch (continueLoop)                   // This is similar to an if loop, but checks all options at once. Makes things neater to do.
                {
                    case "1":                           // This will be the result if the user inputs "1".
                        string teamNameFormality = "Team Names: " + Environment.NewLine;
                        Console.Clear();
                        Console.WriteLine("Team Information Input");
                        Console.WriteLine("Would you like to: 1. Add or 2. Edit the team information?\n Please input the number or option. \nIf neither of these, input anything else to return to menu.\n");
                        var teamOptionChoice = Console.ReadLine();
                        if (teamOptionChoice.ToLower() == "add" || teamOptionChoice == "1")     // If the user wants to add teams..
                        {
                            Console.Clear();
                            if (teamNameList.Count == 4)
                            {
                                Console.WriteLine("\n4 teams already exist. Please use the edit command to change their values. Returning to menu..");
                                break;      // This will inform the user that 4 teams already exist and no more can be added, before returning to the menu.
                            }
                            Console.WriteLine("Please enter the name of all teams, separated by commas. There should be 4 teams.\n");
                            var teamNameUserInput = Console.ReadLine();
                            String[] teamNameArray = SplitList(teamNameUserInput);      /* This is handed to splitList. In this, the string that the user inputted is divided up into small chunks,
                                                                                         * which is then written to a list which contains the team names for future reference. */
                            for (int i = 0; i < teamNameArray.Length; i++)
                            {
                                teamNameArray[i] = teamNameArray[i].Trim(' ');      // A cleanup tool, removes all extra spaces that come from the regular user input.
                            }
                            if (teamNameArray.Length > 4)
                            {
                                Console.WriteLine("\nNumber of teams inputted greater than 4. Returning to menu..");      // A simple check to make sure that the right number of teams exist.
                                break;                                                                                  // If this is fulfilled, the user is returned to menu and nothing is
                            }                                                                                           // written.
                            var oldTeamNameListPos = teamNameList.Count;
                            teamNameList.AddRange(teamNameArray);
                            if (teamNameList.Count > 4)
                            {
                                Console.WriteLine("\nThe number of teams in total would be greater than 4. Changes have not been saved. Returning to menu..");
                                teamNameList.RemoveRange(oldTeamNameListPos, teamNameArray.Length);  // If the user were to enter the code, input 1-3 teams, then return and try and enter more than 4, this will stop that from changing anything.
                                break;
                            }
                            WriteToTextFile(teamNamePath, teamNameFormality, teamNameList);
                            Console.WriteLine("\nTeams created. These are the teams: \n");
                            PrintArray(teamNameList);   // Prints off all inputted team names for user to confirm knowledge.
                            Console.WriteLine("\nThe list of teams has been written to Team Name List.txt, in the Program Text Files folder. \nThis text file will be overwritten when this section of the program is next run. Returning to menu..");
                        }
                        else if (teamOptionChoice.ToLower() == "edit" || teamOptionChoice == "2")       // If the user wants to edit teams..
                        {
                            Console.Clear();
                            if (teamNameList.Count == 0)        // A user cannot edit teams that do not exist. Kicks them out if they try to.
                            {
                                Console.WriteLine("\nNo teams exist that can be edited. Please create a team beforehand. Returning to menu..");
                                break;
                            }
                            var teamNamePosition = 0;
                            Console.WriteLine("\nWhich team would you like to edit? Please enter the name, the current teams are: ");
                            PrintArray(teamNameList);       // Lets user confirm once more which team they wish to edit
                            Console.WriteLine("\n");
                            var teamNameEditChoice = Console.ReadLine();
                            foreach (var team in teamNameList)      // Cycles through each existing team to check if it is the selected one.
                            {
                                teamNamePosition++;
                                if (teamNameEditChoice == team)     // Triggers if the team name is found
                                {
                                    Console.WriteLine("Please enter the new name of this team: \n");
                                    var newTeamName = Console.ReadLine();       // User inputs the new team name they want
                                    Console.WriteLine("\nChanging team name from " + teamNameEditChoice + " to " + newTeamName + "\n");      // Tells user what it was, and what it will be.
                                    teamNameList[teamNamePosition - 1] = newTeamName;   // New team name is applied.
                                    File.WriteAllText(teamNamePath, String.Empty);        // The text document containing names is updated to reflect changes.
                                    File.WriteAllText(teamNamePath, teamNameFormality);
                                    File.AppendAllLines(teamNamePath, teamNameList);       // This is working on the assumption that a file already exists.
                                    Console.WriteLine("Finished. These are the teams currently: ");
                                    PrintArray(teamNameList);       // Again reminds the user of what the teams now are, rather than them having to hope it worked.
                                    Console.WriteLine("\nThe list of teams has been updated to reflect these changes. Returning to menu..");
                                    break;
                                }
                                else if (teamNameEditChoice != team && teamNamePosition == 4)   // Else if the user's input does not exist..
                                {
                                    Console.WriteLine("No team under this name exists. Returning to menu..");   // Code will cycle through, and if no team names match the inputted one, returns to menu.
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearNreturn();
                            break;
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Team Member Information Input");
                        Console.WriteLine("Would you like to: 1. Add or 2. Edit the team member information?\n Please input the number or option.  \nIf neither of these, input anything else to return to menu.\n");
                        var teamMemberOptionChoice = Console.ReadLine();
                        if (teamMemberOptionChoice.ToLower() == "add" || teamMemberOptionChoice == "1")     // If the user wants to add teams..
                        {
                            if (teamNameList.Count != 4)    // If not all teams exist, data entry will be declined.
                            {
                                Console.WriteLine("Not enough teams currently exist. Please go back and ensure that all teams have been inputted.");
                                break;
                            }
                            Console.Clear();
                            Console.WriteLine("Team Member Input\n");
                            var nextArrayLine = 0;
                            var returning = false;
                            for (var loop = 0; loop < 4; loop++) // Loops this four times - four times for four teams
                            {
                                if (returning == true)  // If the user makes erroneous input, cancels everything.
                                    break;
                                Console.WriteLine("\nPlease enter all five members of " + teamNameList[nextArrayLine] + ", separated by commas:\n");
                                var teamMemberUserInput = Console.ReadLine();       // User inputs all 5 team members.
                                var teamMemberInputList = SplitList(teamMemberUserInput);   // The user's string is split into a list.
                                for (int i = 0; i < teamMemberInputList.Length; i++)
                                {
                                    teamMemberInputList[i] = teamMemberInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (var L = 0; L < teamNameList.Count + 1; L++)
                                {
                                    try
                                    {
                                       teamMembers[nextArrayLine, L] = teamMemberInputList[L]; // Slots each new value into the multi-dimensional array.
                                    }
                                    catch (IndexOutOfRangeException)    // This triggers if the user does not enter 5 values.
                                    {
                                        Console.Clear();
                                        Console.WriteLine("5 members are required for each team. Returning to menu..");
                                        returning = true;   // Sets off the trigger needed to cancel the loops.
                                    }
                                }
                                nextArrayLine++;
                            }
                            if (returning == true)
                                break;      // Further breaks the loop so nothing is written.
                            WriteToTeamMembersText(teamNameList, teamMembers, teamMemberPath);
                            Console.WriteLine("\nAll inputted teams have been accepted. This information has been written to Team Members List.txt, in the Program Text Files folder.");
                            break;
                            
                        }
                        else if (teamMemberOptionChoice.ToLower() == "edit" || teamMemberOptionChoice == "2")
                        {
                            if (teamNameList.Count != 4)    // If not all teams exist, data entry will be declined.
                            {
                                Console.WriteLine("Not enough teams currently exist. Please go back and ensure that all teams have been inputted.");
                                break;
                            }
                            Console.Clear();
                            Console.WriteLine("\nPlease enter the name of the team who's members you would like to edit. The teams are below: ");
                            PrintArray(teamNameList);
                            Console.WriteLine("\n");
                            var userMemberEditSelection = Console.ReadLine();
                            for (var rowPosition = 0; rowPosition < teamNameList.Count + 1; rowPosition++)
                            {
                                if (userMemberEditSelection == teamNameList[rowPosition])
                                {
                                    Console.WriteLine("\nPlease enter the name of the team member you are changing - these are the members: \n");
                                    for (var i = 0; i < 5; i++)
                                        Console.WriteLine(teamMembers[rowPosition, i]); // Outputs each team member that exists
                                    Console.WriteLine("\n");
                                    userMemberEditSelection = Console.ReadLine();
                                    for (var columnPosition = 0; columnPosition < 5; columnPosition++)
                                    {
                                        if (userMemberEditSelection == teamMembers[rowPosition, columnPosition])    // If the user's input is an existing member..
                                        {
                                            Console.WriteLine("\nPlease enter the new name for this person: \n");
                                            var newTeamMemberName = Console.ReadLine(); // User inputs new name.
                                            Console.WriteLine("\nChanging " + teamMembers[rowPosition, columnPosition] + " to " + newTeamMemberName + "..\n");
                                            teamMembers[rowPosition, columnPosition] = newTeamMemberName;   // User's new name replaces old name.
                                            WriteToTeamMembersText(teamNameList, teamMembers, teamMemberPath);  // Writes this to the text file.
                                            Console.WriteLine("Success. Team member name has been changed. Text file has updated to reflect this.");
                                            break;
                                        }
                                    }
                                    break;
                                }
                                else if (userMemberEditSelection != teamNameList[rowPosition] && rowPosition == 3)
                                {
                                    Console.WriteLine("This input was not valid. Returning to menu..");
                                    break;
                                }
                            }

                        }
                        break;
                    case "3":
                        string singleNameFormality = "Individual members list:" + Environment.NewLine;
                        Console.Clear();
                        Console.WriteLine("Individual Information Input");
                        Console.WriteLine("Would you like to: 1. Add or 2. Edit individual information?\n Please input the number or option.  \nIf neither of these, input anything else to return to menu.\n");
                        var singleOptionChoice = Console.ReadLine();
                        if (singleOptionChoice.ToLower() == "add" || singleOptionChoice == "1")     // If user wants to add..
                        {
                            Console.Clear();
                            if (singleNameList.Count > 19)      // If the max number of students exist..
                            {
                                Console.WriteLine("You have already added the max number of individuals to the list. Please edit existing individuals. Returning to menu..");
                                break;      // User is told to edit instead and returned to menu.
                            }
                            Console.WriteLine("Please enter the name of all (or additional) individuals, separated by commas. This should be no less than 5 and no more than 20: \n");
                            var singleNameUserInput = Console.ReadLine();               // User inputs all students, split by commas.
                            String[] singleNameArray = SplitList(singleNameUserInput);  // Same process with adding user input to a list.
                            for (int i = 0; i < singleNameArray.Length; i++)
                                singleNameArray[i] = singleNameArray[i].Trim(' ');      // Removes blank space from the list, which isn't needed.
                            var oldSingleListPos = singleNameList.Count;
                            singleNameList.AddRange(singleNameArray);
                            if (singleNameList.Count > 20)
                            {
                                Console.WriteLine("\nThe number of individuals in total would be greater than 20. Changes have not been saved. Returning to menu..");
                                teamNameList.RemoveRange(oldSingleListPos, singleNameArray.Length);  // If the user were to enter the code, input 1-19 individuals, then return and try and enter enough to exceed 20 individuals, this will stop that from changing anything.
                                break;
                            }
                            else if (singleNameList.Count < 5)
                            {
                                try
                                {
                                    teamNameList.RemoveRange(oldSingleListPos, singleNameArray.Length);  // If the user were to enter the code and input less than 5 people, this will stop that from changing anything.
                                }
                                catch (ArgumentException)
                                {
                                    Console.WriteLine("\nThe number of individuals you have inputted is lower than 5. Changes have not been saved. Returning to menu..");
                                    break;
                                }
                                Console.WriteLine("\nThe number of individuals you have inputted is lower than 5. Changes have not been saved. Returning to menu..");
                                break;
                            }
                            WriteToTextFile(singleNamePath, singleNameFormality, singleNameList);
                            Console.WriteLine("\nIndividuals created. These are the individuals: \n");
                            PrintArray(singleNameList); // Outputs user's list for clarity
                            Console.WriteLine("\nThe list of individuals has been written to Single Name List.txt, in the Program Text Files folder. This text file will be overwritten when this section the program is next run. Returning to menu..");
                        }
                        else if (singleOptionChoice.ToLower() == "edit" || singleOptionChoice == "2")   // If user wishes to edit existing info..
                        {
                            Console.Clear();
                            if (singleNameList.Count == 0)  // The user will not be allowed to edit entries that do not exist.
                            {
                                Console.WriteLine("\nNo individuals exist that can be edited. Please create a list of individuals beforehand. Returning to menu..");
                                break;
                            }
                            var singleNamePosition = 0; // Sets a variable which will count the position of the upcoming loop for future use.
                            Console.WriteLine("\nWhich individual would you like to edit? Please enter the name, the current individuals are: \n");
                            PrintArray(singleNameList); // Lists out individuals.
                            var singleNameEditChoice = Console.ReadLine();
                            foreach (var individual in singleNameList)  // This loop will check that the value that the user entered actually exists in the list.
                            {
                                singleNamePosition++;
                                if (singleNameEditChoice == individual) // If it does exist..
                                {
                                    Console.WriteLine("\nPlease enter the new name of this individual: \n");
                                    var newSingleName = Console.ReadLine();     // User enters new desired name
                                    Console.WriteLine("\nChanging individual name from " + singleNameEditChoice + " to " + newSingleName + "..\n");  // Reminds user of what it is changing to.
                                    singleNameList[singleNamePosition - 1] = newSingleName;
                                    WriteToTextFile(singleNamePath, singleNameFormality, singleNameList);
                                    Console.WriteLine("\nFinished. These are the individuals currently: \n");
                                    PrintArray(singleNameList);
                                    break;
                                }
                                else if (singleNameEditChoice != individual && singleNamePosition == 20)    // If not..
                                {
                                    Console.WriteLine("No individual under this name exists. No changes have been made. Returning to menu..");   // Code will cycle through, and if no individual names match the inputted one, returns to menu.
                                    break;
                                }
                            }
                        }
                        else        // If the user does not enter one of the specific inputs..
                        {
                            ClearNreturn();
                            break;
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("Event Information");
                        Console.WriteLine("Would you like to work with 1. Team events or 2. Individual events? \nIf neither of these, input anything else to return to menu.\n");
                        var eventOptionChoice = Console.ReadLine();
                        if (eventOptionChoice == "1" || eventOptionChoice.ToLower() == "team")  // If user wants to edit team events..
                        {
                            Console.WriteLine("\n1. Add or 2. Edit team events? If neither of these, input anything else to return to menu.\n");    // Asks if they want to add or edit
                            eventOptionChoice = Console.ReadLine();
                            var maxTeamEvents = 0;  // A check for later down the line, to ensure that the max number of events exist.
                            if (eventOptionChoice == "1" || eventOptionChoice.ToLower() == "add")   // If user input is add..
                            {
                                Console.Clear();
                                for (int i = 0; i < 5; i++)
                                {
                                    if (teamEvents[i, 0] != null)
                                        maxTeamEvents++;            // If one of the 5 event sections isn't blank, it adds one to the previous variable.
                                    else                            
                                        maxTeamEvents += 0;         // If it is, then it adds nothing.
                                }
                                if (maxTeamEvents == 5)             // If the variable is 5 (this means that all values are not null)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Max amount of events already exist. Please edit existing events. Returning to menu..");
                                    break;                          // Returns the user to the menu, saying that max values exist.
                                }
                                Console.WriteLine("Team Event Input\n");
                                Console.WriteLine("Please enter all five team event names in order from first to last, separated by commas:\n");
                                var teamEventUserInput = Console.ReadLine();       // User inputs all 5 team members.
                                var teamEventInputList = SplitList(teamEventUserInput);   // The user's string is split into a list.
                                if (teamEventInputList.Length != 5)
                                {
                                    Console.Clear();
                                    Console.WriteLine("The number of events you have added is not equal to 5. Returning to menu..");
                                    break;
                                }
                                for (int i = 0; i < teamEventInputList.Length; i++)
                                {
                                    teamEventInputList[i] = teamEventInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (var L = 0; L < teamEventInputList.Length; L++)
                                {
                                    teamEvents[L, 0] = teamEventInputList[L]; // Slots each new value into the multi-dimensional array.
                                }
                                Console.WriteLine("\nPlease enter the category of each of these events, in their respective order. \n This should be sporting or academic.\n");
                                teamEventUserInput = Console.ReadLine();    // User enters all categories in order.
                                teamEventInputList = SplitList(teamEventUserInput);
                                for (int i = 0; i < teamEventInputList.Length; i++)
                                {
                                    teamEventInputList[i] = teamEventInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (int k = 0; k < 5; k++)
                                {                                               // This check ensures that an inputted category is allowed. If not, returns to menu.
                                    if (teamEventInputList[k].ToLower() != "sporting" && teamEventInputList[k].ToLower() != "academic")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("One of the inputted values was not Sporting or Academic. Returning to menu..");
                                        break;
                                    }
                                }
                                for (int i = 0; i < teamEventInputList.Length; i++)
                                {
                                    teamEventInputList[i] = teamEventInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (var L = 0; L < teamEventInputList.Length; L++)
                                {
                                    teamEvents[L, 1] = teamEventInputList[L]; // Slots each new value into the multi-dimensional array.
                                }
                                WriteToTeamEventText(teamEvents, teamEventPath);
                                Console.WriteLine("\nAll team events successfully registered. This information has been written to Team Events List.txt, in the Program Text Files folder.");
                                break;
                            }
                            else if (eventOptionChoice == "2" || eventOptionChoice.ToLower() == "edit")
                            {                                                                           
                                Console.Clear();                                                        
                                Console.WriteLine("Team Event Editing");
                                Console.WriteLine("\nPlease input the name of the event you would like to edit. These are the events: \n");
                                for (var i = 0; i < 5; i++)
                                {
                                    Console.WriteLine(teamEvents[i, 0]);    // Writes all existing events for the user.
                                }
                                Console.WriteLine("");
                                var teamEventUserInput = Console.ReadLine();
                                for (var i = 0; i < 5; i++)
                                {
                                    if (teamEventUserInput == teamEvents[i, 0]) // If the users input equals one of the pre-existing team events..
                                    {
                                        Console.WriteLine("\nPlease enter the new name of this event: \n");
                                        var newTeamEventName = Console.ReadLine();  // User enters new event name.
                                        Console.WriteLine("\nPlease enter the new category of this event. Previously it was " + teamEvents[i, 1] + ".\n");  // Program displays old category to make the user's life easier.
                                        var newTeamEventType = Console.ReadLine();  // User enters new event category.
                                        teamEvents[i, 0] = newTeamEventName;    // Sets old value to new value.
                                        teamEvents[i, 1] = newTeamEventType;    // Sets old value to new value.
                                        WriteToTeamEventText(teamEvents, teamEventPath);    // Writes all updated info to the text file.
                                        Console.WriteLine("\nChanges have successfully saved. The information in Team Event List.txt has been overwritten to reflect this. \nReturning to menu..");
                                        break;
                                    }
                                    else if (teamEventUserInput != teamEvents[i, 0] && i == 4)  // If the user input doesn't equal an event and the loop has reached the end..
                                    {
                                        Console.Clear();
                                        Console.WriteLine("The user input does not match an existing event. Returning to menu..");
                                        break;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                ClearNreturn();
                                break;
                            }
                        }
                        else if (eventOptionChoice == "2" || eventOptionChoice.ToLower() == "individual")   // This section of code below is identical to the team event inputs. 
                        {                                                                                   // Please refer to previous comments above for understanding.
                            Console.WriteLine("1. Add or 2. Edit individual events? If neither of these, input anything else to return to menu.\n");
                            eventOptionChoice = Console.ReadLine();
                            var maxSingleEvents = 0;
                            if (eventOptionChoice == "1" || eventOptionChoice.ToLower() == "add")
                            {
                                Console.Clear();
                                for (int i = 0; i < 5; i++)
                                {
                                    if (singleEvents[i, 0] != null)
                                        maxSingleEvents++;
                                    else
                                        maxSingleEvents += 0;
                                }
                                if (maxSingleEvents == 5 && teamEvents[5, 1] != null)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Max amount of events already exist. Please edit existing events. Returning to menu..");
                                    break;
                                }
                                Console.WriteLine("Individual Event Input\n");
                                Console.WriteLine("Please enter all five individual event names, separated by commas:\n");
                                var singleEventUserInput = Console.ReadLine();       // User inputs all 5 team members.
                                var singleEventInputList = SplitList(singleEventUserInput);   // The user's string is split into a list.
                                if (singleEventInputList.Length != 5)
                                {
                                    Console.Clear();
                                    Console.WriteLine("The number of events you have added is not equal to 5. Returning to menu..");
                                    break;
                                }
                                for (int i = 0; i < singleEventInputList.Length; i++)
                                {
                                    singleEventInputList[i] = singleEventInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (var L = 0; L < singleEventInputList.Length; L++)
                                {
                                    singleEvents[L, 0] = singleEventInputList[L]; // Slots each new value into the multi-dimensional array.
                                }
                                Console.WriteLine("\nPlease enter the category of these events, in their respective order. \nThis should be sporting or academic.\n");
                                singleEventUserInput = Console.ReadLine();
                                singleEventInputList = SplitList(singleEventUserInput);
                                for (int i = 0; i < singleEventInputList.Length; i++)
                                {
                                    singleEventInputList[i] = singleEventInputList[i].Trim(' ');  // Trimmed to remove excess blank space.
                                }
                                for (int i = 0; i < singleEventInputList.Length; i++)
                                {
                                    if (singleEventInputList[i].ToLower() != "sporting" && singleEventInputList[i].ToLower() != "academic")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("One of the inputted values was not Sporting or Academic. Returning to menu..");
                                        break;
                                    }
                                }
                                for (var L = 0; L < singleEventInputList.Length; L++)
                                {
                                    singleEvents[L, 1] = singleEventInputList[L]; // Slots each new value into the multi-dimensional array.
                                }
                                WriteToTeamEventText(singleEvents, singleEventPath);
                                Console.WriteLine("\nAll individual events successfully registered. This information has been written to Single Events List.txt, in the Program Text Files folder.");
                                break;
                            }
                            else if (eventOptionChoice == "2" || eventOptionChoice.ToLower() == "edit")
                            {
                                Console.Clear();
                                Console.WriteLine("Individual Event Editing\n");
                                Console.WriteLine("Please input the name of the event you would like to edit. These are the events: \n");
                                for (var i = 0; i < 5; i++)
                                {
                                    Console.WriteLine(singleEvents[i, 0]);
                                }
                                Console.WriteLine("");
                                var singleEventUserInput = Console.ReadLine();
                                for (var i = 0; i < 5; i++)
                                {
                                    if (singleEventUserInput == singleEvents[i, 0])
                                    {
                                        Console.WriteLine("\nPlease enter the new name of this event: \n");
                                        var newSingleEventName = Console.ReadLine();
                                        Console.WriteLine("\nPlease enter the new category of this event. Previously it was " + singleEvents[i, 1] + ".\n");
                                        var newSingleEventType = Console.ReadLine();
                                        singleEvents[i, 0] = newSingleEventName;
                                        singleEvents[i, 1] = newSingleEventType;
                                        WriteToTeamEventText(singleEvents, singleEventPath);
                                        Console.WriteLine("\nChanges have successfully saved. The information in Single Event List.txt has been overwritten to reflect this. \nReturning to menu..");
                                        break;
                                    }
                                    else if (singleEventUserInput != teamEvents[i, 0] && i == 4)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("The user input does not match an existing event. Returning to menu..");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ClearNreturn();
                                break;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("");
                            Console.WriteLine("Returning to menu..");
                            break;
                        }
                        break;
                    case "5":
                        Console.Clear();
                        string[] ranks = new string[] { "First", "Second", "Third", "Fourth", "Fifth" };    // Holds the positions to be used later.
                        Console.WriteLine("New Tournament\n");
                        Console.WriteLine("Is this a 1. Team or 2. Individual tournament?\nUnderstand that you will need to refer to the existing text files for input reference.\n");
                        var userInput = Console.ReadLine();
                        if (userInput == "1" || userInput.ToLower() == "team")  // If user wants to add a team tournament..
                        {
                            if (teamNameList.Count == 4 || teamMembers[4, 5] != null || teamEvents[5, 2] != null)
                            {
                                int[] teamScores = new int[] {0, 0, 0, 0};  // Holds all the team scores. Positions are relevant to which team (far left position belongs to team one, etc.).
                                int[] teamPointValues = new int[] { 6, 4, 2 };  // Holds the scores to be given dependent on the team's position (first, second, third).
                                for (var i = 0; i < 5; i++) // Loops 5 times for 5 events.
                                {
                                    Console.Clear();
                                    Console.WriteLine("\n" + ranks[i] + " team event: " + teamEvents[i,0] + "\n");    // Displays the event in question.

                                    for (var j = 0; j < 3;) // Loops 3 times for 3 team positions.
                                    {
                                        Console.WriteLine("Which team came " + ranks[j].ToLower() + " in this event?\n"); // Changes depending on First / Second / Third place.
                                        userInput = Console.ReadLine();
                                        for (var h = 0; h < teamNameList.Count; h++)    // Loops for as many team names exist.
                                        {
                                            if (userInput.ToLower() == teamNameList[h].ToLower())   // If the user's selection is equal to an existing team..
                                            {
                                                teamScores[h] += teamPointValues[j];    // The team score position that is equal to the team name position is given a certain value of points.
                                                Console.WriteLine("\n" + teamPointValues[j] + " points given to " + teamNameList[h] + "!");    // Outputs what team achieved points.
                                                j++;    // Only moves onto the next team position 
                                                break;
                                            }
                                            else if (userInput != teamNameList[h] && h == 3)
                                            {
                                                Console.WriteLine("This is not a valid team. Please enter a valid team."); // If no team exists there, informs the user.
                                                break;
                                            }
                                        }
                                    }
                                    Console.WriteLine("\nPlease enter any key to move onto the next event."); // Allows the user to reaffirm which teams scored before clearing the console to show the next event.
                                    Console.ReadKey();
                                }
                                Console.Clear();
                                using (var writeTeamTournament = new StreamWriter(teamTournamentPath))  // This section of code generates the list of individuals who partook in the event in a text file for future reference.
                                {
                                    writeTeamTournament.WriteLineAsync("Each team and its members of the tournament and their score: ");  // A section which neatens out how the text file will look.
                                    int maxScore = teamScores.Max();    // A variable takes in the highest score out of all of them.
                                    var winner = "";    // A variable to be used later.
                                    for (var i = 0; i < teamNameList.Count; i++) // Loops for as many team names exist.
                                    {
                                        if (teamScores[i] == maxScore)  // If the team score being reviewed equals max score..
                                        {
                                            winner = teamNameList[i];   // Winner is equal to the team name which is in the same position as the score.
                                            break;
                                        }
                                    }
                                    for (int i = 0; i < teamNameList.Count; i++)    // Loops for as many team names exist.
                                    {
                                        writeTeamTournament.Write("Team: " + teamNameList[i] + ", "); // Writes the team's name.
                                        writeTeamTournament.Write("Members: "); 
                                        for (int j = 0; j < 5; j++)
                                            writeTeamTournament.Write(teamMembers[i, j] + ", "); // Writes all the members of the team.
                                        writeTeamTournament.Write("Score: " + teamScores[i]);    // Writes the score for the team.
                                        writeTeamTournament.Write("\n");                         // Starts a new line.
                                    }
                                    writeTeamTournament.Write(winner + " was the winning team! Congratulations!"); // Outputs the winning team.
                                    writeTeamTournament.Flush();   // Closes the text file, so that no errors are caused with it existing in the background and being unreadable.
                                    writeTeamTournament.Close();
                                }
                                Console.WriteLine("Tournament successfully registed. \nTeam names, members, scores and the winner have been saved to Team Tournament Results.txt, in the Program Text Files folder.\n Please save this file elsewhere if you intend to do multiple tournaments.");
                            }
                        }
                        else if (userInput == "2" || userInput.ToLower() == "individual")   // Many sections in individual are identical to team, but I will comment on areas which are different.
                        {
                            if (singleNameList.Count >= 5 || singleNameList.Count <= 20 || singleEvents[5, 2] != null)  // If all criteria are met to allow data input..
                            {
                                int[] singleScores = new int[] {};  // Creates the single score list to be used later. Note the undeclared size.
                                Array.Resize(ref singleScores, singleScores.Length + singleNameList.Count); // Resizes the array to be equal to the number of names in singleNameList.
                                for (int i = 0; i < singleScores.Length; i++)
                                {
                                    singleScores[i] = 0; // Fills in all sections of the array with 0.
                                }
                                int[] singlePointValues = new int[] { 10, 8, 6, 4, 2 }; // Due to there being 5 point achievers, 5 scores are held.
                                for (var i = 0; i < 5; i++)
                                {
                                    Console.WriteLine("\n" + ranks[i] + " team event: " + singleEvents[i, 0] + "\n");
                                    for (var j = 0; j < 5;)
                                    {
                                        Console.WriteLine("Which student came " + ranks[j].ToLower() + " in this event?");
                                        Console.WriteLine("");
                                        userInput = Console.ReadLine();
                                        for (var h = 0; h < singleNameList.Count; h++)
                                        {
                                            if (userInput == singleNameList[h])
                                            {
                                                Console.WriteLine("");
                                                singleScores[h] += singlePointValues[j];
                                                Console.WriteLine(singlePointValues[j] + " points given to " + singleNameList[h] + "!");
                                                j++;
                                                break;
                                            }
                                            else if (userInput != singleNameList[h] && h == singleNameList.Count - 1)
                                            {
                                                Console.WriteLine("This is not a valid team. Please enter a valid team.");
                                                break;
                                            }
                                        }
                                    }
                                }
                                Console.Clear();
                                using (var writeSingleTournament = new StreamWriter(singleTournamentPath))  // This section of code generates the list of individuals who partook in the event in a text file for future reference.
                                {
                                    writeSingleTournament.WriteLineAsync("Each participant of the tournament and their score: ");  // A section which neatens out how the text file will look.
                                    int maxScore = singleScores.Max();
                                    var winner = "";
                                    for (var i = 0; i < singleNameList.Count - 1; i++)
                                    {
                                        if (singleScores[i] == maxScore)
                                        {
                                            winner = singleNameList[i];
                                            break;
                                        }
                                    }
                                    for (int i = 0; i < singleNameList.Count - 1; i++)
                                    {
                                        writeSingleTournament.Write(singleNameList[i] + ", ");   // Rather than printing team, it will print each individual and their score.
                                        writeSingleTournament.Write("score: " + singleScores[i]);
                                        writeSingleTournament.Write("\n");
                                    }
                                    writeSingleTournament.Write(winner + " was the winner! Congratulations!");
                                    writeSingleTournament.Flush();   // Closes the text file, so that no errors are caused with it existing in the background and being unreadable.
                                    writeSingleTournament.Close();
                                }
                                Console.WriteLine("Tournament successfully registed.\nIndividual names, scores and the winner have been saved to Single Tournament Results.txt, in the Program Text Files folder.\n Please save this file elsewhere if you intend to do multiple tournaments.");
                            }
                        }
                        else
                        {
                            ClearNreturn();
                            break;
                        }
                        break;

                }
            }
        }

        public static void PrintArray(List<String> arrayToPrint)
        {
            foreach (var v in arrayToPrint)
                Console.WriteLine(v);
        }

        public static String[] SplitList(string nameUserInput)
        {
            return nameUserInput.Split(',');
        }

        public static void WriteToTeamMembersText(List<string> tempTeamNameList, string[,] tempTeamMembers, string tempTeamMemberPath)
        {
            using (var writeTeamMembers = new StreamWriter(tempTeamMemberPath))  // This section of code generates the list of team members in a text file for future reference.
            {
                writeTeamMembers.WriteLineAsync("Team Members in Descending Order: ");  // A section which neatens out how the text file will look.
                for (int i = 0; i < 4; i++)
                {
                    writeTeamMembers.Write("All members of " + tempTeamNameList[i] + ": "); // Clarifies which students are part of which team.
                    for (int j = 0; j < 5; j++)
                    {
                        writeTeamMembers.Write(tempTeamMembers[i, j] + ", ");   // Prints out all 5 students in a team before looping to the next team.
                    }
                    writeTeamMembers.Write("\n");
                }
                writeTeamMembers.Flush();   // Closes the text file, so that no errors are caused with it existing in the background and being unreadable.
                writeTeamMembers.Close();
            }
        }

        public static void WriteToTeamEventText(string[,] tempTeamEvents, string tempTeamEventPath)
        {
            using (var writeTeamEvents = new StreamWriter(tempTeamEventPath))  // This section of code generates the list of team events in a text file for future reference.
            {
                writeTeamEvents.WriteLineAsync("Events and their categories in Descending Order: ");  // A section which neatens out how the text file will look.
                for (int i = 0; i < 5; i++)
                {
                    writeTeamEvents.Write(tempTeamEvents[i, 0] + ", ");   // Prints out all 5 team events and categories before looping to the next event.
                    writeTeamEvents.Write(tempTeamEvents[i, 1]);
                    writeTeamEvents.Write("\n");
                }
                writeTeamEvents.Flush();   // Closes the text file, so that no errors are caused with it existing in the background and being unreadable.
                writeTeamEvents.Close();
            }
        }

        public static void ClearNreturn()
        {
            Console.Clear();
            Console.WriteLine("\nReturning to menu..");
        }

        public static void WriteToTextFile(string tempNamePath, string tempFormality, List<string> tempNameList)
        {
            if (!File.Exists(tempNamePath))
            {
                File.Create(tempNamePath).Dispose();                // Creates the file if non-existant.
                File.WriteAllText(tempNamePath, tempFormality);     // Writes a heading text to the file.
                File.AppendAllLines(tempNamePath, tempNameList);    // Writes individuals to file for future use.
            }
            else
            {
                File.WriteAllText(tempNamePath, String.Empty);      // Empties existing file.
                File.WriteAllText(tempNamePath, tempFormality);     // Writes a heading text to the file.
                File.AppendAllLines(tempNamePath, tempNameList);    // Writes individuals to file.
            }
        }
    }
}
