using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexDocumentsResult
    {
        public IndexDocumentsResult(int documentsIndexed, int documentsNotIndexed, IndexingError error)
        {
            DocumentsIndexed = documentsIndexed;
            DocumentsNotIndexed = documentsNotIndexed;
            Error = error;
        }

        public IndexDocumentsResult(int documentsIndexed, int documentsNotIndexed)
        {
            DocumentsIndexed = documentsIndexed;
            DocumentsNotIndexed = documentsNotIndexed;
            Error = DocumentsNotIndexed > 0 ? IndexingError.OnlyPartOfDocumentsIndexed : IndexingError.None;
        }

        public int DocumentsIndexed { get; }
        public int DocumentsNotIndexed { get; }
        public IndexingError Error { get; }

        public bool HaveError => Error != IndexingError.None;
    }
}
