using System.Collections.Generic;
using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexObjectsResult
    {
        public IndexObjectsResult(IReadOnlyList<object> objectsIndexed, IReadOnlyList<object> objectsNotIndexed, IndexingError error, IReadOnlyList<object> objectsNotIndexedStore = null)
            : this(objectsIndexed, objectsNotIndexed, objectsNotIndexedStore)
        {
            Error = error;
        }

        public IndexObjectsResult(IReadOnlyList<object> objectsIndexed, IReadOnlyList<object> objectsNotIndexed, IReadOnlyList<object> objectsNotIndexedStored = null)
        {
            ObjectsIndexed = objectsIndexed;
            ObjectsNotIndexed = objectsNotIndexed;
            ObjectsNotIndexedStored = objectsNotIndexedStored ?? new List<object>();
            Error = ObjectsNotIndexed.Count > 0 ? IndexingError.OnlyPartOfDocumentsIndexed : IndexingError.None;
        }

        public IReadOnlyList<object> ObjectsIndexed { get; }
        public IReadOnlyList<object> ObjectsNotIndexed { get; }
        public IReadOnlyList<object> ObjectsNotIndexedStored { get; }
        public IndexingError Error { get; }

        public bool HaveError => Error != IndexingError.None;
    }
}
