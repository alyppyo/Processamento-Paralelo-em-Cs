// Gere um grande vetor de números aleatórios. Divida o vetor em partes e atribua
// cada parte a um thread diferente. Cada thread deve contar a frequência de cada número em
// sua parte e armazenar os resultados em uma estrutura de dados compartilhada.
//
// 2. Faça uma segunda versão usando contadores privados.

using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int[,] _frequency;
    static readonly object _locker = new object();

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
        for(int M = 2; M < 65; M *= 2) // Número de Threads
        {
            int slotSize = N/M;
            int numberOfBiggerSlots = N%M;
            int end = 0;

            _frequency = new int[M,N];
            Thread[] threads = new Thread [M];

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
                
                threads[i] = new Thread(() => UpdateFrequency(threadId, vector, begin, threadEnd));
                threads[i].Start();
            }

            for (int i = 0; i < M; ++i)
            {
                threads[i].Join();
            }

            timer.Stop();
            Console.WriteLine($"Tempo de execução com {M,2} threads: {timer.ElapsedMilliseconds,3} ms");
        }
    }

    static void UpdateFrequency(int threadId, int[] vector, int begin, int end)
    {
        for(int i = begin; i < end; ++i)
        {
            ++_frequency[threadId, vector[i]];
        }
    }
}