﻿using System.Collections.Generic;
using Bulkzor.Results;

namespace Bulkzor.Storage
{
    public interface IStoreObjects
    {
        void StoreObjects(IEnumerable<object> objects, string indexName, string typeName);
    }
}
