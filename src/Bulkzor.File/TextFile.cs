using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bulkzor.File
{
    public class TextFile<T>
        : ISource<T>
        where T : class
    {
        private readonly string _path;
        private readonly string _extension;

        public TextFile(string path)
        {
            _path = path;
            _extension = "*.txt";
        }

        public TextFile(string path, string extension)
        {
            _path = path;
            _extension = $"*.{extension}";
        }

        public IEnumerable<T> GetData()
        {
            var attributes = System.IO.File.GetAttributes(_path);
            var isDirectory = attributes.HasFlag(FileAttributes.Directory);

            if (isDirectory)
            {
                return GetObjectsFromDirectory(_path);
            }

            return GetObjectsFromFile(_path);
        }

        private IEnumerable<T> GetObjectsFromDirectory(string directoryPath)
        {
            var fileInfos = new DirectoryInfo(directoryPath).GetFiles(_extension);

            return fileInfos.SelectMany(fileInfo => GetObjectsFromFile(fileInfo.FullName));
        }

        private IEnumerable<T> GetObjectsFromFile(string filePath)
        {
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        yield return JsonConvert.DeserializeObject<T>(line);
                    }
                }
            }
        }
    }
}
