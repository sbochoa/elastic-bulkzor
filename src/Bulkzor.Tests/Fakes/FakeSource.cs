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

        public IEnumerable<object> GetData()
        {
            for (var i = 0; i < _quantity; i++)
            {
                yield return Activator.CreateInstance<Person>();
            }
        }
    }
}
