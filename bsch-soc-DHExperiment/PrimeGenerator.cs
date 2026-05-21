using System;
using System.Numerics;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Handles prime number generation using the Miller-Rabin probabilistic
    /// primality test. See the discrete logarithm tutorial for an explanation
    /// of why efficient prime generation requires a probabilistic approach.
    /// </summary>
    public class PrimeGenerator
    {
        private readonly Random _random;
        private const int MillerRabinRounds = 20;

        public PrimeGenerator(int seed = 42)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// Generates a prime number of approximately the specified bit length.
        /// Larger bit lengths take longer to generate; this is expected and
        /// is itself an interesting observation for your investigation.
        /// </summary>
        public BigInteger GeneratePrime(int bitLength)
        {
            if (bitLength < 8)
                throw new ArgumentException("Bit length must be at least 8.");

            while (true)
            {
                BigInteger candidate = GenerateRandomOdd(bitLength);
                if (IsMillerRabinPrime(candidate, MillerRabinRounds))
                    return candidate;
            }
        }

        /// <summary>
        /// Finds a primitive root for the given prime p.
        /// A primitive root g ensures that g^k mod p cycles through all
        /// values from 1 to p-1, which is required for Diffie-Hellman security.
        /// </summary>
        public BigInteger FindPrimitiveRoot(BigInteger p)
        {
            BigInteger phi = p - 1;
            BigInteger[] primeFactors = GetPrimeFactors(phi);

            for (BigInteger g = 2; g < p; g++)
            {
                bool isPrimitiveRoot = true;

                foreach (BigInteger factor in primeFactors)
                {
                    if (BigInteger.ModPow(g, phi / factor, p) == 1)
                    {
                        isPrimitiveRoot = false;
                        break;
                    }
                }

                if (isPrimitiveRoot)
                    return g;
            }

            throw new Exception("No primitive root found.");
        }

        private bool IsMillerRabinPrime(BigInteger n, int rounds)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;

            BigInteger d = n - 1;
            int r = 0;
            while (d % 2 == 0) { d /= 2; r++; }

            for (int i = 0; i < rounds; i++)
            {
                BigInteger a = GenerateRandomInRange(2, n - 2);
                BigInteger x = BigInteger.ModPow(a, d, n);

                if (x == 1 || x == n - 1) continue;

                bool composite = true;
                for (int j = 0; j < r - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == n - 1) { composite = false; break; }
                }

                if (composite) return false;
            }

            return true;
        }

        private BigInteger[] GetPrimeFactors(BigInteger n)
        {
            var factors = new System.Collections.Generic.List<BigInteger>();
            BigInteger temp = n;

            for (BigInteger i = 2; i * i <= temp; i++)
            {
                if (temp % i == 0)
                {
                    factors.Add(i);
                    while (temp % i == 0)
                        temp /= i;
                }
            }

            if (temp > 1) factors.Add(temp);
            return factors.ToArray();
        }

        private BigInteger GenerateRandomOdd(int bitLength)
        {
            byte[] bytes = new byte[bitLength / 8 + 1];
            _random.NextBytes(bytes);
            bytes[bytes.Length - 1] = 0;
            bytes[bitLength / 8 - 1] |= 0x80;
            bytes[0] |= 0x01;
            return BigInteger.Abs(new BigInteger(bytes));
        }

        private BigInteger GenerateRandomInRange(BigInteger min, BigInteger max)
        {
            BigInteger range = max - min;
            byte[] bytes = range.ToByteArray();
            BigInteger result;
            do
            {
                _random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                result = new BigInteger(bytes);
            } while (result > range);
            return min + result;
        }
    }
}
