using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bulkzor.Types;

namespace Bulkzor.Results
{
    public class IndexObjectsResult<T>
        where T : class, IIndexableObject
    {
        public IndexObjectsResult(IReadOnlyList<T> objectsIndexed, IReadOnlyList<T> objectsNotIndexed, IndexingError error)
            : this(objectsIndexed, objectsNotIndexed)
        {
            Error = error;
        }

        public IndexObjectsResult(IReadOnlyList<T> objectsIndexed, IReadOnlyList<T> objectsNotIndexed)
        {
            ObjectsIndexed = objectsIndexed;
            ObjectsNotIndexed = objectsNotIndexed;
            Error = ObjectsNotIndexed.Count > 0 ? IndexingError.OnlyPartOfDocumentsIndexed : IndexingError.None;
        }

        public IReadOnlyList<T> ObjectsIndexed { get; }
        public IReadOnlyList<T> ObjectsNotIndexed { get; }
        public IndexingError Error { get; }

        public bool HaveError => Error != IndexingError.None;
    }
}
