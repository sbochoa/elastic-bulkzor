using System;

namespace Bulkzor.Tests.Fakes
{
    public class Person
        : IIndexableObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Person()
        {
            Id = Guid.NewGuid().ToString();
            Name = "Fake Name";
        }
    }
}