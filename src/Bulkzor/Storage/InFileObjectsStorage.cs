using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Storage
{
    public class InFileObjectsStorage
        : IStoreObjects
    {
        public void StoreObjects<T>(IEnumerable<T> objects, string indexName, string typeName)
            where T : class, IIndexableObject
        {
            // TODO : Store objects not indexed
            throw new System.NotImplementedException();
        }
    }
}
