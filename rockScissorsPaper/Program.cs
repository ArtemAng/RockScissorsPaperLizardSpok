using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace rockScissorsPaper
{
    class Program
    {
        static void DrawMenu(string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                Console.WriteLine(i + 1 + "-" + strs[i]);
            }
            Console.WriteLine("0-Exit");
        }
        static string IsWinGame(string[] arr, int compMove, int myMove)
        {
            List<int> winners = new List<int>();
            for (int i = 0, j = myMove + 1; i < arr.Length / 2; i++, j++)
            {
                winners.Add(j >= arr.Length ? j - arr.Length : j);
            }
            return winners.IndexOf(compMove) != -1 ? "Computer win" : "You win ";
        }
        static string GetKey()
        {
            byte[] rand = new byte[16];
            RandomNumberGenerator.Create().GetBytes(rand);
            StringBuilder key = new StringBuilder();
            foreach (var el in rand)
            {
                key.Append(el.ToString("x2"));
            }
            return key.ToString();
        }
        static string GetHMAC(string key)
        {
            StringBuilder hmac = new StringBuilder();
            foreach (var el in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key.ToString())))
            {
                hmac.Append(el.ToString("x2"));
            }
            return hmac.ToString();
        }
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0 || args.Distinct().Count() != args.Length)
            {
                Console.WriteLine("Invalid parametrs\nExemple parametrs: Rock Scissors Paper Lizard Spok");

            }
            else
            {
                DrawMenu(args);

                int compMove = new Random().Next(0, args.Length);
                string key = GetKey();
                Console.WriteLine($"HMAC:\n{GetHMAC(key)}");

                Console.Write("Enter your move: ");
                int myMove = Convert.ToInt32(Console.ReadLine());
                while (myMove > args.Length)
                {
                    Console.WriteLine("Invaild operation! Try again");
                    Console.Write("Enter your move: ");
                    myMove = Convert.ToInt32(Console.ReadLine());
                }
                if (myMove == 0)
                    return;
                Console.WriteLine($"Your move is: {args[myMove-1]}\nComputer move is: {args[compMove]}");
                if (myMove-1 == compMove)
                    Console.WriteLine("Draw!");
                else Console.WriteLine(IsWinGame(args, compMove, myMove - 1));
                Console.WriteLine($"HMAC key: {key}");
            }
        }
    }
}
