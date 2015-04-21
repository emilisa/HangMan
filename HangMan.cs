using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HangMan_Sample
{
    class HangMan_Sample
    {
        static int playerStartingPoints; // Starting points of Player are 90

        static List<string> alreadySguestedLetters; // Array in which all of the guessed letters are saved

        static bool[] revealedLettersPosition; // Bool massive which reveals or not exact letters from guessWord

        static List<string> wordsDictionary; // Dictionary with words to guess from (words can be changed if needed)

        static string guessWord; // Word that should be guessed (it is taken from wordsDictionary)

        static string sugestion; // Exactly one letter taken from user with Console.ReadLine(); method

        static string gameStatus; // Game status has three values 1.Play, 2.YOU LOST, 3.YOU WIN (which determines game states)

        static bool consist; // Bool variable which is used to determine if letter from user is consisted in guessWord or not

        static bool validInput; // Validates user input (exactly one letter should be typed)

        /// <summary>
        /// Main method all the logic is initialized here
        /// </summary>

        private static void Main()
        {
            Console.Title = "Hang Man";

            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;

            playerStartingPoints = 90; 

            alreadySguestedLetters = new List<string>() { };

            revealedLettersPosition = new bool[1];

            guessWord = GetWordFromDictionary();

            while (true)
            {
                Console.Clear();

                Printer(0, 0, "Already guested letters: ", ConsoleColor.Gray);

                AddSugestionToAlreadyGuestedLetters(sugestion);

                CheckIfPlayerWinsOrLoose(playerStartingPoints);

                Printer(0, 1, "Your Points: " + playerStartingPoints.ToString(), ConsoleColor.Gray);

                Printer(0, 2, "Game status: ", ConsoleColor.Gray);

                Printer(5, 5, "GUESS THE WORD: ", ConsoleColor.Gray);

                RevealedLettersPosition(guessWord);

                HideOrShowGuessWord(guessWord);

                Printer(5, 7, "SUGEST LETTER: ", ConsoleColor.Gray);

                StartNewGameOrClose(gameStatus);

                sugestion = Console.ReadLine();

                CheckForValidUserInput(sugestion);

                GuessWordConsistSugestion(sugestion, guessWord);

                PlayerPoints(consist);

                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Method that prints given text at given position x, y on the console window
        /// </summary>

        private static void Printer(int x, int y, string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Method that returns random word from given dictionary with words
        /// </summary>

        private static string GetWordFromDictionary()
        {
            wordsDictionary = new List<string>() { "REVOLVER", "HOME", "WINDOW", "CAT", "DOG", "TOWN" };

            Random number = new Random();

            int random = number.Next(0, wordsDictionary.Count);

            return wordsDictionary[random];
        }

        /// <summary>
        /// Method that appends all guest letters to list
        /// </summary>

        private static void AddSugestionToAlreadyGuestedLetters(string sugestion)
        {
            if (validInput == true && gameStatus == "Play")
            {
                alreadySguestedLetters.Add(sugestion.ToUpper());
            }

            for (int i = 1; i <= alreadySguestedLetters.Count; i++)
            {
                Printer(24 + (i * 2 - 1), 0, alreadySguestedLetters[i - 1] + ",", ConsoleColor.Gray);
            }
        }

        /// <summary>
        /// Method that hides ungest letters or reaveals already guest letters
        /// </summary>

        private static void HideOrShowGuessWord(string guessWord)
        {
            for (int i = 0; i < guessWord.Length; i++)
            {
                if (revealedLettersPosition[i] == true)
                {

                    Printer(i + 21, 5, guessWord[i].ToString(), ConsoleColor.Cyan);
                }

                else
                {
                    Printer(i + 21, 5, "*", ConsoleColor.Gray);
                }
            }
        }

        /// <summary>
        /// Method that checks if guess word consist sugested Letter
        /// </summary>

        private static bool GuessWordConsistSugestion(string sugestion, string guessWord)
        {
            consist = false;

            if (validInput == true)
            {
                for (int i = 0; i < guessWord.Length; i++)
                {
                    if (guessWord[i] == Convert.ToChar(sugestion.ToUpper()))
                    {
                        consist = true;

                        revealedLettersPosition[i] = true; // exact member of the array is asigned with true.
                    }
                }
            }

            return consist;
        }

        /// <summary>
        /// Method that counts player points
        /// If sugested letter IS NOT consisted in the given word to guess(guessWord)
        /// the points of player are DECREASED with 10 points
        /// If sugested letter IS consisted in the given word to guess(guessWord)
        /// the points of player are INCREASED with 10 points
        /// </summary>

        private static int? PlayerPoints(bool consist)
        {
            if (consist == false && validInput == true && gameStatus == "Play")
            {
                playerStartingPoints -= 10;
            }

            if (consist == true && validInput == true && gameStatus == "Play")
            {
                playerStartingPoints += 10;
            }

            return playerStartingPoints;
        }

        /// <summary>
        /// Method that checks winnig or losing conditions
        /// </summary>

        private static void CheckIfPlayerWinsOrLoose(int playerStartingPoints)
        {
            if (playerStartingPoints > 0)
            {
                gameStatus = "Play";

                Printer(13, 2, gameStatus, ConsoleColor.Yellow);
            }


            if (playerStartingPoints == 0)
            {
                gameStatus = "YOU LOST";

                Printer(13, 2, gameStatus, ConsoleColor.Red);
            }

            int countTrue = 0;

            for (int i = 0; i < revealedLettersPosition.Length; i++)
            {
                if (revealedLettersPosition[i] == true)
                {
                    countTrue++;
                }

                if (countTrue == revealedLettersPosition.Length)
                {
                    gameStatus = "YOU WIN";

                    Printer(13, 2, gameStatus, ConsoleColor.Green);
                }
            }
        }

        /// <summary>
        /// Method that checks input for invalid characters, numbers, signs and etc...
        /// </summary>

        private static bool CheckForValidUserInput(string sugestion)
        {
            validInput = true;

            if (sugestion.Length != 1)  //Check if input is not a string 
            {
                Printer(20, 7, "Please Type letter", ConsoleColor.Red);

                Thread.Sleep(3000);

                validInput = false;
            }

            else if (!char.IsLetter(sugestion, 0))  //Check if input is letter at all
            {
                Printer(20, 7, "Please Type letter", ConsoleColor.Red);

                Thread.Sleep(3000);

                validInput = false;
            }

            else if (alreadySguestedLetters.Contains(sugestion)) //Check if input is not already used
            {
                Printer(20, 7, "This letter is already used", ConsoleColor.Red);

                Thread.Sleep(3000);

                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        /// Method that initialize bool massive coresponding to the exact guessWord Lenght.
        /// All the members of the array are initialy asigned with false.
        /// </summary>

        private static void RevealedLettersPosition(string guessWord)
        {
            if (alreadySguestedLetters.Count == 0)
            {
                revealedLettersPosition = new bool[guessWord.Length];

                for (int i = 0; i < revealedLettersPosition.Length; i++)
                {
                    revealedLettersPosition[i] = false;
                }
            }

        }

        /// <summary>
        /// Method that according to user comand restarts or closes the application
        /// </summary>

        private static void StartNewGameOrClose(string gameStatus)
        {
            if (gameStatus == "YOU LOST" || gameStatus == "YOU WIN")
            {
                Printer(0, 18, "New Game(N) or Close(C) N/C: ", ConsoleColor.Gray);

                string comand = Console.ReadLine().ToUpper(); // reading user comand 

                if (comand != "N" && comand != "C")
                {
                    Printer(29, 18, "Please type 'N' or 'C'", ConsoleColor.Red);

                    Thread.Sleep(3000);

                    Printer(29, 18, "                      ", ConsoleColor.Gray);

                    StartNewGameOrClose(gameStatus);
                }

                else
                {
                    if (comand == "N") // user comand for starting new game (Application Restart)
                    {
                        Console.Clear();

                        Printer(5, 5, "STARTING NEW GAME...", ConsoleColor.Gray);

                        Thread.Sleep(3000);
                        
                        Main();
                    }

                    else if (comand == "C") // user comand for closing the application
                    {
                        Console.Clear();

                        Printer(5, 5, "PROGRAM IS CLOSING...", ConsoleColor.Gray);

                        Thread.Sleep(3000);

                        Environment.Exit(0);
                    }
                }
            }

        }
    }
}
