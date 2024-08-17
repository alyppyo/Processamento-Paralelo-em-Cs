using System;
using System.Threading;

public class Program
{
    static void Main(string[] args)
    {
        // Ler o valor de N da entrada
        int.TryParse(Console.ReadLine(), out int N);

        // Criar um array de strings de tamanho N
        string[] frases = new string[N];

        // Ler as N frases da entrada e salvá-las no array
        for (int i = 0; i < N; ++i)
        {
            frases[i] = Console.ReadLine();
        }

        // Continue a Implementação (Criar as threads e etc)
        for(int i = 0; i < N; ++i)
        {
            int id = i;
            new Thread(() => WritePhrase(id+1, frases[id])).Start();
        }
    }

    static void WritePhrase(int idThread, string text)
    {
        Console.WriteLine($"Thread {idThread}: {text}");
    }
}
