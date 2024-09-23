using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex6
{
    internal class Conta(string usuario, decimal saldo)
    {
        public string Usuario { get; private set; } = usuario;
        public decimal Saldo { get; private set; } = saldo;

        private readonly object _lock = new object();

        public async Task DepositarAsync(decimal valor)
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    Saldo += valor;
                }
            });
        }

        public async Task SacarAsync(decimal valor)
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (valor > Saldo)
                    {
                        throw new SaldoInsuficienteException($"Erro: Saldo Insuficiente na conta de {Usuario}. Saque de {valor} com saldo {Saldo}.");
                    }
                    lock (_lock)
                    {
                        Saldo -= valor;
                    }
                }
                catch (SaldoInsuficienteException ex)
                {
                    await Logger.LogError(ex);
                }
            });
        }

        public async Task TransferirAsync(Conta destino, decimal valor)
        {
            await Task.WhenAll(
                SacarAsync(valor),
                destino.DepositarAsync(valor)
            );
        }
    }
}
