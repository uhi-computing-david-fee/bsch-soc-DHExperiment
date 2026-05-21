# Diffie-Hellman Experiment (C#)

A console application for investigating Diffie-Hellman key exchange and the discrete logarithm problem empirically. Built for the Science of Computing & Digital Media (UI110010) UHI BSc Hons, core exercise four.

## Purpose

This application generates Diffie-Hellman key exchanges across a range of prime bit lengths, runs a brute force discrete logarithm attack against each exchange, and records timing and iteration data for analysis. The prime generator and key exchange are supplied and working. Your task is to implement the brute force attack and design a series of experiments that reveal where the attack transitions from feasible to infeasible.

## Project Structure

| File | Purpose |
|------|---------|
| `Program.cs` | Entry point and menu-driven interface |
| `PrimeGenerator.cs` | Prime generation and primitive root finding |
| `DiffieHellman.cs` | Key exchange implementation |
| `DiscreteLogAttack.cs` | Brute force attack scaffold — implement here |
| `ExperimentRunner.cs` | Runs exchanges and attacks, records results |
| `ExperimentResult.cs` | Data class holding the result of a single run |
| `ResultReporter.cs` | Writes results to CSV |

## Getting Started

Requires .NET 6 or later.

```bash
dotnet run
```

The application will present a menu. Select run experiments, enter your chosen bit lengths, number of runs, and results file path.

## The Generator

For prime bit lengths of 64 and below the application computes a true primitive root for the generator g. Above 64 bits primitive root computation becomes impractically slow, so g=2 is used as a fixed generator instead. The console output labels g accordingly for each run.

This does not affect the validity of your attack investigation. If you want to explore the performance cost of primitive root computation as an experimental variable, the threshold in ExperimentRunner.cs is a single conditional you can adjust.

## Suggested Bit Lengths

| Range | Bit Lengths | Expected Behaviour |
|-------|------------|-------------------|
| Small | 8, 12, 16, 20, 24 | Attack completes quickly |
| Medium | 28, 32, 40 | Attack slows, transition zone |
| Large | 48, 64, 128 | Attack infeasible within limit |

Do not exceed 128 bits. Prime generation above this size becomes impractically slow.

## CSV Output

Results are written to CSV with the following columns:

| Column | Description |
|--------|-------------|
| `BitLength` | Prime bit length for this run |
| `RunNumber` | Run index for this bit length |
| `PrimeGenerationMs` | Time to generate prime and find generator |
| `ExchangeMs` | Time to complete the key exchange |
| `AttackSucceeded` | Whether the attack recovered the private key |
| `AttackIterations` | Number of iterations performed |
| `AttackMs` | Time taken by the attack |

## Where to Extend

Open `DiscreteLogAttack.cs`. Implement the `Attack` method following the guidance in the summary comments. The brute force attack tutorial covers the mathematical approach and the iterative efficiency improvement your implementation should use.

Verify your implementation recovers the correct private key on small primes before running experiments. The experiment runner prints a warning if the recovered key does not verify correctly.

## Notes

- Results are appended to the CSV file if it already exists, allowing multiple sessions to accumulate into a single dataset.
- The `MAX_ITERATIONS` constant in `DiscreteLogAttack.cs` controls how long the attack runs before giving up. Runs that exceed this limit return -1 and are recorded as failed; this is a meaningful experimental result, not an error.
- The experiment runner verifies recovered keys by recomputing the public key and comparing against the original. A warning is printed if verification fails.
- Prime generation time increases with bit length and is itself an interesting observation for your investigation.