using System.Numerics;


namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Holds the result of a single experimental run.
    /// </summary>
    public class ExperimentResult
    {
        public int BitLength { get; }
        public int RunNumber { get; }
        public double PrimeGenerationMs { get; }
        public double ExchangeMs { get; }
        public bool AttackSucceeded { get; }
        public BigInteger AttackIterations { get; }
        public double AttackMs { get; }

        public ExperimentResult(
            int bitLength,
            int runNumber,
            double primeGenerationMs,
            double exchangeMs,
            bool attackSucceeded,
            BigInteger attackIterations,
            double attackMs)
        {
            BitLength = bitLength;
            RunNumber = runNumber;
            PrimeGenerationMs = primeGenerationMs;
            ExchangeMs = exchangeMs;
            AttackSucceeded = attackSucceeded;
            AttackIterations = attackIterations;
            AttackMs = attackMs;
        }
    }
}
