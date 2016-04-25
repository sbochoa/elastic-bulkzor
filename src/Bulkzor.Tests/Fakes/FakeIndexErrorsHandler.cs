using System;
using System.Collections.Generic;
using Bulkzor.Errors;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIndexErrorsHandler<T> : IHandleIndexErrors<T>
        where T : class, IIndexableObject
    {
        private readonly Func<IndexingError, IReadOnlyList<T>, IndexObjectsResult<T>> _resultOverride;

        public FakeIndexErrorsHandler(Func<IndexingError, IReadOnlyList<T>, IndexObjectsResult<T>> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public IndexObjectsResult<T> HandleError(IndexingError error, IReadOnlyList<T> objectsNotIndexed, string indexName, string typeName)
        {
            return _resultOverride(error, objectsNotIndexed);
        }
    }
}
