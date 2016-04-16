using System.Collections.Generic;
using System.Diagnostics;
using Bulkzor.Commands;
using Bulkzor.Results;
using Bulkzor.Types;

namespace Bulkzor.Handlers
{
    public class BulkDocumentsHandler<TDocument>
        : IHandle<BulkDocuments<TDocument>, IndexResult>
        where TDocument : class 
    {
        private readonly IHandle<IndexChunk<TDocument>, IndexResult> _indexDocumentsChunkHandler;

        public BulkDocumentsHandler
            (IHandle<IndexChunk<TDocument>, IndexResult> indexDocumentsChunkHandler)
        {
            _indexDocumentsChunkHandler = indexDocumentsChunkHandler;
        }

        public IndexResult Handle(BulkDocuments<TDocument> message)
        {
            var chunkConfiguration = message.ChunkConfiguration;
            var documents = message.Documents;

            var documentsCount = 0;
            var documentsIndexed = 0;
            var documentsNotIndexed = 0;
            var documentsChunk = new List<TDocument>();

            var watch = new Stopwatch();

            watch.Start();

            foreach (var document in documents)
            {
                if (documentsCount >= chunkConfiguration.GetChunkSize)
                {
                    IndexChunk(message, documentsChunk, ref documentsIndexed, ref documentsNotIndexed);
                }

                documentsChunk.Add(document);
                documentsCount++;
            }

            IndexChunk(message, documentsChunk, ref documentsIndexed, ref documentsNotIndexed);

            watch.Stop();

            return new IndexResult(documentsIndexed, documentsNotIndexed, watch.Elapsed);
        }

        private void IndexChunk(BulkDocuments<TDocument> message, List<TDocument> documentsChunk,ref int documentsIndexed, ref int documentsNotIndexed)
        {
            var result =
                _indexDocumentsChunkHandler
                    .Handle(new IndexChunk<TDocument>(documentsChunk, message.IndexName, message.TypeName,
                        message.ChunkConfiguration));

            documentsIndexed += result.DocumentsIndexed;
            documentsNotIndexed += result.DocumentsNotIndexed;

            documentsChunk.Clear();
        }
    }
}
