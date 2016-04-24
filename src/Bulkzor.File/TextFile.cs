using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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

        public IEnumerable<T> GetData<T>()
            where T : class, IIndexableObject
        {
            var attributes = System.IO.File.GetAttributes(_path);
            var isDirectory = attributes.HasFlag(FileAttributes.Directory);

            return isDirectory ? GetObjectsFromDirectory<T>(_path) : GetObjectsFromFile<T>(_path);
        }

        private IEnumerable<T> GetObjectsFromDirectory<T>(string directoryPath)
            where T : class
        {
            var fileInfos = new DirectoryInfo(directoryPath).GetFiles(_extension ?? DefaultExtension);

            return fileInfos.SelectMany(fileInfo => GetObjectsFromFile<T>(fileInfo.FullName));
        }

        private IEnumerable<T> GetObjectsFromFile<T>(string filePath)
            where T : class
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
                                yield return DeserializeFromJson<T>(line);
                                break;
                            case FormatType.Xml:
                                yield return DeserializeFromXml<T>(line);
                                break;
                            default:
                                throw new InvalidOperationException(nameof(_formatType));
                        }
                    }
                }
            }
        }

        private static T DeserializeFromJson<T>(string line)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(line);
        }

        private static T DeserializeFromXml<T>(string line)
            where T : class
        {
            return (T) new XmlSerializer(typeof (T)).Deserialize(new StringReader(line));
        }
    }
}
