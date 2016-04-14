using System;

namespace Bulkzor
{
    public class BulkTaskConfiguration<T>
        where T : class
    {
        private object _source;
        private string _typeName;
        private string _indexName;
        private Uri[] _nodes;
        private int _bulkSize;
        private BeforeBulkTaskRun _beforeBulkTaskRun;
        private AfterBulkTaskRun _afterBulkTaskRun;
        private OnBulkTaskError _onBulkTaskError;
        private OnBulkIndexed _onBulkIndexed;

        internal object GetSource => _source;
        internal string GetTypeName => _typeName;
        internal string GetIndexName => _indexName;
        internal Uri[] GetNodes => _nodes;
        internal AfterBulkTaskRun GetAfterBulkTaskRun => _afterBulkTaskRun;
        internal BeforeBulkTaskRun GetBeforeBulkTaskRun => _beforeBulkTaskRun;
        internal int GetBulkSize => _bulkSize;
        internal OnBulkTaskError GetOnBulkTaskError => _onBulkTaskError;
        internal OnBulkIndexed GetOnBulkIndexed => _onBulkIndexed;
        
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

        public void BulkSize(int bulkSize)
        {
            _bulkSize = bulkSize;
        }

        public void BeforeBulkTaskRun(BeforeBulkTaskRun beforeBulkTaskRun)
        {
            _beforeBulkTaskRun = beforeBulkTaskRun;
        }

        public void AfterBulkTaskRun(AfterBulkTaskRun afterBulkTaskRun)
        {
            _afterBulkTaskRun = afterBulkTaskRun;
        }

        public void OnBulkTaskError(OnBulkTaskError onBulkTaskError)
        {
            _onBulkTaskError = onBulkTaskError;
        }

        public void OnBulkIndexed(OnBulkIndexed onBulkIndexed)
        {
            _onBulkIndexed = onBulkIndexed;
        }
    }
}
