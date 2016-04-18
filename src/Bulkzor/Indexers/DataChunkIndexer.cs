using System.Diagnostics;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Indexers
{
    public class DataChunkIndexer
        : IIndexDataChunk
    {
        private readonly IIndexDocuments _documentsIndexer;
        private const int PartsQuantity = 3;

        public DataChunkIndexer(IIndexDocuments documentsIndexer)
        {
            _documentsIndexer = documentsIndexer;
        }

        public IndexResult IndexDataChunk<T>(DataChunk<T> dataChunk) 
            where T : class
        {
            var watch = new Stopwatch();

            watch.Start();

            var indexDocumentsResult = _documentsIndexer.IndexDocuments(dataChunk.Data, dataChunk.IndexName, dataChunk.TypeName);

            if (indexDocumentsResult.HaveError)
            {
                if (indexDocumentsResult.Error == IndexingError.LengthExceeded)
                {
                    indexDocumentsResult = IndexDataChunkInParts(dataChunk, PartsQuantity);
                }
                else 
                {
                    // TODO : Store documents not indexed
                }    
            }

            watch.Stop();

            return new IndexResult(indexDocumentsResult.DocumentsIndexed, indexDocumentsResult.DocumentsNotIndexed, watch.Elapsed);
        }

        private IndexDocumentsResult IndexDataChunkInParts<T>(DataChunk<T> dataChunk, int partsQuantity) where T : class
        {
            var chunkDataParts = dataChunk.GetDataInParts(partsQuantity);

            var documentsIndexed = 0;
            var documentsNotIndexed = 0;

            foreach (var chunkData in chunkDataParts)
            {
                var indexDocumentsResult = _documentsIndexer.IndexDocuments(chunkData, dataChunk.IndexName, dataChunk.TypeName);

                if (indexDocumentsResult.HaveError)
                {
                    documentsNotIndexed += indexDocumentsResult.DocumentsNotIndexed;
                }
                else
                {
                    documentsIndexed += indexDocumentsResult.DocumentsIndexed;
                }
            }

            return new IndexDocumentsResult(documentsIndexed, documentsNotIndexed);
        }
    }
}
