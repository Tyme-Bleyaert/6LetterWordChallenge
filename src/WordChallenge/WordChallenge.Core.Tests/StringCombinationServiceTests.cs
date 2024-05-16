using FakeItEasy;
using Microsoft.Extensions.Logging;
using WordChallenge.Core.Services;

namespace WordChallenge.Core.Tests
{
    [TestClass]
    public class StringCombinationServiceTests
    {
        //TODO add more tests when algorithms are finished.

        private readonly ILogger _logger;
        private readonly StringCombinationService _service;
        private static readonly List<string> sixLetterWords = new List<string>() { "modern", "mod", "ern", "mode", "rn" };
        private static readonly List<string> wordsWithSpaces = new List<string>() { "new era", "new", "era", "new e", "ra" };

        public StringCombinationServiceTests()
        {
            _logger = A.Fake<ILogger>();
            _service = new StringCombinationService(_logger);
        }

        [TestMethod]
        public void ShouldReturnCorrectCombinationFor6LetterWord()
        {

            var combinations = _service.GetAllPossibleCombinations(sixLetterWords).ToList();

            Assert.IsNotNull(combinations);
            CollectionAssert.Contains(combinations, $"mod+ern=modern");
            CollectionAssert.Contains(combinations, $"mode+rn=modern");
        }

        [TestMethod]
        public void ShouldNotReturnWordCombinationForWordsWithSpaces()
        {

            var combinations = _service.GetAllPossibleCombinations(wordsWithSpaces).ToList();

            Assert.IsTrue(combinations.Count == 0);
        }

        [TestMethod]
        public void ShouldNotThrowExceptionOnEmptyInput()
        {

            var combinations = _service.GetAllPossibleCombinations(Array.Empty<string>()).ToList();

            Assert.IsNotNull(combinations);
        }

        
    }
}