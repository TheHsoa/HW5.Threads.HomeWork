using System;

namespace ThreadsBattle
{
    public static class Calculating
    {
        private static readonly int Seed = Environment.TickCount;
        private static readonly Random Random = new Random(Seed);

        // Определение числа на простоту взял тут https://habrahabr.ru/post/205318/
        public static bool IsPrime(long number)
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
    }
}