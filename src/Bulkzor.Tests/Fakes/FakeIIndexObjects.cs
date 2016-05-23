using System;
using System.Collections.Generic;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIIndexObjects : IIndexObjects
    {
        private readonly Func<IReadOnlyList<object>, IndexObjectsResult> _resultOverride;

        public FakeIIndexObjects(Func<IReadOnlyList<object>, IndexObjectsResult> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public IndexObjectsResult Index(IReadOnlyList<object> objects, string indexName, string typeName)
        {
            return _resultOverride(objects);
        }
    }
}
