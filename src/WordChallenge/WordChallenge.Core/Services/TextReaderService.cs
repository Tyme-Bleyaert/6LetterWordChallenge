using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordChallenge.Core.Interfaces;

namespace WordChallenge.Core.Services
{
    public class TextReaderService : ITextFileReader
    {
        private readonly ILogger<TextReaderService> _logger;
        private const string FileNotFoundErrorMsg = "Unable to find file :{filePath}";

        public TextReaderService(ILogger<TextReaderService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> GetTextLines(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogError(FileNotFoundErrorMsg, filePath);
                return Array.Empty<string>();
            }

            return File.ReadAllLines(filePath);
        }

        public byte[] ReadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogError(FileNotFoundErrorMsg, filePath);
                return Array.Empty<byte>();
            }

            return File.ReadAllBytes(filePath);
        }

    }
}
