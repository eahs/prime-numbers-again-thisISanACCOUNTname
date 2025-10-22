using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace PrimeNumbersAgain
{
    class Program
    {
        static void Main(string[] args)
        {
            int n, prime;
            Stopwatch timer = new Stopwatch();

            PrintBanner();
            n = GetNumber();

            timer.Start();
            prime = FindNthPrime(n);
            timer.Stop();

            Console.WriteLine($"\nToo easy.. {prime} is the nth prime when n is {n}. I found that answer in {timer.Elapsed.TotalSeconds:F3} seconds.");

            EvaluatePassingTime(timer.Elapsed);
        }

        static int FindNthPrime(int n)
        {
            if (n < 1)
            {
                throw new ArgumentException("n must be >= 1", nameof(n));
            }

            // quick returns for small n
            if (n == 1) return 2;
            if (n == 2) return 3;
            if (n == 3) return 5;
            if (n == 4) return 7;
            if (n == 5) return 11;

            // estimate an upper bound for the nth prime using p_n < n (ln n + ln ln n) for n >= 6
            double dn = n;
            int limit;
            try
            {
                double estimate = dn * (Math.Log(dn) + Math.Log(Math.Log(dn)));
                limit = (int)(estimate + 10);
            }
            catch
            {
                limit = 2000000; // fallback
            }

            // ensure minimum size
            if (limit < 100) limit = 100;

            // sieve of eratosthenes
            while (true)
            {
                bool[] isPrime = new bool[limit + 1];
                for (int i = 2; i <= limit; i++)
                {
                    isPrime[i] = true;
                }

                for (int i = 2; i * i <= limit; i++)
                {
                    if (isPrime[i])
                    {
                        for (int j = i * i; j <= limit; j += i)
                        {
                            isPrime[j] = false;
                        }
                    }
                }

                List<int> primes = new List<int>();
                for (int i = 2; i <= limit; i++)
                {
                    if (isPrime[i]) primes.Add(i);
                }

                if (primes.Count >= n)
                {
                    return primes[n - 1];
                }
            }
        }

        static int GetNumber()
        {
            int n = 0;
            while (true)
            {
                Console.Write("Which nth prime should I find?: ");

                string num = Console.ReadLine();
                if (Int32.TryParse(num, out n))
                {
                    if (n >= 1) return n;
                    Console.WriteLine($"{num} is not a valid number. Please enter an integer greater than or equal to 1.\n");
                    continue;
                }

                Console.WriteLine($"{num} is not a valid number.  Please try again.\n");
            }
        }

        static void PrintBanner()
        {
            Console.WriteLine(".................................................");
            Console.WriteLine(".#####...#####...######..##...##..######...####..");
            Console.WriteLine(".##..##..##..##....##....###.###..##......##.....");
            Console.WriteLine(".#####...#####.....##....##.#.##..####.....####..");
            Console.WriteLine(".##......##..##....##....##...##..##..........##.");
            Console.WriteLine(".##......##..##..######..##...##..######...####..");
            Console.WriteLine(".................................................\n\n");
            Console.WriteLine("Nth Prime Solver O-Matic Online..\nGuaranteed to find primes up to 2 million in under 30 seconds!\n\n");

        }

        static void EvaluatePassingTime(TimeSpan elapsed)
        {
            Console.WriteLine("\n");
            Console.Write("Time Check: ");

            if (elapsed.TotalSeconds <= 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pass");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fail");
            }
        }
    }
}
