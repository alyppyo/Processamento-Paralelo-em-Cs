using System;
using System.Collections.Concurrent;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim semaforo;

    static async Task Main(string[] args)
    {
        // Obter o número de pedidos (P) e de cozinheiros (C)
        string[] entrada = Console.ReadLine().Split();
        int P = int.Parse(entrada[0]);
        int C = int.Parse(entrada[1]);

        var pedidos = new List<(string prato, int tempo)>();

        for (int i = 0; i < P; i++)
        {
            string[] pedido = Console.ReadLine().Split(',');
            string prato = pedido[0].Trim();
            int tempo = int.Parse(pedido[1].Trim());
            pedidos.Add((prato, tempo));
        }

        ConcurrentBag<Task> tasksLista = new();
        semaforo = new(C);

        Parallel.ForEach(pedidos, async (pedido) =>
        {
            tasksLista.Add(PrepararPedido(pedido.prato, pedido.tempo));
        });

        await Task.WhenAll(tasksLista);

        int tempoSec = 30;
        Console.WriteLine($"\n\nPrograma finalizado. O console irá fechar em {tempoSec} segundos.");
        Thread.Sleep(tempoSec * 1000);
    }

    static async Task PrepararPedido(string prato, int tempoMs)
    {
        await semaforo.WaitAsync();

        Console.WriteLine($"O prato '{prato}' começou a ser preparado.");
        await Task.Delay(tempoMs);
        Console.WriteLine($"O prato '{prato}' está pronto.");

        semaforo.Release();
    }
}
