using System.Collections.Generic;

namespace Bulkzor.Commands
{
    public class StoreDocumentsNotIndexed<TDocument>
        where TDocument : class
    {
        public StoreDocumentsNotIndexed(IReadOnlyList<TDocument> documentsNotIndexed, string indexName, string typeName)
        {
            DocumentsNotIndexed = documentsNotIndexed;
            IndexName = indexName;
            TypeName = typeName;
        }

        public IReadOnlyList<TDocument> DocumentsNotIndexed { get; }
        public string IndexName { get; }
        public string TypeName { get; }
    }
}
