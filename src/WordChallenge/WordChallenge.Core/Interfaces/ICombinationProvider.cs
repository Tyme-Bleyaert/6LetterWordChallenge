using System;
using System.Collections.Generic;
using System.Text;

namespace WordChallenge.Core.Interfaces
{
    public interface ICombinationProvider
    {
        IEnumerable<string> GetAllPossibleCombinations(IEnumerable<string> input);
        IEnumerable<string> GetCombinations(IEnumerable<string> input, int combinationAmount = 2);
    }
}
