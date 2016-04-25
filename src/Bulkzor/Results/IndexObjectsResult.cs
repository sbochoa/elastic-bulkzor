using System.Collections.Generic;
using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexObjectsResult<T>
        where T : class, IIndexableObject
    {
        public IndexObjectsResult(IReadOnlyList<T> objectsIndexed, IReadOnlyList<T> objectsNotIndexed, IndexingError error, IReadOnlyList<T> objectsNotIndexedStore = null)
            : this(objectsIndexed, objectsNotIndexed, objectsNotIndexedStore)
        {
            Error = error;
        }

        public IndexObjectsResult(IReadOnlyList<T> objectsIndexed, IReadOnlyList<T> objectsNotIndexed, IReadOnlyList<T> objectsNotIndexedStored = null)
        {
            ObjectsIndexed = objectsIndexed;
            ObjectsNotIndexed = objectsNotIndexed;
            ObjectsNotIndexedStored = objectsNotIndexedStored ?? new List<T>();
            Error = ObjectsNotIndexed.Count > 0 ? IndexingError.OnlyPartOfDocumentsIndexed : IndexingError.None;
        }

        public IReadOnlyList<T> ObjectsIndexed { get; }
        public IReadOnlyList<T> ObjectsNotIndexed { get; }
        public IReadOnlyList<T> ObjectsNotIndexedStored { get; }
        public IndexingError Error { get; }

        public bool HaveError => Error != IndexingError.None;
    }
}
