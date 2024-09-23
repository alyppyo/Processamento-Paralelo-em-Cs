using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static int IdSolicitacao = 1;
    static readonly object locker = new object();

    static void Main(string[] args)
    {
        // Leitura do número total de pedidos (N), vendedores (V) e entregadores (E)
        string[] entrada = Console.ReadLine().Split();
        int N = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);
        int E = int.Parse(entrada[2]);

        int solicitacoesPorVendedor = N / V;
        int vendedoresComSolicitacaoExtra = N % V;

        ConcurrentQueue<int> filaDeSolicitacoes = new();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        Task[] solicitacoes = new Task[V];
        Task[] entregas = new Task[E];

        Parallel.Invoke(
            () =>
            {
                for (int i = 0; i < V; ++i)
                {
                    int id = i + 1;
                    int numeroDeSolicitacoes = solicitacoesPorVendedor;
                    if(i < vendedoresComSolicitacaoExtra)
                    {
                        ++numeroDeSolicitacoes;
                    }
                    solicitacoes[i] = Task.Run(() => CriarSolicitacao(id, filaDeSolicitacoes, numeroDeSolicitacoes));
                }
            },
            () =>
            {
                for (int i = 0; i < E; ++i)
                {
                    int id = i + 1;
                    entregas[i] = Task.Run(() => EntregarPedido(id, filaDeSolicitacoes, cancellationTokenSource.Token));
                }
            }
        );      

        Task.WaitAll(solicitacoes);
        cancellationTokenSource.Cancel();
        Task.WaitAll(entregas);

        Thread.Sleep(30000);
    }

    static void CriarSolicitacao(int idVendedor, ConcurrentQueue<int> filaDeSolicitacoes, int solicitacoesPorVendedor)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());

        for (int i = 0; i < solicitacoesPorVendedor; ++i)
        {
            int id;
            lock(locker)
            {
                id = IdSolicitacao++;
            }

            Console.WriteLine($"Vendedor {idVendedor}: Pedido #{id:D3} criado.");
            filaDeSolicitacoes.Enqueue(id);

            Task.Delay(random.Next(100, 2000)).Wait();
        }
    }

    static void EntregarPedido(int idEntregador, ConcurrentQueue<int> filaDeSolicitacoes, CancellationToken token)
    {
        while(!token.IsCancellationRequested || !filaDeSolicitacoes.IsEmpty)
        {
            if(filaDeSolicitacoes.TryDequeue(out int id))
            {
                Console.WriteLine($"Entregador {idEntregador}: Pedido #{id:D3} entregue.");
            }
        }
    }
}
