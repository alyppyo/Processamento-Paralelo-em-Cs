// Gere um grande vetor de números aleatórios. Divida o vetor em partes e atribua
// cada parte a um thread diferente. Cada thread deve contar a frequência de cada número em
// sua parte e armazenar os resultados em uma estrutura de dados compartilhada.
//
// 1. Faça uma primeira versão onde todos as threads atualizam a estrutura
// compartilhada.
//
// 3. Coloca o número de thread como parâmetro do programa e calcula o speedup das
// duas versões usando vários números de threads (por exemplo 2/4/8/10/12/16). O
// que você observa?
// R: Mesmo aumentando o número de threads, o overhead continua sendo um obstáculo
//    para a otimização da soma. Em algumas rodadas até se consegue algum speedup
//    para 2, 4, ou 8 threads, mas eles são intermitentes.
// 
// 4. Aumenta a quantidade de números aleatórios e o range dos números, o que você
// observa?
// R: O aumento dos números aleatórios aumentou também o tempo total de processamento,
//    o que já era esperado, dado que serão mais números a serem processados. A proporção
//    de tempo entre as threads se manteve. A alteração do range dos números não teve 
//    impacto significativo no tempo de processamento.

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

            // Iniciar a medição de tempo
            Stopwatch timer = new Stopwatch();
            Thread[] threads = new Thread [M];
            timer.Start();    

            for (int i = 0; i < M; i++)
            {
                int currentSlotSize = (i < numberOfBiggerSlots) ? slotSize+1 : slotSize;

                int begin = end;
                end = begin+currentSlotSize;
                int threadEnd = end;
                
                threads[i] = new Thread(() => UpdateFrequency(vector, rangeStart, rangeEnd, begin, threadEnd));
                threads[i].Start();
            }

            for (int i = 0; i < M; i++)
            {
                threads[i].Join();
            }

            timer.Stop();

            Console.WriteLine($"Tempo de execução com {M,2} threads: {timer.ElapsedMilliseconds} ms");
            // Console.WriteLine(">> Frequencies:");
            // for(int i = rangeStart; i < rangeEnd; ++i)
            // {
            //     Console.WriteLine($"{i} = {_frequency[i-rangeStart]}");
            // }

            // Console.WriteLine();
        }
    }

    static void UpdateFrequency(int[] vector, int rangeStart, int rangeEnd, int begin, int end)
    {
        for(int i = begin; i < end; ++i)
        {
            lock(_locker)
            {
                ++_frequency[vector[i]-rangeStart];
            }
        }
    }
}