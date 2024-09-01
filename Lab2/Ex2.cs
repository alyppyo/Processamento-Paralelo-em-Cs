// Questão 2
// ----------
// Escreva um programa que recebe um número inteiro N e gera um vetor de
// números reais (double) com N elementos aleatórios. Em seguida, o programa deve calcular
// a média, mediana, variância e desvio padrão do vetor usando tarefas (tasks). Ao final, o
// programa deve imprimir essas estatísticas do vetor no console.
// Observação: Algumas das estatísticas possuem dependências entre si. Por exemplo, o
// cálculo da variância depende do valor da média. Considere como essas dependências
// podem ser exploradas para otimizar a construção e a execução das tarefas.

using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(Console.ReadLine());

        double[] sequence = new double[N];

        // Inicializar o array com reais aleatórios entre 0 e N
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.NextDouble() * 100;
        }

        // Continue a Implementação
        // ...

    }

    static double Media(int[] sequence)
    {
        return sequence.
    }

    static double Mediana(int[] sequence)
    {
        
    }

    static double Variancia(int[] sequence)
    {
        
    }

    static double DesvioPadrao(int[] sequence)
    {
        
    }
}