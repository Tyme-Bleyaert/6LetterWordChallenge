using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Eventing.Reader;
using WordChallenge.Core.Extensions;
using WordChallenge.Core.Interfaces;
using Stm = System;

namespace WordChallenge.Console
{
    internal class Program
    {
        static ILogger<Program>? _logger;

        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddLogging();
            builder.Services.AddWordChallengeServices();

            using IHost host = builder.Build();

            _logger = host.Services.GetService<ILogger<Program>>();
            DetermineFileToUse(host);

            await host.RunAsync();

        }

        static void DetermineFileToUse(IHost host)
        {
            Stm.Console.WriteLine("Give path to input file or press enter to use default input.");
            string? path = Stm.Console.ReadLine();

            if (string.IsNullOrEmpty(path)) 
            {
                OpenAndProcessFile(host, @"input.txt");
                return;
            }

            OpenAndProcessFile(host, path);
        }

        static void OpenAndProcessFile(IHost host, string path) 
        {
            var textReader = host.Services.GetService<ITextFileReader>();
            var combinationProv = host.Services.GetService<ICombinationProvider>();

            //Filter out duplicate strings because we don't want duplicate strings on the heap.
            var wordList = textReader?.GetTextLines(path).Distinct().ToList();

            if(wordList is null || wordList.Count == 0)
            {
                _logger?.LogError("No words found in file: {path}", path);
                return;
            }

            //Group words per length
            Stm.Console.WriteLine("Give a number for the max allowed word length, or leave empty to use the longest word of the input file");
            var wordLength = Stm.Console.ReadLine();

            if(int.TryParse(wordLength, out var length))
            {
                wordList = wordList.Where(x => x.Length <= length).ToList();
            }
            else if (!string.IsNullOrEmpty(wordLength))
            {
                _logger?.LogWarning("Invalid number input : {input}. Using default length.", wordLength);
            }


            Stm.Console.WriteLine("Give a number number of combinations you want words to consist of.");
            var combinationAmount = Stm.Console.ReadLine();

            IList<string>? combinationResults = null;

            if (int.TryParse(combinationAmount, out var combination))
            {
                combinationResults = combinationProv?.GetCombinations(wordList, combination).ToList();
            }
            else if (!string.IsNullOrEmpty(combinationAmount))
            {
                _logger?.LogWarning("Invalid number input : {input}. Finding all combinations.", combinationAmount);
            }
            else
            {
                combinationResults = combinationProv?.GetAllPossibleCombinations(wordList).ToList();
            }

            if(combinationResults is null || combinationResults.Count == 0)
            {
                _logger?.LogWarning("No valid combinations found");
                return;
            }

            Stm.Console.WriteLine($"Found {combinationResults.Count} combinations:");
            foreach (var wordCombination in combinationResults)
            {
                Stm.Console.WriteLine(wordCombination);
            }
        }

        
    }
}
