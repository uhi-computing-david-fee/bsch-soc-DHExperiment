using System;
using System.Numerics;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Implements the Diffie-Hellman key exchange protocol.
    ///
    /// Protocol summary:
    ///   1. Both parties agree on public parameters: prime p and generator g
    ///   2. Alice chooses private key a, computes public key A = g^a mod p
    ///   3. Bob chooses private key b, computes public key B = g^b mod p
    ///   4. Alice computes shared secret: B^a mod p
    ///   5. Bob computes shared secret:  A^b mod p
    ///   Both arrive at the same value: g^(ab) mod p
    /// </summary>
    public class DiffieHellman
    {
        public BigInteger P { get; private set; }
        public BigInteger G { get; private set; }

        private readonly Random _random;

        public DiffieHellman(BigInteger p, BigInteger g, int seed = 42)
        {
            P = p;
            G = g;
            _random = new Random(seed);
        }

        public BigInteger GeneratePrivateKey()
        {
            byte[] bytes = (P - 2).ToByteArray();
            BigInteger privateKey;

            do
            {
                _random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                privateKey = new BigInteger(bytes);
            } while (privateKey < 2 || privateKey >= P - 1);

            return privateKey;
        }

        public BigInteger ComputePublicKey(BigInteger privateKey)
        {
            return BigInteger.ModPow(G, privateKey, P);
        }

        public BigInteger ComputeSharedSecret(BigInteger receivedPublicKey, BigInteger privateKey)
        {
            return BigInteger.ModPow(receivedPublicKey, privateKey, P);
        }

        public bool PerformExchange(out BigInteger sharedSecret)
        {
            BigInteger alicePrivate = GeneratePrivateKey();
            BigInteger bobPrivate = GeneratePrivateKey();

            BigInteger alicePublic = ComputePublicKey(alicePrivate);
            BigInteger bobPublic = ComputePublicKey(bobPrivate);

            BigInteger aliceShared = ComputeSharedSecret(bobPublic, alicePrivate);
            BigInteger bobShared = ComputeSharedSecret(alicePublic, bobPrivate);

            sharedSecret = aliceShared;
            return aliceShared == bobShared;
        }
    }
}