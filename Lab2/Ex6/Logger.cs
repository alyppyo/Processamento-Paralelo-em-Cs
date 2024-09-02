using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex6
{
    internal class Logger
    {
        private static readonly Logger Instance = new Logger();
        private static string logFile = "log.txt";

        public static async Task LogError(SaldoInsuficienteException ex)
        {
            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                await writer.WriteLineAsync($"{DateTime.Now}: {ex.Message}");
            }
        }
    }
}
