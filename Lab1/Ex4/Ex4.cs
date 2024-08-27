using System;
using System.Threading;

class Program
{
    static int contadorCompartilhado = 0;
    static readonly object locker = new();

    static void Main()
    {
        int N = int.Parse(Console.ReadLine());

        Random random = new();
        int contagemTotal = 0;

        var threads = new Thread [N];
        for(int i = 0; i < N; ++i)
        {
            int numeroDeVezes = random.Next(0,10);
            contagemTotal += numeroDeVezes;

            int threadId = i;
            threads[i] = new Thread(() => IncrementarContador(threadId, numeroDeVezes));
            threads[i].Start();
            
        }

        for(int i = 0; i < N; ++i)
        {
            threads[i].Join();
        }

        Console.WriteLine($"Número de Threads = {N} | Contagem Total = {contagemTotal} | Contador Compartilhado = {contadorCompartilhado}");
    }

    static void IncrementarContador(int idThread, int numeroDeVezes)
    {
        Console.WriteLine($">> Thread [{idThread}] vai contar {numeroDeVezes} vez(es)");
        Random random = new(Guid.NewGuid().GetHashCode());

        while(numeroDeVezes-- > 0) {
            lock(locker)
            {
                ++contadorCompartilhado;
            }
            Console.WriteLine($">> Thread [{idThread}] | Contador: {contadorCompartilhado}");
            
            int timeWaiting = random.Next(1,50); 
            Console.WriteLine($">> Thread [{idThread}] | Waiting {timeWaiting} ms...");
            Thread.Sleep(timeWaiting);
        }
    }
}
