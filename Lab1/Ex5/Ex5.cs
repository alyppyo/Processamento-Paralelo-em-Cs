using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    private static Queue<string> _directories = new Queue<string>();

    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Uso: Programa <ArquivoDeEntrada>");
            return;
        }

        string inputFilePath = args[0];
        string inputFileDirectory = Path.GetDirectoryName(Path.GetFullPath(inputFilePath));

        // Ler o arquivo de entrada
        string[] lines = File.ReadAllLines(inputFilePath);
        if (lines.Length < 2)
        {
            Console.WriteLine("O arquivo de entrada deve conter ao menos duas linhas");
            return;
        }

        string keyword = lines[0];
        string[] directories = new string[lines.Length - 1];
        Array.Copy(lines, 1, directories, 0, directories.Length);

        // Inicializa a fila com os caminhos absolutos dos diretórios
        foreach (var dir in directories)
        {
            string absolutePath = Path.Combine(inputFileDirectory, dir);
            _directories.Enqueue(absolutePath);
        }

        // Continue a Implementação
        foreach(var dir in _directories)
        {
            new Thread(() => ProcessDirectory(dir, keyword)).Start();
        }
    }

    // Função que pesquisa a palavra chave nos arquivos de um diretório
    static void ProcessDirectory(string directory, string keyword)
    {
        if (!Directory.Exists(directory))
        {
            Console.WriteLine($"O Diretório não existe: {directory}");
            return;
        }

        Console.WriteLine($"Verificando diretório: {directory}");

        string[] files = Directory.GetFiles(directory, "*.txt");
        foreach (string file in files)
        {
            try
            {
                // Ler todas as linhas do arquivo
                string[] lines = File.ReadAllLines(file);

                // Verificar cada linha para encontrar a keyword
                foreach (string line in lines)
                {
                    if (line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"keyword '{keyword}' encontrada no arquivo: {file}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo {file}: {ex.Message}");
            }
        }
    }
}
