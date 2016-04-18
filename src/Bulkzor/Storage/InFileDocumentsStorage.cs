using System.Collections.Generic;

namespace Bulkzor.Storage
{
    public class InFileDocumentsStorage
        : IStoreDocuments
    {
        public void StoreDocuments<T>(IEnumerable<T> documents, string indexName, string typeName)
        {
            // TODO : Store documents not indexed
            throw new System.NotImplementedException();
        }
    }
}
