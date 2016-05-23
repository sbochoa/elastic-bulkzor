using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Storage
{
    public class InFileObjectsStorage
        : IStoreObjects
    {
        public void StoreObjects(IEnumerable<object> objects, string indexName, string typeName)
        {
            // TODO : Store objects not indexed
            throw new System.NotImplementedException();
        }
    }
}
