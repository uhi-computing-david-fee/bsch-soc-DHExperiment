using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Writes experiment results to a CSV file.
    /// Results are appended if the file already exists.
    /// </summary>
    public class ResultReporter
    {
        private readonly string _outputFilePath;

        public ResultReporter(string outputFilePath)
        {
            _outputFilePath = outputFilePath;
            EnsureHeaderExists();
        }

        public void Write(IEnumerable<ExperimentResult> results)
        {
            var sb = new StringBuilder();
            foreach (var r in results)
            {
                sb.AppendLine(
                    $"{r.BitLength}," +
                    $"{r.RunNumber}," +
                    $"{r.PrimeGenerationMs:F4}," +
                    $"{r.ExchangeMs:F4}," +
                    $"{r.AttackSucceeded}," +
                    $"{r.AttackIterations}," +
                    $"{r.AttackMs:F4}");
            }
            File.AppendAllText(_outputFilePath, sb.ToString());
        }

        private void EnsureHeaderExists()
        {
            if (!File.Exists(_outputFilePath))
                File.WriteAllText(_outputFilePath,
                    "BitLength,RunNumber,PrimeGenerationMs,ExchangeMs," +
                    "AttackSucceeded,AttackIterations,AttackMs\n");
        }
    }
}
