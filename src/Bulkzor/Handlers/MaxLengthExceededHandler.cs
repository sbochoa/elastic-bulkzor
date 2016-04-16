using System;
using System.Linq;
using Bulkzor.Events;
using Bulkzor.Indexers;
using Bulkzor.Results;
using Bulkzor.Utils;

namespace Bulkzor.Handlers
{
    public class MaxLengthExceededHandler<TDocument>
        : IHandle<MaxLengthExceeded<TDocument>, DocumentIndexingResult>
        where TDocument:class
    {
        private readonly IDocumentsIndexer _documentsIndexer;
        private readonly int _numberPartsToDivide;

        public MaxLengthExceededHandler(IDocumentsIndexer documentsIndexer, int numberPartsToDivide)
        {
            _documentsIndexer = documentsIndexer;
            _numberPartsToDivide = numberPartsToDivide;
        }

        public DocumentIndexingResult Handle(MaxLengthExceeded<TDocument> message)
        {
            var documentsParts = message.Documents.Split(_numberPartsToDivide).ToList();

            var documentsNotIndexed = 0;
            var documentsIndexed = 0;

            foreach (var workingDocumentsPart in documentsParts)
            {
                var response = _documentsIndexer.Index(workingDocumentsPart, message.IndexName, message.TypeName);

                if (!response.Errors)
                {
                    documentsNotIndexed += response.TotalDocumentsIndexed;
                    // to-do write working result part failed in text file maybe?
                }
                else
                {
                    documentsIndexed += response.TotalDocumentsIndexed;
                }
            }

            return new DocumentIndexingResult(documentsIndexed, documentsNotIndexed);
        }
    }
}
