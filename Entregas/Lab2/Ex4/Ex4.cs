// Exercício 4: Encontrar Números Primos (2.5 pontos)
//
// Objetivo: Utilizar diferentes recursos da Task Parallel Library (TPL) do C#.
//
// Descrição: Elabore uma aplicação que receba um número inteiro N, seguido por N
// números inteiros. Para cada número k informado na entrada, a aplicação deve imprimir
// todos os números primos entre 1 e k.
//
// A. Faça uma versão que utilize Tasks.
// B. Faça uma segunda versão que utiliza Parallel.For.
// C. Faça uma terceira versão que utiliza Parallel.ForEach.
//
// D. Compare o tempo de execução das três implementações.
// R: As versões com Parallel.ForEach e Tasks foram as que tiveram um melhor resultado,
//    embora não tenham conseguido superar o cálculo sequencial. O Parallel.For teve o
//    pior desempenho, levando de 2 a 3 vezes mais tempo que as demais alternativas para
//    realizar o cálculo e divulgar os resultados.
//
//                  |                Rodadas            
//       Método     |   1       2       3       4       5 
// ---------------------------------------------------------
// Sequencial       |  9ms     9ms     9ms    10ms    10ms
// Task             | 14ms    15ms    15ms    16ms    16ms
// Parallel.For     | 30ms    28ms    30ms    25ms    31ms
// Parallel.ForEach | 13ms    14ms    12ms    19ms    12ms

using System.Collections.Concurrent;
using System.Diagnostics;

class Program {
    static async Task Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        string[] entrada = Console.ReadLine().Split(' ');
        int[] sequencia = new int[N];

        for(int i = 0; i < N; i++) {
            sequencia[i] = int.Parse(entrada[i]);
        }

        // Continue a Implementação
        Stopwatch cronometro = new();
        
        cronometro.Start();
        ImprimirPrimos(sequencia);
        cronometro.Stop();
        long tempoSolucaoPadrao = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        await ImprimirPrimosA(sequencia);
        cronometro.Stop();
        long tempoSolucaoA = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        ImprimirPrimosB(sequencia);
        cronometro.Stop();
        long tempoSolucaoB = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        ImprimirPrimosC(sequencia);
        cronometro.Stop();
        long tempoSolucaoC = cronometro.ElapsedMilliseconds;

        Console.WriteLine($"\n\n> Solução Padrão: {tempoSolucaoPadrao}ms");
        Console.WriteLine($"> Solução A: {tempoSolucaoA}ms");
        Console.WriteLine($"> Solução B: {tempoSolucaoB}ms");
        Console.WriteLine($"> Solução C: {tempoSolucaoC}ms");

        Thread.Sleep(20000);
    }

    static void ImprimirPrimos(int[] sequencia)
    {
        foreach (int valor in sequencia)
        {
            Console.Write($"-P- Primos até {valor}: ");
            
            if (valor < 2)
            {
                Console.WriteLine("Não existem primos");
                continue;
            }

            Console.Write($"2 ");

            int limite = valor + 1;
            for (int j = 3; j < limite; j += 2)
            {
                int k;
                for(k = j-1; k > 2; --k)
                {
                    if (j % k == 0)
                        break;
                }

                if(k <= 2)
                {
                    Console.Write(j + " ");
                }
            }

            Console.WriteLine();
        }
    }

    static async Task ImprimirPrimosA(int[] sequencia)
    {
        ConcurrentDictionary<int, bool> numerosPrimos = new();
        object locker = new();

        foreach (int valor in sequencia)
        {
            Task<int> task = Task.Run(() =>
            {
                int limite = valor + 1;
                for (int j = 3; j < limite; j += 2)
                {
                    if (!numerosPrimos.ContainsKey(j))
                    {
                        int k;
                        for (k = j - 1; k > 2; --k)
                        {
                            if (j % k == 0)
                                break;
                        }

                        numerosPrimos.TryAdd(j, k <= 2);
                    }
                }

                return valor;
            });

            await task.ContinueWith(task =>
            {
                lock(locker)
                {
                    Console.Write($"-A- Primos até {valor}: ");

                    if (valor < 2)
                    {
                        Console.WriteLine("Não existem primos");
                        return;
                    }

                    Console.Write($"2 ");

                    List<int> listaPrimos = numerosPrimos.Where(entrada => entrada.Key <= task.Result && entrada.Value).
                        Select(entrada => entrada.Key).ToList();
                    listaPrimos.Sort();

                    listaPrimos.ForEach(i => Console.Write(i + " "));

                    Console.WriteLine();
                }
            });
        }
    }

    static void ImprimirPrimosB(int[] sequencia)
    {
        ConcurrentDictionary<int, bool> primeNumbers = new();
        object locker = new();

        Parallel.For(0, sequencia.Length, index =>
        {
            int limite = sequencia[index] + 1;
            for (int j = 3; j < limite; j += 2)
            {
                if (!primeNumbers.ContainsKey(j))
                {
                    int k;
                    for (k = j-1; k > 2; --k)
                    {
                        if (j % k == 0)
                            break;
                    }

                    primeNumbers.TryAdd(j, k <= 2);
                }
            }

            lock (locker)
            {
                Console.Write($"-B- Primos até {sequencia[index]}: ");

                if (sequencia[index] < 2)
                {
                    Console.WriteLine("Não existem primos");
                    return;
                }

                Console.Write($"2 ");

                List<int> primesList = primeNumbers.Where(entrada => entrada.Key <= sequencia[index] && entrada.Value).
                    Select(entrada => entrada.Key).ToList();
                primesList.Sort();

                primesList.ForEach(i => Console.Write(i + " "));

                Console.WriteLine();
            }
        });
    }

    static void ImprimirPrimosC(int[] sequencia)
    {
        ConcurrentDictionary<int, bool> primeNumbers = new();
        object locker = new();

        Parallel.ForEach(sequencia, item =>
        {
            int limite = item + 1;
            for (int j = 3; j < limite; j += 2)
            {
                if (!primeNumbers.ContainsKey(j))
                {
                    int k;
                    for (k = j - 1; k > 2; --k)
                    {
                        if (j % k == 0)
                            break;
                    }

                    primeNumbers.TryAdd(j, k <= 2);
                }
            }

            lock (locker)
            {
                Console.Write($"-C- Primos até {item}: ");

                if (item < 2)
                {
                    Console.WriteLine("Não existem primos");
                    return;
                }

                Console.Write($"2 ");

                List<int> primesList = primeNumbers.Where(entrada => entrada.Key <= item && entrada.Value).
                    Select(entrada => entrada.Key).ToList();
                primesList.Sort();

                primesList.ForEach(i => Console.Write(i + " "));

                Console.WriteLine();
            }
        });
    }
}