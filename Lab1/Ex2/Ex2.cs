/*
 * Crie um programa que recebe um inteiro N como entrada e gera uma sequência
 * (array) de N números inteiros aleatórios. O programa deve somar todos os elementos da
 * sequência utilizando M threads, onde cada thread é responsável por somar uma parte do
 * array. No final, o programa deve combinar os resultados obtidos por cada thread para
 * calcular a soma total de todos os elementos da sequência. Meça o tempo de execução para
 * diferentes valores de M (por exemplo, 2, 4, 8, 16 e 32 threads) e calcule a eficiência
 * (speedup em relação a uma thread). O que você observa?
 *
 * R: Mesmo aumentando o número de threads não foi possível observar uma melhoria no tempo
 *    de soma. Pelo contrário, com o aumento das threads, o tempo de soma também aumentou.
 *    Isso se torna mais evidente quando comparamos à soma simples realizadas sem threads.
 *    Com isso podemos concluir que o overhead gerado pela abordagem foi prejudicial ao
 *    procedimento, sendo necessárias outras técnicas para se adquirir a aceleração na
 *    realização da operação desejada. 
 */

using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static long _parallelSum;
    static readonly object _locker = new();

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

        // Iniciar a contagem do tempo de execução
        Stopwatch timer = new Stopwatch();
        timer.Start();

        long sum = 0;
        for (int i = 0; i < N; i++)
        {
            sum += sequence[i];
        }
        timer.Stop();
        Console.WriteLine($"Tempo de execução com laço simples: {timer.ElapsedMilliseconds} ms | Soma = {sum}");
        Console.WriteLine("----------------------------------------------------");

        // Continue a Implementação
        for(int M = 2; M < 65; M *= 2) // Número de threads
        {
            int end = 0;
            int slotSize = N/M;
            int numberOfBiggerSlots = N%M;
            _parallelSum = 0;
            Thread[] threads = new Thread[M];
         
            timer.Start();

            for (int i = 0; i < M; ++i)
            {
                int currentSlotSize = (i < numberOfBiggerSlots) ? slotSize+1 : slotSize;

                int begin = end;
                end = begin+currentSlotSize;
                int threadEnd = end;
                
                threads[i] = new Thread(() => SumVector(sequence, begin, threadEnd));
                threads[i].Start();
            }

            for(int i = 0; i < M; ++i)
            {
                threads[i].Join();
            }
                    
            // Finalizar a contagem do tempo de execução
            timer.Stop();
            Console.WriteLine($"Tempo de execução com {M,2} threads: {timer.ElapsedMilliseconds,2} ms | Soma paralela: {_parallelSum}");
        }
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
    }
}
