using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsBattle
{
    class Program
    {
        private static long N;
        private static int A;
        private static int B;
        private static long M;
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        static void Main(string[] args)
        {
            var argss = new [] { "5", "10", "0", "10" };
            ParseArgs(argss);
            var queue = new Queue<long>();

            AsyncAddRandomNumbersInQueue(queue, N);
            AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(queue, M);
            Console.ReadKey();
        }

        private static void ParseArgs(string[] args)
        {
            N = Convert.ToInt64(args[0]);
            M = Convert.ToInt64(args[1]);
            A = Convert.ToInt32(args[2]);
            B = Convert.ToInt32(args[3]);
        }

        private static void AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(Queue<long> queue, long m)
        {
            for (var i = 0; i < m; i++)
            {
                Task.Factory.StartNew(() => TakeNumberFromQueueAndDetermineIsPrime(queue));
            }
        }

        private static void AsyncAddRandomNumbersInQueue(Queue<long> queue, long n)
        {
            for (var i = 0; i < n; i++)
            {
                Task.Factory.StartNew(() => AddRandomNumberToQueue(queue));
            }
        }

        private static void AddRandomNumberToQueue(Queue<long> queue)
        {
            while (true)
            {
                Thread.Sleep(Random.Next(1000));
                queue.Enqueue(Random.Next(A, B));
            }
        }

        private static void TakeNumberFromQueueAndDetermineIsPrime(Queue<long> queue)
        {
                 while (true)
                 {
            Thread.Sleep(Random.Next(1000));
            try
            {
                var number = queue.Dequeue();
                WriteIsPrimeNumber(number);

            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                    Thread.Sleep(Random.Next(5000));
            }

                }
        }

        private static bool IsPrime(long number)
        {
            if (number <= 1)
                return false;
            if (number == 2)
                return true;

            for (var i = 0; i < 100; i++)
            {
                var a = Random.Next() % (number - 2) + 2;
                if (GCD(a, number) != 1)
                    return false;
                if (pows(a, number - 1, number) != 1)
                    return false;
            }
            return true;
        }

        static long mul(long a, long b, long m)
        {
            if (b == 1)
                return a;
            if (b % 2 == 0)
            {
                long t = mul(a, b / 2, m);
                return (2 * t) % m;
            }
            return (mul(a, b - 1, m) + a) % m;
        }

        static long pows(long a, long b, long m)
        {
            if (b == 0)
                return 1;
            if (b % 2 == 0)
            {
                long t = pows(a, b / 2, m);
                return mul(t, t, m) % m;
            }
            return (mul(pows(a, b - 1, m), a, m)) % m;
        }

        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private static void WriteIsPrimeNumber(long number)
        {
            Console.WriteLine(IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }


    }
}
