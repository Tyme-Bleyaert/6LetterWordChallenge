using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordChallenge.Core.Interfaces;

namespace WordChallenge.Core.Services
{
    //TODO : if we want to increase speed we can prepare a file and cache all results in a dictionary.
    internal class StringCombinationService : ICombinationProvider
    {
        private readonly ILogger _logger;

        public StringCombinationService(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> GetAllPossibleCombinations(IEnumerable<string> input)
        {
            _logger.LogInformation("Starting word combination search.");

            if(input is null || !input.Any())
                return Enumerable.Empty<string>();

            var wordsPerLength = input.GroupBy(x => x.Length).ToList();
            var maxWordLength = wordsPerLength.Max(x => x.Key);

            var possibleCombinations = GetCombinationPossibilities(maxWordLength);

            return GetWordCombinations(wordsPerLength, possibleCombinations);
        }

        public IEnumerable<string> GetCombinations(IEnumerable<string> input, int combinationAmount = 2)
        {
            _logger.LogInformation("Starting word combination search with {combinationAmount}.", combinationAmount);

            if (input is null || !input.Any())
                return Enumerable.Empty<string>();

            var wordsPerLength = input.GroupBy(x => x.Length).ToList();
            var maxWordLength = wordsPerLength.Max(x => x.Key);

            var possibleCombinations = GetCombinationPossibilities(maxWordLength);
            var matchingCombinations = possibleCombinations.Where(x => x.Length == combinationAmount).ToList();

            return GetWordCombinations(wordsPerLength, matchingCombinations);
        }

        private static IEnumerable<string> GetWordCombinations(IEnumerable<IGrouping<int, string>> input, IEnumerable<int[]> possibilities)
        {
            var wordsToFind = input.OrderBy(x => x.Key).LastOrDefault().ToList();

            foreach (var word in wordsToFind)
            {
                if (word.Contains(' '))
                    continue;

                foreach (var possibility in possibilities)
                {
                    //TODO : Check each possibility position of each word. eg: [2,4] or [4,2], we need to check both.
                    int offset = 0;
                    string returnValue = string.Empty;

                    for (int i = 0; i < possibility.Length; i++)
                    {
                        var subpart = word.Substring(offset, possibility[i]);
                        var matchingGroup = input.Where(x => x.Key == possibility[i]).FirstOrDefault();
                        offset += possibility[i];

                        if (matchingGroup != null && matchingGroup.Contains(subpart))
                        {
                            returnValue += subpart;
                            if (i + 1 != possibility.Length)
                                returnValue += '+';
                        }
                        else
                        {
                            goto Next;
                        }
                    }

                    returnValue += $"={word}";
                    yield return returnValue;

                Next:;
                }
            }
        }


        private static List<int[]> GetCombinationPossibilities(int length)
        {
            if(length == 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var returnValue = new List<int[]>();

            returnValue.AddRange(GetDefaultPossibilities(length));
            returnValue.AddRange(GetAdvancedPossibilities(length));

            return returnValue;
        }

        private static IEnumerable<int[]> GetDefaultPossibilities(int length)
        {
            for (int i = 0; i < length - 1; i++)
            {
                var possibilities = new int[length - i];
                for (int j = 0; j < length - i - 1; j++)
                {
                    possibilities[j] = 1;
                }
                possibilities[length - i - 1] = i + 1;
                yield return possibilities;
            }
        }

        private static IEnumerable<int[]> GetAdvancedPossibilities(int length)
        {
            for (int i = length - 2; i >= 2; i--)
            {
                var listOfOtherPossibilities = new List<int>();

                //TODO: We need to continue here to check if there are multiple possibilities for each i value.
                for (int y = length - i; y >= 1; y--)
                {
                    if (y > i)
                    {
                        y = (int)Math.Ceiling((double)y / 2);
                    }

                    listOfOtherPossibilities.Add(y);

                    if (i + y == length || (length - i == 2 && i + y + 1 == length))
                    {
                        break;
                    }
                    else if (i + y * 2 == length)
                    {
                        listOfOtherPossibilities.Add(y);
                        break;
                    }
                }

                var possibilities = new int[listOfOtherPossibilities.Count + 1];

                possibilities[0] = i;

                for (int x = 1; x < possibilities.Length; x++)
                {
                    possibilities[x] = listOfOtherPossibilities[x - 1];
                }

                yield return possibilities;
            }
        }
    }
}
