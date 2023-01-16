namespace NUPoker.Services.Engine.Helper
{
    public static class CombinationHelper
    {
        public static long Combination(int n, int r)
        {
            return Permutation(n, r) / Factorial(r);
        }

        public static long Permutation(int n, int r)
        {
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }
}
