using System.Collections.Generic;

namespace Bulkzor.Events
{
    public class MaxLengthExceeded<T>
        where T : class
    {
        public IReadOnlyList<T> Documents { get; }
        public string IndexName { get; }
        public string TypeName { get; }

        public MaxLengthExceeded(IReadOnlyList<T> documents, string indexName, string typeName)
        {
            Documents = documents;
            IndexName = indexName;
            TypeName = typeName;
        }
    }
}
