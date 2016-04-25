using System;
using System.Collections.Generic;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIIndexObjects<T> : IIndexObjects<T>
        where T : class, IIndexableObject
    {
        private readonly Func<IReadOnlyList<T>, IndexObjectsResult<T>> _resultOverride;

        public FakeIIndexObjects(Func<IReadOnlyList<T>, IndexObjectsResult<T>> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public IndexObjectsResult<T> Index(IReadOnlyList<T> objects, string indexName, string typeName)
        {
            return _resultOverride(objects);
        }
    }
}
