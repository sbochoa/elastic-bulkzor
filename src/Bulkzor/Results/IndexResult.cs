using System;

namespace Bulkzor.Results
{
    public class IndexResult
    {
        public int TotalDocumentsIndexed { get; }
        public int TotalDocumentsNotIndexed { get; }
        public bool Errors { get; }

        public IndexResult(int totalDocumentsIndexed, int totalDocumentsNotIndexed, bool errors)
        {
            TotalDocumentsIndexed = totalDocumentsIndexed;
            TotalDocumentsNotIndexed = totalDocumentsNotIndexed;
            Errors = errors;
        }
    }
}