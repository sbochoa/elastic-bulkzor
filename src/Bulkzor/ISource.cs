using System.Collections.Generic;

namespace Bulkzor
{
    public interface ISource
    {
        IEnumerable<object> GetData();
    }

    public interface IManagedSource : ISource
    {
        void OpenConnection();
        void CloseConnection();
    }
}
