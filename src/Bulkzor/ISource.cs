using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Bulkzor
{
    public interface ISource<out T>
        where T:class
    {
        IEnumerable<T> GetData();
    }
}
