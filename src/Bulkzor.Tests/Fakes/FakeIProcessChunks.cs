using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Models;
using Bulkzor.Processors;
using Bulkzor.Results;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIProcessChunks<T> : IProcessChunks<T>
        where T : class, IIndexableObject
    {
        private readonly Func<IReadOnlyList<Chunk<T>>, ObjectsProcessedResult> _resultOverride;

        public FakeIProcessChunks(Func<IReadOnlyList<Chunk<T>>, ObjectsProcessedResult> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public ObjectsProcessedResult ProcessChunks(IReadOnlyList<Chunk<T>> chunks) 
        {
             return _resultOverride(chunks);
        }
    }
}
