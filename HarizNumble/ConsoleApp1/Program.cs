using System;

class Player
{
    public string Name { get; }

    public Player(string name)
    {
        Name = name;
    }

    public int GuessNumber()
    {
        int guess = 0; // Initialize with a default value
        bool isValidGuess = false;
        do
        {
            Console.Write($"{Name}, enter your guess (between 1 and 100, or 999 to end the game): ");
            string input = Console.ReadLine();
            if (input == "999")
            {
                Console.WriteLine("Game ended by player.");
                Environment.Exit(0);
            }
            else if (int.TryParse(input, out guess) && guess >= 1 && guess <= 100)
            {
                isValidGuess = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 100, or 999 to end the game.");
            }
        } while (!isValidGuess);
        return guess;
    }
}

class NumberGenerator
{
    private static readonly Random random = new Random();

    public static int GenerateNumber()
    {
        return random.Next(1, 101);
    }
}

class Game
{
    private readonly Player player;
    private int playerScore;
    private int botScore;

    public Game(Player player)
    {
        this.player = player;
    }

    public void Play()
    {
        Console.WriteLine($"Welcome to the Number Guessing Game, {player.Name}!");

        for (int round = 1; round <= 4; round++)
        {
            Console.WriteLine($"\nRound {round}");

            int targetNumber = NumberGenerator.GenerateNumber();

            int playerGuess = player.GuessNumber();
            int botGuess = NumberGenerator.GenerateNumber();

            Console.WriteLine($"{player.Name} guessed: {playerGuess}");
            Console.WriteLine($"Bot guessed: {botGuess}");

            int playerDifference = Math.Abs(playerGuess - targetNumber);
            int botDifference = Math.Abs(botGuess - targetNumber);

            if (playerGuess == targetNumber)
            {
                Console.WriteLine($"{player.Name} guessed the correct number and earns a point!");
                playerScore++;
            }
            else if (botGuess == targetNumber)
            {
                Console.WriteLine("Bot guessed the correct number and earns a point!");
                botScore++;
            }
            else
            {
                if (playerDifference < botDifference)
                {
                    Console.WriteLine($"{player.Name} was closer to the correct number and earns a point!");
                    playerScore++;
                }
                else if (botDifference < playerDifference)
                {
                    Console.WriteLine("Bot was closer to the correct number and earns a point!");
                    botScore++;
                }
                else
                {
                    Console.WriteLine("It's a tie. No points awarded.");
                }
            }
        }

        Console.WriteLine($"\nGame over!");
        Console.WriteLine($"{player.Name} scored: {playerScore}");
        Console.WriteLine($"Bot scored: {botScore}");

        if (playerScore > botScore)
        {
            Console.WriteLine($"{player.Name} wins!");
        }
        else if (botScore > playerScore)
        {
            Console.WriteLine("Bot wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Number Guessing Game!");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName);
        Game game = new Game(player);
        game.Play();
    }
}
