// Exercício 1: Soma de Quadrados (1 ponto)
//
// Objetivo: Exercitar paralelismo de dados e compartilhamento de recursos usando TPL.
//
// Descrição: Dado um número inteiro N lido da entrada, seu programa deve calcular a soma
// dos quadrados dos números inteiros no intervalo de 0 a N. Por fim, imprima o resultado.
// Implemente uma versão sequencial e uma versão usando paralelismo. Qual é o speedup
// que a sua implementação paralela obteve em relação a sequencial?
//
// R: A partir de 64 divisões no Parallel.For, os resultados começaram a ser melhores que
//    a solução sequencial. Abaixo, sequência de resultados para N = 1.000.000.000.
//           |                Rodadas            
//  Divisões |   1       2       3       4       5 
// --------------------------------------------------
//        32 | 0,77x   0,77x   0,77x   0,77x   0,75x
//        64 | 1,50x   1,53x   1,73x   1,28x   1,73x
//       128 | 2,56x   3,03x   2,89x   3,00x   3,09x
//       256 | 5,35x   4,90x   5,36x   4,80x   5,24x

using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(":: Somas dos Quadrados (Sequencial e Paralela) ::");
        Console.Write("- Informe o valor final do intervalo: ");
        int N = int.Parse(Console.ReadLine());

        Stopwatch cronometro = new();
        double somaDosQuadrados;

        // Implementação sequencial
        cronometro.Start();
        somaDosQuadrados = SomaSequencial(N);
        cronometro.Stop();

        long tempoSequencial = cronometro.ElapsedMilliseconds;
        Console.WriteLine($"Soma sequencial: {somaDosQuadrados:e5} | Tempo: {tempoSequencial} ms");

        // Implementação paralela
        for (int M = 2; M < 257; M *= 2)
        {
            cronometro.Restart();
            somaDosQuadrados = SomaParalela(N, M);
            cronometro.Stop();

            long tempoParalelo = cronometro.ElapsedMilliseconds;
            Console.WriteLine($"Soma paralela ({M,3} divisões): {somaDosQuadrados:e5} | Tempo: {tempoParalelo} ms | Speedup: {1 / ((double)tempoParalelo / tempoSequencial):f2}x");
        }
    }

    static double SomaSequencial(int N)
    {
        double somaDosQuadrados = 0;

        for (int i = 0; i < N; ++i)
        {
            somaDosQuadrados += (double)i * i;
        }

        return somaDosQuadrados;
    }

    static double SomaParalela(int N, int M)
    {
        double somaDosQuadrados = 0;
        double[] somas = new double[M];
        int pedaco = N / M;

        Parallel.For(0, M, i =>
        {
            int end = i < M - 1 ? (i + 1) * pedaco : N;
            for (int j = i * pedaco; j < end; ++j)
            {
                somas[i] += (double)j * j;
            }
        });

        for (int i = 0; i < M; ++i)
        {
            somaDosQuadrados += somas[i];
        }

        return somaDosQuadrados;
    }
}