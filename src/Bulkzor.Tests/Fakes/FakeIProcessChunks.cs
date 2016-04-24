using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Models;
using Bulkzor.Processors;
using Bulkzor.Results;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIProcessChunks
        : IProcessChunks
    {
        private readonly Func<ObjectsProcessedResult, ObjectsProcessedResult> _resultOverride;

        public FakeIProcessChunks(Func<ObjectsProcessedResult, ObjectsProcessedResult> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public ObjectsProcessedResult ProcessChunks<T>(IReadOnlyList<Chunk<T>> chunks) 
            where T : class, IIndexableObject
        {
             return _resultOverride(new ObjectsProcessedResult(chunks.Sum(c => c.Data.Count), 0));
        }
    }
}
