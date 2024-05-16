using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordChallenge.Core.Interfaces
{
    public interface IFileReader
    {
        byte[] ReadData(string filePath);
    }
}
