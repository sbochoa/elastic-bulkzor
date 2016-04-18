using System;
using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexResult
    {
        public IndexResult(int objectsIndexed, int objectsNotIndexed, TimeSpan timeElapsed)
        {
            ObjectsIndexed = objectsIndexed;
            ObjectsNotIndexed = objectsNotIndexed;
            TimeElapsed = timeElapsed;

            Message =
                $"{ObjectsIndexed} documents indexed - {ObjectsNotIndexed} documents not indexed - Ended in {TimeElapsed.ToString("g")}";
        }

        public int ObjectsIndexed { get; }
        public int ObjectsNotIndexed { get; }
        public TimeSpan TimeElapsed { get; }
        public string Message { get; }
    }
}