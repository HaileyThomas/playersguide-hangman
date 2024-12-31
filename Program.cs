// Create a hangman game 
// game picks a word at random from a list
// game's state is displayed to the player, as shown below
// player can pick a letter. If they pick a letter they already chose, pick again
// the game should update it's state based on the letter the player picked
// the game needs to detect a win for the player (when all leterrs have been guessed)
// the game needs to detect a loss for the player (out of incorrect guesses)
// Example: C _ T | Remaining: 5 | Incorrect: PSRLM | Last Guess: T

Console.Title = "Hangman";

Game game = new Game();

public class Game
{
    public Word Word { get; set; }
    public Guesses Guesses { get; set; }
    int maxAttempts = 7;

    public Game()
    {
        Console.WriteLine("Welcome to Hangman!");
        Word = new Word();
        Guesses = new Guesses();
        Play();
    }

    // main game loop
    private void Play()
    {
        while (Guesses.IncorrectGuesses.Count < maxAttempts && Word.isWordComplete == false)
        {
            DisplayState();

            char guess = AskGuess("Please guess a letter: ");

            bool isCorrect = Word.CheckGuess(guess);

            Guesses.AddGuess(guess, isCorrect);

            Word.CheckComplete();
        }

        EndGame();
    }

    public void DisplayState()
    {
        int remaining = maxAttempts - Guesses.IncorrectGuesses.Count;
        string incorrectList = string.Join("", Guesses.IncorrectGuesses).ToUpper();

        Console.WriteLine($"Word: {Word.RevealedWord.ToUpper()} | Remaining Guesses: {remaining} | Incorrect: {incorrectList} | Last Guess: {Guesses.LastGuess.ToString().ToUpper()}");
        Console.WriteLine("--------------------------------------------------------------------------------");
    }

    public char AskGuess(string question)
    {
        Console.Write(question);
        string answer = Console.ReadLine().ToLower();

        int currentLineCursor = Console.CursorTop; // Get the current cursor line
        Console.SetCursorPosition(0, currentLineCursor - 1); // Move up one line
        Console.Write(new string(' ', Console.WindowWidth)); // Overwrite with spaces
        Console.SetCursorPosition(0, currentLineCursor - 1); // Reset cursor to that line

        if (answer == null)
        {
            Console.WriteLine("Invalid input, try again.");
            return AskGuess(question);
        }

        if (answer.Length > 1 || answer.Length < 1)
        {
            Console.WriteLine("Invalid input. Guess can only be a single letter.");
            return AskGuess(question);
        }

        char parsedGuess = Convert.ToChar(answer);
        bool beenGuessed = Guesses.HasAlreadyGuessed(parsedGuess);

        if (beenGuessed)
        {
            Console.WriteLine("You have already guessed that letter. Please try again.");
            return AskGuess(question);
        }

        return parsedGuess;
    }

    public void EndGame()
    {
        if (Word.isWordComplete)
        {
            Console.WriteLine("Congratulations! You've guessed the word!");
        }
        else
        {
            Console.WriteLine("You've run out of guesses. Better luck next time!");
        }

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}

public class Word
{
    private static readonly string[] WordList = {
        "house",
        "garden",
        "apple",
        "wizard",
        "magic",
        "forest",
        "castle",
        "dragon",
        "knight",
        "sword",
        "shield",
        "potion",
        "monster",
        "treasure",
        "adventure",
        "journey",
        "hero",
        "villain",
        "princess",
        "kingdom",
        "quest",
        "mystery",
        "legend",
        "guardian",
        "crypt",
        "shadow",
        "phoenix"
    };

    public string TargetWord;
    public string RevealedWord;
    public bool isWordComplete;

    public Word()
    {
        TargetWord = GetRandomWord();
        RevealedWord = new string('_', TargetWord.Length); // Initializes with underscores
        isWordComplete = false;
    }

    private static string GetRandomWord()
    {
        Random random = new Random();
        return WordList[random.Next(WordList.Length)];
    }

    public bool CheckGuess (char guess)
    {
        if (TargetWord.Contains(guess))
        {
            char[] revealedArray = RevealedWord.ToCharArray(); // Convert RevealedWord to a char array

            for (int i = 0; i < TargetWord.Length; i++)
            {
                if (TargetWord[i] == guess)
                {
                    revealedArray[i] = guess; // Update the revealed word at the correct index
                }
            }

            RevealedWord = new string(revealedArray); // Reconstruct RevealedWord from the updated char array

            return true;
        }

        return false;
    }
    
    public void CheckComplete()
    {
        if (RevealedWord == TargetWord)
        {
            isWordComplete = true;
        }
    }
}

public class Guesses
{
    public List<char> CorrectGuesses { get; set; }
    public List<char> IncorrectGuesses { get; set; }
    public char LastGuess { get; set; }

    public Guesses()
    {
        CorrectGuesses = new List<char>(); // Initialize empty list
        IncorrectGuesses = new List<char>();
        LastGuess = '-';
    }

    public bool HasAlreadyGuessed (char guess)
    {
        if (CorrectGuesses.Contains(guess) || IncorrectGuesses.Contains(guess))
        {
            return true;
        }

        return false;
    }

    public void AddGuess(char guess, bool isCorrect)
    {
        LastGuess = guess;

        if (isCorrect)
        {
            CorrectGuesses.Add(guess);
        }
        else
        {
            IncorrectGuesses.Add(guess);
        }
    }
}