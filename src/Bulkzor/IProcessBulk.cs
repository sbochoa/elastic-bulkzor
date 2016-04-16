using System.Collections.Generic;

namespace Bulkzor
{
    public interface IProcessBulk
    {
        void Process<T>(IEnumerable<T> data, string indexName, string typeName);
    }
}
