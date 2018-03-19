using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace ThreadsBattle
{
    public static class Program
    {
        private const int MaxSleepingTime = 1000;
        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var queue = new ConcurrentQueue<int>();

                Action enqueue = () => queue.Enqueue(Random.Next(options.A, options.B));
                enqueue.WithSleep().InfinityJob().StartJobs(options.N);
                TakeNumberFromQueueAndDetermineIsPrimeAction(queue).WithSleep().InfinityJob().StartJobs(options.M);

/*                StartJobs(options.N,
                    () => InfinityJob(
                        () => JobWithSleep(
                            () => queue.Enqueue(Random.Next(options.A, options.B)))));
                StartJobs(options.M,
                    () => InfinityJob(
                        () => JobWithSleep(
                            () => TakeNumberFromQueueAndDetermineIsPrime(queue))));*/
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

        private static Action WithSleep(this Action job)
        {
            return () =>
            {
                job();
                Sleep(GetWaitPeriod());
            };
        }

        private static Action InfinityJob(this Action job)
        {
            return () =>
            {
                while (true) job();
            };
        }

        private static void StartJobs(this Action job, int num)
        {
            for (var i = 0; i < num; i++) Task.Run(job);
        }

        private static Action TakeNumberFromQueueAndDetermineIsPrimeAction(ConcurrentQueue<int> queue)
        {
            return () =>
            {
                if (queue.TryDequeue(out var number)) WriteIsPrimeNumber(number);
            };
        }

        private static void WriteIsPrimeNumber(int number)
        {
            Console.WriteLine(Calculating.IsPrime(number) ? $"Число {number} простое" : $"Число {number} не простое");
        }
    }
}