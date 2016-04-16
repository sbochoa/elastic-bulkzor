using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bulkzor.Commands;
using Bulkzor.Configuration;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Handlers
{
    public class IndexChunkHandler<TDocument>
        : IHandle<IndexChunk<TDocument>,IndexResult>
        where TDocument:class 
    {
        private readonly IDocumentsIndexer _documentsIndexer;
        private readonly IHandle<IndexChunkInParts<TDocument>, IndexResult> _indexDocumentsChunkInPartsHandler;

        public IndexChunkHandler
            (IDocumentsIndexer documentsIndexer
            , IHandle<IndexChunkInParts<TDocument>, IndexResult> indexDocumentsChunkInPartsHandler)
        {
            _documentsIndexer = documentsIndexer;
            _indexDocumentsChunkInPartsHandler = indexDocumentsChunkInPartsHandler;
        }

        public IndexResult Handle(IndexChunk<TDocument> message)
        {
            var watch = new Stopwatch();

            watch.Start();

            var indexResult = _documentsIndexer.Index(message.Chunk, message.IndexName, message.TypeName);

            if (indexResult.Error != IndexingError.None)
            {
                indexResult = HandleError(indexResult.Error, message.Chunk, message.IndexName, message.TypeName, message.ChunkConfiguration);
            }

            watch.Stop();

            var result = new IndexResult(indexResult.DocumentsIndexed, indexResult.DocumentsNotIndexed, watch.Elapsed, indexResult.Error);

            message.ChunkConfiguration.GetOnChunkIndexed?.Invoke(result, message.IndexName, message.TypeName);

            return result;
        }

        private IndexResult HandleError
            (IndexingError error, IReadOnlyList<TDocument> documentsChunk, string indexName
            , string typeName, ChunkConfiguration chunkConfiguration)
        {
            switch (error)
            {
                case IndexingError.LengthExceeded:
                    return _indexDocumentsChunkInPartsHandler
                                .Handle(new IndexChunkInParts<TDocument>
                                            (documentsChunk, indexName, typeName, chunkConfiguration.GetNumberPartsToDivideWhenChunkFail));
                case IndexingError.Unknow:
                    //TODO: unknow error handler 
                    return null;
                case IndexingError.OnlyPartOfDocumentsIndexed:
                    //TODO: only part of documents handler
                    return null;
            }

            throw new ArgumentException(nameof(error));
        }
    }
}
