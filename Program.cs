using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsBattle
{
    public class Program
    {
        private static int N;
        private static int A;
        private static int B;
        private static int M;
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public static void Main(string[] args)
        {
           ParseArgs(args);
            var queue = new Queue<long>();

            AsyncAddRandomNumbersInQueue(queue, N);
            AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(queue, M);
            Console.ReadKey();
        }

        private static void ParseArgs(string[] args)
        {
            try
            {
                N = Convert.ToInt32(args[0]);
                N = Convert.ToInt32(args[0]);
                M = Convert.ToInt32(args[1]);
                A = Convert.ToInt32(args[2]);
                B = Convert.ToInt32(args[3]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Заданы не все входные параметры");
            }
            catch (FormatException)
            {
                Console.WriteLine("Входные параметры должны быть целыми числами");
            }
            catch (Exception)
            {
                Console.WriteLine("Неверно заданы входные параметры");
            }

        }

        private static void AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(Queue<long> queue, int m)
        {
            Parallel.For(0, m, x => { Task.Factory.StartNew(() => TakeNumberFromQueueAndDetermineIsPrime(queue)); });
        }

        private static void AsyncAddRandomNumbersInQueue(Queue<long> queue, long n)
        {
            Parallel.For(0, n, x => { Task.Factory.StartNew(() => AddRandomNumberToQueue(queue)); });
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

        // Определение числа на простоту взял тут https://habrahabr.ru/post/205318/
        private static bool IsPrime(long number)
        {
            if (number <= 1)
                return false;
            if (number == 2)
                return true;

            for (var i = 0; i < 100; i++)
            {
                var a = Random.Next() % (number - 2) + 2;
                if (Gcd(a, number) != 1 || Pows(a, number - 1, number) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private static long Mul(long a, long b, long m)
        {
            if (b == 1)
                return a;
            if (b % 2 != 0)
            {
                return (Mul(a, b - 1, m) + a) % m;
            }
            var t = Mul(a, b / 2, m);
            return (2 * t) % m;
        }

        private static long Pows(long a, long b, long m)
        {
            if (b == 0)
                return 1;
            if (b % 2 != 0)
            {
                return (Mul(Pows(a, b - 1, m), a, m)) % m;
            }
            var t = Pows(a, b / 2, m);
            return Mul(t, t, m) % m;
        }

        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                var a1 = a;
                a = b;
                b = a1 % b;
            }
            return a;
        }

        private static void WriteIsPrimeNumber(long number)
        {
            Console.WriteLine(IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }
    }
}
