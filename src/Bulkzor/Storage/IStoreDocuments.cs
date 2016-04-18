using System.Collections.Generic;

namespace Bulkzor.Storage
{
    public interface IStoreDocuments
    {
        void StoreDocuments<T>(IEnumerable<T> documents, string indexName, string typeName);
    }
}
