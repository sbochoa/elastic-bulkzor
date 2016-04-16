using System;
using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexResult
    {
        public IndexResult(int documentsIndexed, int documentsNotIndexed, TimeSpan timeElapsed, IndexingError error)
            : this(documentsIndexed, documentsNotIndexed, timeElapsed)
        {
            Error = error;
        }

        public IndexResult(int documentsIndexed, int documentsNotIndexed, TimeSpan timeElapsed)
        {
            DocumentsIndexed = documentsIndexed;
            DocumentsNotIndexed = documentsNotIndexed;
            TimeElapsed = timeElapsed;
            Error = documentsNotIndexed > 0 ? IndexingError.OnlyPartOfDocumentsIndexed : IndexingError.None;

            Message =
                $"{DocumentsIndexed} documents indexed - {DocumentsNotIndexed} documents not indexed - Ended in {TimeElapsed.ToString("g")}";
        }

        public int DocumentsIndexed { get; }
        public int DocumentsNotIndexed { get; }
        public TimeSpan TimeElapsed { get; }
        public IndexingError Error { get; }
        public string Message { get; }
    }
}