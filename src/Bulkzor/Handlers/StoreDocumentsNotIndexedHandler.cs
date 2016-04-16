using System;
using Bulkzor.Commands;
using Bulkzor.Results;

namespace Bulkzor.Handlers
{
    public class StoreDocumentsNotIndexedHandler<TDocument>
        : IHandle<StoreDocumentsNotIndexed<TDocument>, IndexResult>
        where TDocument : class
    {
        public IndexResult Handle(StoreDocumentsNotIndexed<TDocument> message)
        {
            //TODO: store documents not indexed
            throw new NotImplementedException();
        }
    }
}
