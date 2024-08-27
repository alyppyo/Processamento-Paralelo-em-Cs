// Gere um grande vetor de números aleatórios. Divida o vetor em partes e atribua
// cada parte a um thread diferente. Cada thread deve contar a frequência de cada número em
// sua parte e armazenar os resultados em uma estrutura de dados compartilhada.
//
// 5. Tente implementar uma terceira versão usando uma forma mais otimizada de
// agregar os resultados.
// R: Ao criar uma solução na qual cada thread possui uma estrutura de contagem e,
//    sincronizam apenas ao passar suas respectivas contagens para a estrutura de
//    contagem final, conseguimos um speedup considerável com relação à primeira
//    solução, embora continue sem um speedup significativo no aumento das threads.
//

using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int[] _frequency;
    static readonly object _locker = new object();

    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(args[0]);
        int rangeStart = int.Parse(args[1]);
        int rangeEnd = int.Parse(args[2]);

        // Criar o vetor de tamanho N
        int[] vector = new int[N];

        // Inicializar o vetor com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            vector[i] = rand.Next(rangeStart, rangeEnd);
        }

        // Continue a Implementação
        for(int M = 2; M < 65; M *= 2) // Número de Threads
        {
            int slotSize = N/M;
            int numberOfBiggerSlots = N%M;
            int end = 0;

            _frequency = new int[rangeEnd-rangeStart];
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
                
                threads[i] = new Thread(() => UpdateFrequency(vector, rangeStart, rangeEnd, begin, threadEnd));
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

    static void UpdateFrequency(int[] vector, int rangeStart, int rangeEnd, int begin, int end)
    {
        int rangeSize = rangeEnd-rangeStart;
        int[] counters = new int[rangeSize];
        for(int i = begin; i < end; ++i)
        {
            ++counters[vector[i]-rangeStart];
        }

        lock(_locker)
        {
            for(int i = 0; i < rangeSize; ++i)
            {
                _frequency[i] += counters[i];                
            }
        }
    }
}