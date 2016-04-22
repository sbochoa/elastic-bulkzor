using System.Diagnostics;
using Bulkzor.Results;
using Bulkzor.Storage;
using Bulkzor.Types;

namespace Bulkzor.Indexers
{
    public class DataChunkIndexer
        : IIndexDataChunk
    {
        private readonly IIndexDocuments _documentsIndexer;
        private readonly IStoreDocuments _documentsStorage;
        private static readonly int PartsQuantity = 3;

        public DataChunkIndexer(IIndexDocuments documentsIndexer, IStoreDocuments documentsStorage)
        {
            _documentsIndexer = documentsIndexer;
            _documentsStorage = documentsStorage;
        }

        public IndexDataChunkResult IndexDataChunk<T>(DataChunk<T> dataChunk) 
            where T : class
        {
            var indexDocumentsResult = _documentsIndexer.IndexDocuments(dataChunk.Data, dataChunk.IndexName, dataChunk.TypeName);

            if (indexDocumentsResult.HaveError)
            {
                if (indexDocumentsResult.Error == IndexingError.LengthExceeded)
                {
                    indexDocumentsResult = IndexDataChunkInParts(dataChunk, PartsQuantity);
                }
                else 
                {
                    _documentsStorage.StoreDocuments(dataChunk.Data, dataChunk.IndexName, dataChunk.TypeName);
                }    
            }
            return new IndexDataChunkResult(indexDocumentsResult.DocumentsIndexed, indexDocumentsResult.DocumentsNotIndexed);
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
                    _documentsStorage.StoreDocuments(dataChunk.Data, dataChunk.IndexName, dataChunk.TypeName);
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
