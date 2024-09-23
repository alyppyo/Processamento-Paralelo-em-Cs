// Exercício 2: Estatísticas de um Conjunto de Dados (1.5 pontos)
//
// Objetivo: Familiarizar - se com a criação e gerenciamento de tasks em C#.
//
// Descrição: Escreva um programa que recebe um número inteiro N e gera um vetor de
// números reais (double) com N elementos aleatórios. Em seguida, o programa deve calcular
// a média, mediana, variância e desvio padrão do vetor usando tarefas (tasks). Ao final, o
// programa deve imprimir essas estatísticas do vetor no console.
//
// Observação: Algumas das estatísticas possuem dependências entre si. Por exemplo, o
// cálculo da variância depende do valor da média. Considere como essas dependências
// podem ser exploradas para otimizar a construção e a execução das tarefas.

class Program
{
    static void Main(string[] args)
    {
        // Ler N da entrada
        Console.WriteLine(":: Média, Mediana, Variância e Desvio Padrão ::");
        Console.Write("- Informe o tamanho do array: ");
        int N = int.Parse(Console.ReadLine());

        double[] sequencia = new double[N];

        // Inicializar o array com reais aleatórios entre 0 e 100
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequencia[i] = rand.NextDouble() * 100;
        }

        // Ordenar o array (necessário pra mediana e não influencia nos demais cálculos)
        Array.Sort(sequencia);

        Task<double> taskMedia = Task.Run(() => Media(sequencia));
        Task<double> taskMediana = Task.Run(() => Mediana(sequencia));
        Task<double> taskVariancia = taskMedia.ContinueWith(taskMedia => Variancia(sequencia, taskMedia.Result));
        Task<double> taskDesvioPadrao = taskVariancia.ContinueWith(taskVariancia => DesvioPadrao(taskVariancia.Result));

        Task.WaitAll(taskMedia, taskMediana, taskVariancia, taskDesvioPadrao);

        Console.WriteLine($"> Média: {taskMedia.Result}");
        Console.WriteLine($"> Mediana: {taskMediana.Result}");
        Console.WriteLine($"> Variância: {taskVariancia.Result}");
        Console.WriteLine($"> Desvio Padrão: {taskDesvioPadrao.Result}");
    }

    static double Media(double[] sequencia)
    {
        return sequencia.Average();
    }

    static double Mediana(double[] sequencia)
    {
        int middle = sequencia.Length / 2;

        if(sequencia.Length % 2 != 0)
        {
            return sequencia[middle];
        }

        return (sequencia[middle-1]+ sequencia[middle])/2;
    }

    static double Variancia(double[] sequencia, double media)
    {
        return sequencia.Select(val => Math.Pow(val- media, 2)).Sum(val => val)/sequencia.Length;
    }

    static double DesvioPadrao(double variancia)
    {
        return Math.Sqrt(variancia);
    }
}