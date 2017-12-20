using System;
using System.Collections.Concurrent;
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
        private static readonly Action<int> RandomSleepAction = x => Thread.Sleep(Random.Next(x));
        private const int MaxSleepingTime = 1000;

        public static void Main(string[] args)
        {
           ParseArgs(args);
            var queue = new ConcurrentQueue<long>();

            AsyncAddRandomNumbersInQueue(queue, N);
            AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(queue, M);
            Console.ReadKey();
        }

        private static void ParseArgs(string[] args)
        {
            try
            {
                N = 2;//Convert.ToInt32(args[0]);
                M = 10;//Convert.ToInt32(args[1]);
                A = 0;//Convert.ToInt32(args[2]);
                B = 10; //Convert.ToInt32(args[3]);
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

        private static void AsyncTakeNumbersFromQueueInThreadAndDetermineIsPrime(ConcurrentQueue<long> queue, int m)
        {
            Parallel.For(0, m, x => { Task.Run(() => TakeNumberFromQueueAndDetermineIsPrime(queue)); });
        }

        private static void AsyncAddRandomNumbersInQueue(ConcurrentQueue<long> queue, long n)
        {
            Parallel.For(0, n, x => { Task.Run(() => AddRandomNumberToQueue(queue)); });
        }

        private static void AddRandomNumberToQueue(ConcurrentQueue<long> queue)
        {
            while (true)
            {
                RandomSleepAction(MaxSleepingTime);
                queue.Enqueue(Random.Next(A, B));
            }
        }

        private static void TakeNumberFromQueueAndDetermineIsPrime(ConcurrentQueue<long> queue)
        {
            while (true)
            {
                RandomSleepAction(MaxSleepingTime);

                long number;

                if (queue.TryDequeue(out number))
                {
                    WriteIsPrimeNumber(number);
                }
            }
        }

        private static void WriteIsPrimeNumber(long number)
        {
            Console.WriteLine(Calculating.IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }
    }
}
