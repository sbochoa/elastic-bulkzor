using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Models;
using Bulkzor.Processors;
using Bulkzor.Results;

namespace Bulkzor.Tests.Fakes
{
    public class FakeIProcessChunks : IProcessChunks
    {
        private readonly Func<IReadOnlyList<Chunk>, ObjectsProcessedResult> _resultOverride;

        public FakeIProcessChunks(Func<IReadOnlyList<Chunk>, ObjectsProcessedResult> resultOverride)
        {
            _resultOverride = resultOverride;
        }

        public ObjectsProcessedResult ProcessChunks(IReadOnlyList<Chunk> chunks) 
        {
             return _resultOverride(chunks);
        }
    }
}
