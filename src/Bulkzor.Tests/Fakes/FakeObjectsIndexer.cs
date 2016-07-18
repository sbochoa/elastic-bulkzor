using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Tests.Fakes
{
    public class FakeObjectsIndexer : IObjectIndexer
    {
        public IReadOnlyList<object> ObjectsNotIndexed { get; set; } = new List<object>();
        public IndexingError? IndexingError { get; set; }

        public IndexObjectsResult Index(IReadOnlyList<object> objects, string indexName, string typeName)
        {
            var objectsIndexed = objects.Except(ObjectsNotIndexed).ToList();
            var objectsNotIndexed = ObjectsNotIndexed.Where(objects.Contains).ToList();
            if (IndexingError != null)
            {
                return new IndexObjectsResult(objectsIndexed, objectsNotIndexed, IndexingError.Value);
            }
            return new IndexObjectsResult(objectsIndexed, objectsNotIndexed);
        }
    }
}
