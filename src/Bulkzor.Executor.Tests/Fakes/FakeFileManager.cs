using System.Collections.Generic;
using Bulkzor.Executor.Helpers;

namespace Bulkzor.Executor.Tests.Fakes
{
    public class FakeFileManager : IFileManager
    {
        public Dictionary<string, string> Files { get; } = new Dictionary<string, string>();
        public string ReadTextFromFile(string filePath)
        {
            return Files[filePath];
        }
    }
}
