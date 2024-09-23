// Exercício 3: Frequência das Palavras no Texto (2 pontos)
//
// Objetivo: Utilizar paralelismo em tarefas de processamento de texto.
//
// Descrição: Escreva um programa que recebe o nome de um arquivo de texto, um inteiro N
// e N palavras. O programa deve contar, em paralelo, a quantidade de vezes que cada
// palavra do conjunto aparece no texto contido no arquivo indicado na entrada.
//
// Observação: O programa não deve distinguir letras maiúsculas e minúsculas. Por exemplo,
// "Paralelo" e "paralelo", devem ser consideradas como a mesma palavra pelo programa.

using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string fileName = Console.ReadLine();
        int N = int.Parse(Console.ReadLine());

        string[] words = new string[N];

        for (int i = 0; i < N; i++)
        {
            words[i] = Console.ReadLine();
        }

        string text;
        try
        {
            text = File.ReadAllText(fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao ler o arquivo: ", e.Message);
            return;
        }

        Parallel.ForEach(words, word =>
        {
            string escapedWord = Regex.Escape(word);
            string pattern = $@"^{escapedWord}|{escapedWord}$|\b{escapedWord}\b";
            Console.WriteLine($"{word}: {Regex.Matches(text, pattern, RegexOptions.IgnoreCase).Count}");            
        });

        Thread.Sleep(3000);
    }
}