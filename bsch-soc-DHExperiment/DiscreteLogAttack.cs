using System;
using System.Diagnostics;
using System.Numerics;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Brute force discrete logarithm attack against Diffie-Hellman.
    ///
    /// Given public parameters p and g, and a target public key A = g^a mod p,
    /// this attack attempts to recover the private key a by trying every
    /// possible value until g^x mod p equals A.
    ///
    /// For small primes this completes quickly. For large primes it becomes
    /// completely infeasible. Observing this transition is the core of the
    /// exercise.
    /// </summary>
    public class DiscreteLogAttack
    {
        private readonly BigInteger _p;
        private readonly BigInteger _g;

        public static readonly BigInteger MaxIterations = 10_000_000;

        public DiscreteLogAttack(BigInteger p, BigInteger g)
        {
            _p = p;
            _g = g;
        }

        /// <summary>
        /// Attempts to recover the private key from a public key.
        ///
        /// Returns the recovered private key, or -1 if not found within
        /// MaxIterations.
        ///
        /// Implementation guidance:
        ///   Find x such that g^x mod p equals publicKey.
        ///   Use an iterative approach: maintain a running value and update
        ///   it with a single multiplication and modulo per step rather than
        ///   recomputing g^x mod p from scratch each iteration.
        ///   See the brute force attack tutorial for a full explanation.
        /// </summary>
        public BigInteger Attack(BigInteger publicKey, out BigInteger iterations, out double elapsedMs)
        {
            iterations = 0;
            elapsedMs = 0;

            // TODO: Implement the brute force discrete logarithm attack.
            throw new NotImplementedException("Implement the attack here.");
        }
    }
}
