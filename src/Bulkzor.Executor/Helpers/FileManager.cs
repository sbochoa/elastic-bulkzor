namespace Bulkzor.Executor.Helpers
{
    public class FileManager : IFileManager
    {
        public string ReadTextFromFile(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }
    }
}
