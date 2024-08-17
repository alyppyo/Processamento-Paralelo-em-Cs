using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static long _parallelSum = 0;
    static readonly object _locker = new();
    static CountdownEvent _countdown;

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

        int M = 2; // Número de Threads

        // Iniciar a contagem do tempo de execução
        Stopwatch timer = new Stopwatch();
        timer.Start();

        long sum = 0;
        for (int i = 0; i < N; i++)
        {
            sum += sequence[i];
        }
        timer.Stop();
        Console.WriteLine("Tempo de execução com laço simples: " + timer.ElapsedMilliseconds + " ms");

        timer.Start();

        // Continue a Implementação
        int slotSize = N/M;
        int numberOfBiggerSlots = N%M;
        int end = 0;

        _countdown = new(M);

        for (int i = 0; i < M; i++)
        {
            int currentSlotSize = (i < numberOfBiggerSlots) ? slotSize+1 : slotSize;

            int begin = end;
            end = begin+currentSlotSize;
            int threadEnd = end;
            
            new Thread(() => SumVector(sequence, begin, threadEnd)).Start();
        }

        _countdown.Wait();
                
        // Finalizar a contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
        Console.WriteLine($"Sum: {sum} - Parallel Sum: {_parallelSum}");
    }

    static void SumVector(int[] sequence, int begin, int end)
    {
        long sum = 0;
        for(int i = begin; i < end; ++i)
        {
            sum += sequence[i];
        }

        lock(_locker)
        {
            _parallelSum += sum;
        }

        _countdown.Signal();
    }
}

