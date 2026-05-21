using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Runs Diffie-Hellman exchange and discrete logarithm attack experiments
    /// across a range of prime bit lengths.
    /// </summary>
    public class ExperimentRunner
    {
        private readonly PrimeGenerator _primeGenerator;
        private readonly int _runsPerBitLength;

        public ExperimentRunner(int runsPerBitLength, int seed = 42)
        {
            _primeGenerator = new PrimeGenerator(seed);
            _runsPerBitLength = runsPerBitLength;
        }

        public List<ExperimentResult> RunExperiment(int bitLength)
        {
            Console.WriteLine($"\n  Bit length: {bitLength}");
            var results = new List<ExperimentResult>();

            for (int run = 1; run <= _runsPerBitLength; run++)
            {
                Console.WriteLine($"  Run {run}/{_runsPerBitLength}");

                var primeStopwatch = Stopwatch.StartNew();
                BigInteger p = _primeGenerator.GeneratePrime(bitLength);
                BigInteger g = bitLength > 64 ? 2 : _primeGenerator.FindPrimitiveRoot(p);
                Console.WriteLine($"    g = {g}{(bitLength > 64 ? " (fixed generator, see notes)" : " (primitive root)")}");
                primeStopwatch.Stop();

                Console.WriteLine($"    p = {p}");
                Console.WriteLine($"    g = {g}");
                Console.WriteLine($"    Prime generated in {primeStopwatch.Elapsed.TotalMilliseconds:F2}ms");

                var dh = new DiffieHellman(p, g);
                var exchangeStopwatch = Stopwatch.StartNew();
                bool exchangeSuccess = dh.PerformExchange(out BigInteger sharedSecret);
                exchangeStopwatch.Stop();

                Console.WriteLine($"    Exchange: {(exchangeSuccess ? "succeeded" : "failed")}, " +
                                  $"Time: {exchangeStopwatch.Elapsed.TotalMilliseconds:F4}ms");

                BigInteger alicePrivate = dh.GeneratePrivateKey();
                BigInteger alicePublic = dh.ComputePublicKey(alicePrivate);

                var attack = new DiscreteLogAttack(p, g);
                BigInteger recovered = attack.Attack(alicePublic, out BigInteger iterations, out double attackMs);
                bool attackSuccess = recovered != -1;

                if (attackSuccess && dh.ComputePublicKey(recovered) != alicePublic)
                {
                    Console.WriteLine("    WARNING: Recovered key does not verify correctly.");
                    attackSuccess = false;
                }

                Console.WriteLine($"    Attack: {(attackSuccess ? "succeeded" : "failed within limit")}, " +
                                  $"Iterations: {iterations:N0}, Time: {attackMs:F2}ms");

                results.Add(new ExperimentResult(
                    bitLength, run,
                    primeStopwatch.Elapsed.TotalMilliseconds,
                    exchangeStopwatch.Elapsed.TotalMilliseconds,
                    attackSuccess, iterations, attackMs));
            }

            return results;
        }
    }
}
