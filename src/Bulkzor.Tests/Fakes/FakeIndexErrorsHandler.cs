//using System;
//using System.Collections.Generic;
//using Bulkzor.Errors;
//using Bulkzor.Results;
//using Bulkzor.Types;

//namespace Bulkzor.Tests.Fakes
//{
//    public class FakeIndexErrorsHandler : IHandleIndexErrors
//    {
//        private readonly Func<IndexingError, IReadOnlyList<object>, IndexObjectsResult> _resultOverride;

//        public FakeIndexErrorsHandler(Func<IndexingError, IReadOnlyList<object>, IndexObjectsResult> resultOverride)
//        {
//            _resultOverride = resultOverride;
//        }

//        public IndexObjectsResult HandleError(IndexingError error, IReadOnlyList<object> objectsNotIndexed, string indexName, string typeName)
//        {
//            return _resultOverride(error, objectsNotIndexed);
//        }
//    }
//}
