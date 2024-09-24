using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex5
{
    internal class GerenciadorDeEstacionamento
    {
        private int _capacidade;
        private int _vagasDisponiveis;
        private AutoResetEvent _vagaLiberada = new(false);
        private readonly object _locker = new object();

        public GerenciadorDeEstacionamento(int capacidade)
        {
            _capacidade = capacidade;
            _vagasDisponiveis = capacidade;
        }

        public void EntrarComVeiculo(int idVeiculo)
        {
            Console.WriteLine($"Veículo {idVeiculo} esperando para entrar...");

            while (true)
            {
                lock (_locker)
                {
                    if(_vagasDisponiveis > 0)
                    {
                        Console.WriteLine($"Evento: Veículo entrou. Vagas disponíveis: {--_vagasDisponiveis}");
                        Console.WriteLine($"Veículo {idVeiculo} estacionou.");
                        break;
                    }
                }

                _vagaLiberada.WaitOne();
            }
            
            Thread.Sleep(new Random().Next(500, 5000));
            SairComVeiculo(idVeiculo);
        }

        private void SairComVeiculo(int idVeiculo)
        {
            lock (_locker)
            {
                Console.WriteLine($"Evento: Veículo saiu. Vagas disponíveis: {++_vagasDisponiveis}");
                Console.WriteLine($"Veículo {idVeiculo} saiu.");
                _vagaLiberada.Set();
            }
        }
    }
}
