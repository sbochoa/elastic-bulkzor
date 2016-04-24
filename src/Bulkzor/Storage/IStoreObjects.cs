using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Storage
{
    public interface IStoreObjects
    {
        void StoreObjects<T>(IEnumerable<T> objects, string indexName, string typeName)
            where T : class, IIndexableObject;
    }
}
