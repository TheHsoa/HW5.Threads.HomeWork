using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsBattle
{
    public class Program
    {
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());
        private static readonly Action<int> RandomSleepAction = x => Thread.Sleep(Random.Next(x));
        private const int MaxSleepingTime = 1000;

        public static void Main(string[] args)
        {
            var options = new Options();

            if(CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var queue = new ConcurrentQueue<int>();

                MultithreadingAddRandomNumbersInQueue(queue, options.N, options.A, options.B);
                MultithreadingTakeNumbersFromQueueInThreadAndDetermineIsPrime(queue, options.M);
            }
            Console.ReadKey();
        }

        private static void MultithreadingTakeNumbersFromQueueInThreadAndDetermineIsPrime(ConcurrentQueue<int> queue, int m)
        {
            Parallel.For(0, m, x => { Task.Run(() => TakeNumberFromQueueAndDetermineIsPrime(queue)); });
        }

        private static void MultithreadingAddRandomNumbersInQueue(ConcurrentQueue<int> queue, int n, int a, int b)
        {
            Parallel.For(0, n, x => { Task.Run(() => AddRandomNumberToQueue(queue, a, b)); });
        }

        private static void AddRandomNumberToQueue(ConcurrentQueue<int> queue, int a, int b)
        {
            while (true)
            {
                RandomSleepAction(MaxSleepingTime);
                queue.Enqueue(Random.Next(a, b));
            }
        }

        private static void TakeNumberFromQueueAndDetermineIsPrime(ConcurrentQueue<int> queue)
        {
            while (true)
            {
                RandomSleepAction(MaxSleepingTime);

                if (queue.TryDequeue(out var number))
                {
                    WriteIsPrimeNumber(number);
                }
            }
        }

        private static void WriteIsPrimeNumber(int number)
        {
            Console.WriteLine(Calculating.IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }
    }
}
