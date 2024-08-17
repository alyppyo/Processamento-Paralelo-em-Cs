using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static CountdownEvent _countdown;
    static Barrier _barrier;

    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(Console.ReadLine());

        int[] sequence = new int[N];

        // Inicializar o array com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.Next(1, 100);
        }

        int M = 32; // Número de Threads
        _barrier = new Barrier(M);

        // Iniciar a contagem do tempo de execução
        Stopwatch timer = new Stopwatch();
        timer.Start();

        int sum = 0;
        for (int i = 0; i < N; i++)
        {
            sum += sequence[i];
        }
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");

        timer.Start();

        // Continue a Implementação
        _countdown = new(M);

        for (int i = 0; i < M; i++)
        {
            int id = i;
            new Thread(() => SumVector(sequence, M, id)).Start();
        }

        _countdown.Wait();
        Console.WriteLine($"Sum: {sum} - Parallel Sum: {sequence[0]}");
                
        // Finalizar a contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
    }

    static void SumVector(int[] sequence, int threadsNumber, int threadId)
    {
        int levelOffset = 1;
        int chunkSize = 2;
        int chunkOffset = threadsNumber*2;
        int nextPos = 0;

        while(nextPos <= sequence.Length)
        {
            for(int i = 0; i < sequence.Length; i += chunkOffset*levelOffset)
            {
                int pos = i + (threadId*chunkSize);
                nextPos = pos+levelOffset;

                if(nextPos < sequence.Length)
                {
                    sequence[pos] += sequence[nextPos];    
                }
            }

            levelOffset *= 2;
            chunkSize *= 2;
            nextPos = (threadId*chunkSize) + levelOffset;

            _barrier.SignalAndWait();
        }

        _barrier.RemoveParticipants(1);
        _countdown.Signal();
    }
}

