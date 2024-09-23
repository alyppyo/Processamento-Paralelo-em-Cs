using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static double TemperaturaAtual = 0;
    static ReaderWriterLockSlim _lock = new();

    static async Task Main(string[] args)
    {
        // Obter a quantidade de usuários e atualizações do sensor
        string[] entrada = Console.ReadLine().Split();
        int usuarios = int.Parse(entrada[0]);
        int atualizacoes = int.Parse(entrada[1]);

        // Obter a quantidade de leituras realizadas por cada usuário
        int[] leituras = new int[usuarios];
        for(int i = 0; i < usuarios; i++) {
            leituras[i] = int.Parse(Console.ReadLine());
        }

        ConcurrentBag<Task> taskLista = [AtualizarTemperatura(atualizacoes)];
        
        Parallel.For(0, usuarios, (usuarioId) =>
        {
            Parallel.For(0, leituras[usuarioId], (i) =>
            {
                taskLista.Add(ConsultarTemperaturaAsync(usuarioId));
            });
        });

        await Task.WhenAll(taskLista);

        Thread.Sleep(10000);
    }

    static async Task ConsultarTemperaturaAsync(int usuarioId)
    {
        Random rand = new(Guid.NewGuid().GetHashCode());
        await Task.Delay(rand.Next(100, 3000));

        _lock.EnterReadLock();
        try
        {
            Console.WriteLine($"Usuário {usuarioId}: Temperatura lida: {TemperaturaAtual:F2}ºC");
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    static async Task AtualizarTemperatura(int vezes)
    {
        for (int i = 0; i < vezes; ++i)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            _lock.EnterWriteLock();
            try
            {
                TemperaturaAtual = rand.NextDouble() * 100;
                Console.WriteLine($"[Sensor] Temperatura atualizada: {TemperaturaAtual:F2}°C");
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            await Task.Delay(rand.Next(50,1000));
        }
    }
}
