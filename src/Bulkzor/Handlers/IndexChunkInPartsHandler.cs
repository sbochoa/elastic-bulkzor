using System;
using System.Diagnostics;
using System.Linq;
using Bulkzor.Commands;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Types;
using Bulkzor.Utils;

namespace Bulkzor.Handlers
{
    public class IndexChunkInPartsHandler<TDocument>
        : IHandle<IndexChunkInParts<TDocument>, IndexResult>
        where TDocument:class
    {
        private readonly IDocumentsIndexer _documentsIndexer;

        public IndexChunkInPartsHandler(IDocumentsIndexer documentsIndexer)
        {
            _documentsIndexer = documentsIndexer;
        }

        public IndexResult Handle(IndexChunkInParts<TDocument> message)
        {
            var chunkParts = message.Chunk.Split(message.NumberParts).ToList();

            var documentsIndexed = 0;
            var documentsNotIndexed = 0;

            foreach (var chunkPart in chunkParts)
            {
                var result = _documentsIndexer.Index(chunkPart, message.IndexName, message.TypeName);

                if (result.Error != IndexingError.None)
                {
                    documentsNotIndexed += result.DocumentsNotIndexed;
                }
                else
                {
                    documentsIndexed += result.DocumentsIndexed;
                }
            }

            return new IndexResult(documentsIndexed, documentsNotIndexed, TimeSpan.Zero);
        }
    }
}
