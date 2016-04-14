using System.Collections.Generic;

namespace Bulkzor
{
    public interface ISource<out T>
        where T:class
    {
        IEnumerable<T> GetData();
    }

    public interface IManagedSource<out T> : ISource<T>
        where T:class
    {
        void OpenConnection();
        void CloseConnection();
    }
}
