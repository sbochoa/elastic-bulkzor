using System.Collections.Generic;

namespace Bulkzor
{
    public interface ISource
    {
        IEnumerable<T> GetData<T>() where T : class, IIndexableObject;
    }

    public interface IManagedSource : ISource
    {
        void OpenConnection();
        void CloseConnection();
    }
}
