using Bulkzor.Callbacks;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Indexers
{
    public class DataChunkIndexer
        : IIndexDataChunk
    {
        private readonly IIndexDocuments _documentsIndexer;
        private readonly BeforeIndexDataChunk _beforeIndexDataChunk;
        private readonly AfterIndexDataChunk _afterIndexDataChunk;
        private const int PartsQuantity = 3;

        public DataChunkIndexer(IIndexDocuments documentsIndexer, BeforeIndexDataChunk beforeIndexDataChunk, AfterIndexDataChunk afterIndexDataChunk)
        {
            _documentsIndexer = documentsIndexer;
            _beforeIndexDataChunk = beforeIndexDataChunk;
            _afterIndexDataChunk = afterIndexDataChunk;
        }

        public IndexDataChunkResult IndexDataChunk<T>(DataChunk<T> dataChunk) 
            where T : class
        {
            var objectsIndexed = 0;
            var objectsNotIndexed = 0;

            foreach (var indexChunk in dataChunk.Data)
            {
                _beforeIndexDataChunk?.Invoke(_documentsIndexer, indexChunk.IndexName, indexChunk.TypeName);

                var indexDocumentsResult = _documentsIndexer.IndexDocuments(indexChunk.DataIndex, indexChunk.IndexName, indexChunk.TypeName);

                if (indexDocumentsResult.HaveError)
                {
                    if (indexDocumentsResult.Error == IndexingError.LengthExceeded)
                    {
                        indexDocumentsResult = IndexDataChunkInParts(indexChunk, PartsQuantity);
                    }
                    else
                    {
                        // TODO : Store documents not indexed
                    }
                }

                objectsIndexed += indexDocumentsResult.DocumentsIndexed;
                objectsNotIndexed += indexDocumentsResult.DocumentsNotIndexed;

                _afterIndexDataChunk?.Invoke
                    ( _documentsIndexer
                    , new IndexDataChunkResult(indexDocumentsResult.DocumentsIndexed, indexDocumentsResult.DocumentsNotIndexed)
                    , indexChunk.IndexName
                    , indexChunk.TypeName);
            }

            return new IndexDataChunkResult(objectsIndexed, objectsNotIndexed);
        }

        private IndexDocumentsResult IndexDataChunkInParts<T>(DataIndexChunk<T> dataIndexChunk, int partsQuantity) 
            where T : class
        {
            var chunkDataParts = dataIndexChunk.GetDataIndexInParts(partsQuantity);

            var objectsIndexed = 0;
            var objectsNotIndexed = 0;

            foreach (var chunkData in chunkDataParts)
            {
                var indexDocumentsResult = _documentsIndexer.IndexDocuments(chunkData, dataIndexChunk.IndexName, dataIndexChunk.TypeName);

                if (indexDocumentsResult.HaveError)
                {
                    objectsNotIndexed += indexDocumentsResult.DocumentsNotIndexed;
                }
                else
                {
                    objectsIndexed += indexDocumentsResult.DocumentsIndexed;
                }
            }

            return new IndexDocumentsResult(objectsIndexed, objectsNotIndexed);
        }
    }
}
