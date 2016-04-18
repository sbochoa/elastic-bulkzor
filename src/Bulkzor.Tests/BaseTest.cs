using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Bulkzor.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected string _indexName = "documents_Index";
        protected string _typeName = "documents";

        protected IEnumerable<Document> GetFakeDocumentsList(int quantity = 50)
        {
            for (var i = 0; i < quantity; i++)
            {
                yield return new Document
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString()
                };
            }
        } 
    }

    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
