using System;

namespace Bulkzor
{
    public class BulkzorConfiguration<T>
        where T : class
    {
        private object _source;
        private string _typeName;
        private string _indexName;
        private Uri[] _nodes;

        internal object GetSource => _source;
        internal string GetTypeName => _typeName;
        internal string GetIndexName => _indexName;
        internal Uri[] GetNodes => _nodes;

        public BulkzorConfiguration()
        {
            
        }

        public void Nodes(params Uri[] nodes)
        {
            _nodes = nodes;
        }
        public void Source(ISource<T> source)
        {
            _source = source;
        }

        public void TypeName(string typeName)
        {
            _typeName = typeName;
        }

        public void IndexName(string indexName)
        {
            _indexName = indexName;
        }
    }
}
