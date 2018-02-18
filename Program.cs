using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace ThreadsBattle
{
    public class Program
    {
        private const int MaxSleepingTime = 1000;
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var queue = new ConcurrentQueue<int>();
                StartJobs(options.N,
                    () => InfinityJob(
                        () => JobWithSleep(
                            () => queue.Enqueue(Random.Next(options.A, options.B)))));
                StartJobs(options.M,
                    () => InfinityJob(
                        () => JobWithSleep(
                            () => TakeNumberFromQueueAndDetermineIsPrime(queue))));
            }

            Console.ReadKey();
        }

        private static void Sleep(int wait)
        {
            Thread.Sleep(wait);
        }

        private static int GetWaitPeriod()
        {
            return Random.Next(MaxSleepingTime);
        }

        private static void JobWithSleep(Action job)
        {
            job();
            Sleep(GetWaitPeriod());
        }

        private static void InfinityJob(Action job)
        {
            while (true) job();
        }

        private static void StartJobs(int num, Action job)
        {
            for (var i = 0; i < num; i++) Task.Run(job);
        }

        private static void TakeNumberFromQueueAndDetermineIsPrime(ConcurrentQueue<int> queue)
        {
            if (queue.TryDequeue(out var number)) WriteIsPrimeNumber(number);
        }

        private static void WriteIsPrimeNumber(int number)
        {
            Console.WriteLine(Calculating.IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }
    }
}