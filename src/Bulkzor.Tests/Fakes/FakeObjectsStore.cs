﻿using System;
using System.Collections.Generic;
using Bulkzor.Storage;

namespace Bulkzor.Tests.Fakes
{
    public class FakeObjectsStore : IStoreObjects
    {
        public void StoreObjects(IEnumerable<object> objects, string indexName, string typeName)
        {
            
        }
    }
}
