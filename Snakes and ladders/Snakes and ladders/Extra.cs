using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace A
{
    public static class Player
    {
        public static int PlayerLocation { get; set; }
        public static int OpponentLocation { get; set; }

        public static GameStatus Status { get; set; }
        public enum GameStatus
        {
            Win,
            Lose,
            Playing
        }

        public static void UserInfoHandler()
        {
            Console.WriteLine("Game over");
            Environment.Exit(0);
        }
    }

    public static class Rules
    {
        public static int Bounce(int playerLocation) => 100 - Math.Abs(playerLocation - 100);
        public static void Win()
        {
            Console.WriteLine("Вы победили");
            Environment.Exit(0);
        }
        public static void Lose()
        {
            Console.WriteLine("Вы проиграли");
            Environment.Exit(0);
        }

    }

    public class SnakesLadders
    {
        private Random random = new Random();
        //private delegate int Bounce(int playerLocation);
        //private event Bounce BounceRule;

        public Dictionary<int, int> SnakesOrLadders = new Dictionary<int, int>
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
            {87,94 },

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
        public SnakesLadders()
        {
            Player.PlayerLocation = 0;
            Player.OpponentLocation = 0;
            //Rules rules = new Rules();
            //BounceRule += rules.Bounce;

            while (true)
            {
                Console.WriteLine("You roll:");
                Player.PlayerLocation += Play();
                Thread.Sleep(4000);
                if (Player.PlayerLocation == 100) Rules.Win();

                Console.WriteLine("Your opponent rolls:");
                Player.OpponentLocation += Play();
                if (Player.PlayerLocation == 100) Rules.Lose();
                Thread.Sleep(4000);
            }
        }
        public int Play()
        {

            Tuple<int, int> tuple;
            int sum = 0;
            int rolls = 0;
            do
            {
                rolls++;
                tuple = Roll();
                sum += tuple.Item1 + tuple.Item2;
                Console.WriteLine("Roll " + tuple.Item1 + " + " + tuple.Item2);

                if (rolls > 1) Console.WriteLine("Extra roll");

                if (sum > 100)
                {
                    Console.WriteLine("Oops! Bounce!");
                    sum = Rules.Bounce(sum);
                }

                if (SnakesOrLadders.ContainsKey(sum))
                {
                    if (sum > SnakesOrLadders[sum]) Console.WriteLine("You stepped on a snake!");
                    else Console.WriteLine("You stepped on a ladder!");
                    sum = SnakesOrLadders[sum];
                }

                Console.WriteLine("Position is " + sum);
            }
            while (tuple.Item1 == tuple.Item2);

            return sum;
        }

        private Tuple<int, int> Roll() => Tuple.Create(random.Next(1, 7), random.Next(1, 7));

    }
}


