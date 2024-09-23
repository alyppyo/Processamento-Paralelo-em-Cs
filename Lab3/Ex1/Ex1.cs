using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly object lockObject = new();
    private static int totalVotosA = 0;
    private static int totalVotosB = 0;

    static async Task Main(string[] args)
    {
        // Leitura do número de regiões
        int R = int.Parse(Console.ReadLine());

        var votos = new List<(int votosChapaA, int votosChapaB)>();

        // Leitura dos votos em cada região
        for (int i = 0; i < R; i++)
        {
            string[] entrada = Console.ReadLine().Split();
            int votosRegiaoA = int.Parse(entrada[0]);
            int votosRegiaoB = int.Parse(entrada[1]);
            votos.Add((votosRegiaoA, votosRegiaoB));
        }

        var taskLista = votos.Select(async votosRegiao =>
        {
            await ContarVotos(votosRegiao.votosChapaA, votosRegiao.votosChapaB);
        });
        
        await Task.WhenAll(taskLista);

        DivulgarResultados();

        Thread.Sleep(10000);
    }

    static async Task ContarVotos(int votosRegiaoA, int votosRegiaoB)
    {
        Random random = new();
        await Task.Delay(random.Next(1, 2000));

        lock(lockObject)
        {
            totalVotosA += votosRegiaoA;
            totalVotosB += votosRegiaoB;
        }
    }

    static void DivulgarResultados()
    {
        Console.WriteLine($"Chapa A: {totalVotosA} votos");
        Console.WriteLine($"Chapa B: {totalVotosB} votos");

        if(totalVotosA > totalVotosB)
        {
            Console.WriteLine("A chapa A venceu a eleição!");
        }
        else if (totalVotosB > totalVotosA)
        {
            Console.WriteLine("A chapa B venceu a eleição!");
        }
        else
        {
            Console.WriteLine("Empate entre as duas chapas!");
        }
    }
}
