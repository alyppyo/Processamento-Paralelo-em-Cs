// Exercício 5: Pokédex (3 pontos)
//
// Objetivo: Utilizar o TPL para realizar requisições HTTP de forma assíncrona.
//
// Descrição: O programa receberá uma lista com P números inteiros, que representam os
// identificadores de determinados pokémons na Pokédex. Para cada identificador na lista, a
// aplicação deverá fazer uma requisição na PokeAPI para obter o nome e o(s) tipo(s) do
// pokémon correspondente. Ao final, salve essas informações no arquivo pokedex.txt,
// colocando os dados de um pokémon por linha. Utilize o seguinte formato para salvar as
// informações: nome, tipo_1,..., tipo_N. Lembre-se de que um pokémon pode ter mais de um
// tipo.
//
// A. Implemente uma primeira versão sem paralelismo/concorrência
// B. Implemente outra versão usando Parallel.For ou Parallel.ForEach
// C. Implemente uma terceira versão usando funções assíncronas (async/await)
//
// D. Calcule o speedup das versões B e C em relação a versão A. O que você observa?
// R:
//          |                    Entrada            
//  Solução |   t1.in    t2.in    t3.in    t4.in    t5.in 
// ---------------------------------------------------------
//     A    |  5498ms   6229ms   6395ms  24494ms  31780ms
//     B    |  1021ms   1594ms   1383ms   5106ms   6605ms
//     C    |  1425ms   3197ms   3842ms   7440ms  16884ms
//
// De acordo com os resultados obtidos, podemos verificar uma média de aceleração de 4,71x
// para a Solução B (Parallel.For/ForEach) e de 2,53 para a Solução C (funções assíncronas).
// Mesmo não sendo tão expressivo quanto à Solução B, a Solução B consegue uma excelente
// aceleração, apenas por adicionar uma simples camada de assincronia ao processo.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using File = System.IO.File;

class Program
{
    static async Task Main(string[] args)
    {
        string[] input = Console.ReadLine().Split(' ');
        int P = input.Length;
        int[] pokemonIds = new int[P];

        for (int i = 0; i < P; i++)
        {
            pokemonIds[i] = int.Parse(input[i]);
        }

        using HttpClient client = new();

        Stopwatch cronometro = new();
        
        cronometro.Start();
        ObterESalvarInfoPokemonsA(client, pokemonIds);
        cronometro.Stop();
        long tempoSolucaoA = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        ObterESalvarInfoPokemonsB(client, pokemonIds);
        cronometro.Stop();
        long tempoSolucaoB = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        await ObterESalvarInfoPokemonsC(client, pokemonIds);
        cronometro.Stop();
        long tempoSolucaoC = cronometro.ElapsedMilliseconds;

        Console.WriteLine(":: Pokédex ::");
        Console.WriteLine($"Solução A: {tempoSolucaoA}ms");
        Console.WriteLine($"Solução B: {tempoSolucaoB}ms");
        Console.WriteLine($"Solução C: {tempoSolucaoC}ms");
        Thread.Sleep(10000);
    }

    static void ObterESalvarInfoPokemonsA(HttpClient client, int[] pokemonIds)
    {
        List<string> pokemonInfoLista = new List<string>();

        for (int i = 0; i < pokemonIds.Length; i++)
        {
            string pokemonInfoJson = ObterInformacoesDoPokemonAsync(client, pokemonIds[i]).Result;
            string pokemonInfo = ProcessarConteudoJson(pokemonInfoJson);
            pokemonInfoLista.Add(pokemonInfo);
        }

        File.WriteAllLines("pokedexA.txt", pokemonInfoLista);
    }

    static void ObterESalvarInfoPokemonsB(HttpClient client, int[] pokemonIds)
    {
        ConcurrentBag<string> pokemonInfoLista = new();

        Parallel.ForEach(pokemonIds, pokemonId =>
        {
            string pokemonInfoJson = ObterInformacoesDoPokemonAsync(client, pokemonId).Result;
            string pokemonInfo = ProcessarConteudoJson(pokemonInfoJson);
            pokemonInfoLista.Add(pokemonInfo);
        });

        File.WriteAllLines("pokedexB.txt", pokemonInfoLista);
    }

    static async Task ObterESalvarInfoPokemonsC(HttpClient client, int[] pokemonIds)
    {
        ConcurrentBag<string> pokemonInfoLista = new();

        for (int i = 0; i < pokemonIds.Length; i++)
        {
            string pokemonInfoJson = await ObterInformacoesDoPokemonAsync(client, pokemonIds[i]);
            string pokemonInfo = ProcessarConteudoJson(pokemonInfoJson);
            pokemonInfoLista.Add(pokemonInfo);
        }

        File.WriteAllLines("pokedexC.txt", pokemonInfoLista);
    }

    static async Task<string> ObterInformacoesDoPokemonAsync(HttpClient client, int id)
    {
        string url = $@"https://pokeapi.co/api/v2/pokemon/{id}";
        return await client.GetStringAsync(url);
    }

    static string ProcessarConteudoJson(string conteudo)
    {
        JsonDocument pokemonDetalhes = JsonDocument.Parse(conteudo);
        string nome = pokemonDetalhes.RootElement.GetProperty("name").ToString();
        JsonElement listaTiposJson = pokemonDetalhes.RootElement.GetProperty("types");

        List<string> listaTipos = [nome];

        foreach (JsonElement tipo in listaTiposJson.EnumerateArray())
        {
            listaTipos.Add(tipo.GetProperty("type").GetProperty("name").GetString());
        }

        return string.Join(", ",listaTipos);
    }
}