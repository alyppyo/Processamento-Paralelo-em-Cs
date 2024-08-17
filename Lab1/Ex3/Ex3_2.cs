using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int[,] _frequency;
    static readonly object _locker = new object();

    static CountdownEvent _countdown;

    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(Console.ReadLine());

        // Criar o vetor de tamanho N
        int[] vector = new int[N];

        // Inicializar o vetor com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            vector[i] = rand.Next(1, N);
        }

        // Continue a Implementação
        int M = 8; // Número de Threads
        int slotSize = N/M;
        int numberOfBiggerSlots = N%M;
        int end = 0;

        _frequency = new int[M,N];
        _countdown = new (M);

        // Iniciar a medição de tempo
        Stopwatch timer = new Stopwatch();
        timer.Start();    

        for (int i = 0; i < M; ++i)
        {
            int currentSlotSize = (i < numberOfBiggerSlots) ? slotSize+1 : slotSize;

            int begin = end;
            end = begin+currentSlotSize;
            int threadEnd = end;
            int threadId = i;
            
            new Thread(() => UpdateFrequency(threadId, vector, begin, threadEnd)).Start();
        }

        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
    }

    static void UpdateFrequency(int threadId, int[] vector, int begin, int end)
    {
        for(int i = begin; i < end; ++i)
        {
            ++_frequency[threadId, vector[i]];
        }
    }
}
