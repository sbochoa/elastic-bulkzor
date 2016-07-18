using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bulkzor.File
{
    public class TextFile
        : ISource
        
    {
        private readonly string _path;
        private readonly FormatType _formatType;
        private readonly string _extension;
        private static readonly string DefaultExtension = "*.txt";

        public TextFile(string path, FormatType formatType)
        {
            _path = path;
            _formatType = formatType;
        }

        public TextFile(string path, FormatType formatType, string extension)
            : this(path, formatType)
        {
            _extension = $"*.{extension.TrimStart('*', '.')}";
        }

        public IEnumerable<object> GetData()
        {
            var attributes = System.IO.File.GetAttributes(_path);
            var isDirectory = attributes.HasFlag(FileAttributes.Directory);

            return isDirectory ? GetObjectsFromDirectory(_path) : GetObjectsFromFile(_path);
        }

        private IEnumerable<object> GetObjectsFromDirectory(string directoryPath)
        {
            var fileInfos = new DirectoryInfo(directoryPath).GetFiles(_extension ?? DefaultExtension);

            return fileInfos.SelectMany(fileInfo => GetObjectsFromFile(fileInfo.FullName));
        }

        private IEnumerable<object> GetObjectsFromFile(string filePath)
        {
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        switch (_formatType)
                        {
                            case FormatType.Json:
                                yield return DeserializeFromJson(line);
                                break;
                            case FormatType.Xml:
                                yield return DeserializeFromXml(line);
                                break;
                            default:
                                throw new InvalidOperationException(nameof(_formatType));
                        }
                    }
                }
            }
        }

        private static object DeserializeFromJson(string line)
        {
            return JsonConvert.DeserializeObject<object>(line);
        }

        private static object DeserializeFromXml(string line)
        {
            //TODO deserialize object from XML
            throw new NotImplementedException();
            //return new XmlSerializer().Deserialize(new StringReader(line));
        }
    }
}
