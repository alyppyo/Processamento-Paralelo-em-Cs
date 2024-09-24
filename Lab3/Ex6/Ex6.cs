using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static readonly ConcurrentQueue<int> equipamentos = [];
    static AutoResetEvent equipamentoDisponivel = new(false);
    static int tempoDeEsperaMs = 1000;

    static void Main()
    {
        // Obter número de equipamentos (E) e de pesquisadores (P)
        string[] entrada = Console.ReadLine().Split();
        int E = int.Parse(entrada[0]);
        int P = int.Parse(entrada[1]);

        for(int i = 0; i < E; ++i)
        {
            equipamentos.Enqueue(i+1);
        }
        
        Task[] tasks = new Task[P];
        for(int i = 0; i < P; ++i)
        {
            int id = i + 1;
            tasks[i] = Task.Run(() => UsarEquipamento(id));
        }

        Task.WaitAll(tasks);

        int tempoSec = 30;
        Console.WriteLine($"\n\nPrograma finalizado. O console irá fechar em {tempoSec} segundos.");
        Thread.Sleep(tempoSec * 1000);
    }

    private static void UsarEquipamento(int idPesquisador)
    {
        int idEquipamento;

        while(true)
        {
            if(equipamentos.TryDequeue(out idEquipamento))
            {
                Console.WriteLine($"Pesquisador {idPesquisador} começou a usar 'Equipamento {idEquipamento}'");
                break;
            }
            else
            {
                Console.WriteLine($"Pesquisador {idPesquisador} não conseguiu acesso ao equipamento. Tentando novamente...");
                equipamentoDisponivel.WaitOne();
            }
        }
        
        Thread.Sleep(new Random().Next(500, 10000));
        Console.WriteLine($"Pesquisador {idPesquisador} terminou de usar 'Equipamento {idEquipamento}'");
        equipamentos.Enqueue(idEquipamento);
        equipamentoDisponivel.Set();
    }
}
