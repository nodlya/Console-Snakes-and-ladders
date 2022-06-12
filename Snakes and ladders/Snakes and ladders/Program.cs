using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Snakes_and_ladders
{
    class Program
    {
        public static Player NewPlayer { get; private set; }
        static void Main(string[] args)
        {
            string settings;
            do
            {
                Console.WriteLine("Press y if you want automated game or n if you want push buttons");
                settings = Console.ReadLine();
            } while (settings != "y" && settings !="n");

            Console.ForegroundColor = ConsoleColor.White;
            NewPlayer = new Player();
            Game game = new Game(settings);
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("It's your turn!");
                if (!game.automated) Console.ReadKey();
                NewPlayer.PlayerLocation = game.PlayTurn(NewPlayer.PlayerLocation);
                if (NewPlayer.PlayerLocation == 100) Game.Notify(Game.Event.Victory);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("It's your opponent's turn!");
                NewPlayer.OpponentLocation = game.PlayTurn(NewPlayer.OpponentLocation);
                if (NewPlayer.OpponentLocation == 100) Game.Notify(Game.Event.Defeat);
            }
            
        }
    }

    public class Player
    {
        public int PlayerLocation { get; set; }
        public int OpponentLocation { get; set; }

    }

    public class Roll
    {
        Random random = new Random();
        public Tuple<int, int> DiceResult { get; private set; }
        public int Sum { get; private set; }
        public Roll(int position)
        {
            Sum = position;
            do
            {
                DoRoll();
                Sum += DiceResult.Item1 + DiceResult.Item2;
                Console.WriteLine("Got " + DiceResult.Item1 + " + " + DiceResult.Item2);
                Console.WriteLine("Position is " + Sum);

                if (Sum > 100)
                {
                    Sum = 100 - (Sum - 100);
                    Game.Notify(Game.Event.Bounce);
                    Console.WriteLine("New position is " + Sum);
                }

                if (Game.Ladders.ContainsKey(Sum)) 
                {
                    Sum = Game.Ladders[Sum];
                    Game.Notify(Game.Event.StepOnLadder);
                    Console.WriteLine("New position is " + Sum);
                }

                if (Game.Snakes.ContainsKey(Sum))
                {
                    Sum = Game.Snakes[Sum];
                    Game.Notify(Game.Event.StepOnSnake);
                    Console.WriteLine("New position is " + Sum);
                }

                if (DiceResult.Item2 == DiceResult.Item1) Game.Notify(Game.Event.ExtraRoll);
                
            } while (DiceResult.Item1 == DiceResult.Item2);
        }

        private void DoRoll() =>
            DiceResult = new Tuple<int, int>(random.Next(1, 7), random.Next(1, 7));
        
    }

    public class Game
    {
        public readonly bool automated;
        public static Dictionary<int, int> Ladders = new Dictionary<int, int>
        {
            {2,38 },
            {7,14 },
            {8,31 },
            {15,26 },
            {21,42 },
            {28,84 },
            {36,44 },
            {51,67 },
            {71,91 },
            {78,98 },
            {87,94 }
        };

        public static Dictionary<int, int> Snakes = new Dictionary<int, int>
        {
            {16,6 },
            {46,25 },
            {49,11 },
            {62,19 },
            {64,60 },
            {74,53 },
            {89,68 },
            {92,88 },
            {95,76 },
            {99,80 }
        };

        public enum Event
        {
            ExtraRoll,
            StepOnSnake,
            StepOnLadder,
            Bounce,
            Victory,
            Defeat
        }
        
        public Game(string type)
        {
            Console.WriteLine("Game starts");
            if (type == "y") automated = true;
            else automated = false;
        }

        public int PlayTurn(int whoseTurn)
        {
            Roll newRoll = new Roll(whoseTurn);
            return newRoll.Sum;
        }

        public static void Notify(Event @event)
        {
            string notification;
            switch (@event)
            {
                case Event.ExtraRoll:
                    notification = "Do extra roll"; break;
                case Event.StepOnLadder:
                    notification = "Stepped on a ladder"; break;
                case Event.StepOnSnake:
                    notification = "Stepped on a snake"; break;
                case Event.Bounce:
                    notification = "Too rolled"; break;
                case Event.Victory:
                    notification = "You won!"; break;
                case Event.Defeat:
                    notification = "You lost("; break;
                default: return;
            }

            Console.WriteLine(notification);
            if (@event == Event.Defeat || @event == Event.Victory)
                EndGame();
        }

        private static void EndGame()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to end");
            Console.ReadKey(false);
                Environment.Exit(0);
        }

    }
}
  
    