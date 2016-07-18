using System.Collections.Generic;
using System.IO;
using Bulkzor.Utilities;
using Newtonsoft.Json;

namespace Bulkzor.Storage
{
    public class InFileObjectsStorage
        : IStoreObjects
    {
        private readonly string _rootDirectoryPath;

        public InFileObjectsStorage(string rootDirectoryPath)
        {
            _rootDirectoryPath = rootDirectoryPath;
        }

        public void StoreObjects(string directoryName, IEnumerable<object> objects, string indexName, string typeName)
        {
            var fullDirectoryPath = Path.Combine(_rootDirectoryPath, directoryName);

            Directory.CreateDirectory(fullDirectoryPath);

            foreach (var @object in objects)
            {
                var json = JsonConvert.SerializeObject(new
                {
                    index = indexName,
                    type = typeName,
                    @object
                });

                var fileNameFormat = $"{@object.GetIdFromUnknowObject()}_{indexName}_{typeName}.txt";
                var fullFilePath = Path.Combine(fullDirectoryPath, fileNameFormat);

                File.WriteAllText(fullFilePath, json);
            }
        }
    }
}
