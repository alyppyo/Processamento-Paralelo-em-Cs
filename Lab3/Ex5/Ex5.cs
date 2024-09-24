using Ex5;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Obter a capacidade do estacionamento (C) e o número de veículos (V)
        string[] entrada = Console.ReadLine().Split();
        int C = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);

        GerenciadorDeEstacionamento gerenciadorDeEstacionamento = new(C);
        Task[] veiculos = new Task[V]; 

        for(int i = 0; i < V; ++i)
        {
            int id = i;
            veiculos[i] = Task.Run(() => gerenciadorDeEstacionamento.EntrarComVeiculo(id));
        }

        Task.WaitAll(veiculos);

        int tempoSec = 30;
        Console.WriteLine($"\n\nPrograma finalizado. O console irá fechar em {tempoSec} segundos.");
        Thread.Sleep(tempoSec*1000);
    }
}
