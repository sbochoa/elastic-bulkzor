using System;
using System.Collections.Generic;

namespace Bulkzor.Tests.Fakes
{
    public class FakeSource
        : ISource
    {
        private readonly int _quantity;

        public FakeSource(int quantity)
        {
            _quantity = quantity;
        }

        public IEnumerable<T> GetData<T>() 
            where T : class, IIndexableObject
        {
            for (int i = 0; i < _quantity; i++)
            {
                yield return Activator.CreateInstance<T>();
            }
        }
    }
}
