namespace Bulkzor.Results
{
    public class DocumentIndexingResult
    {
        public DocumentIndexingResult(int documentsIndexed, int documentsNotIndexed)
        {
            DocumentsNotIndexed = documentsNotIndexed;
            DocumentsIndexed = documentsIndexed;
        }

        public int DocumentsIndexed { get; }
        public int DocumentsNotIndexed { get; }
    }
}