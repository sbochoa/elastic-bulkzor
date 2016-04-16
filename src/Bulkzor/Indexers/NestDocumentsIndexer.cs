﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bulkzor.Configuration;
using Bulkzor.Results;
using Bulkzor.Utils;
using Nest;

namespace Bulkzor.Indexers
{
    public class NestDocumentsIndexer
        : IDocumentsIndexer
    {
        private readonly ElasticClient _client;
        public ElasticClient Client => _client;

        public NestDocumentsIndexer(ElasticClient client)
        {
            _client = client;
        }

        public IndexResult Index<T>(IReadOnlyList<T> documents, string indexName, string typeName)
            where T : class 
        {
            var response = _client.IndexMany(documents, indexName, typeName);

            bool errors = !response.ApiCall.Success || response.Errors;

            return new IndexResult(response.Items.Count(), response.ItemsWithErrors.Count(), errors);

            //var documentCount = 0;
            //var totalDocumentsIndexed = 0;
            //var totalDocumentsNotIndexed = 0;

            //var workingDocuments = new List<T>();
            //var chunkWatch = new Stopwatch();
            //var bulkWatch = new Stopwatch();

            //chunkWatch.Start();
            //bulkWatch.Start();

            //foreach (var document in documents)
            //{
            //    if (documentCount >= _chunkConfiguration.GetChunkSize)
            //    {
            //        documentCount = 0;

            //        var result = IndexWorkingResults(indexName, typeName, workingDocuments, chunkWatch);

            //        totalDocumentsIndexed += result.DocumentsIndexed;
            //        totalDocumentsNotIndexed += result.DocumentsNotIndexed;
            //    }

            //    workingDocuments.Add(document);

            //    documentCount++;
            //}

            //if (workingDocuments.Any())
            //{
            //    var lastResult = IndexWorkingResults(indexName, typeName, workingDocuments, chunkWatch);

            //    totalDocumentsIndexed += lastResult.DocumentsIndexed;
            //    totalDocumentsNotIndexed += lastResult.DocumentsNotIndexed;
            //}

            //bulkWatch.Stop();

            //return new IndexResult(totalDocumentsIndexed, totalDocumentsNotIndexed, bulkWatch.Elapsed);
        }

        //private DocumentIndexingResult IndexWorkingResults<T>(string indexName, string typeName, List<T> workingResults, Stopwatch chunkWatch) where T : class
        //{
        //    var result = IndexDocuments(workingResults, indexName, typeName);

        //    chunkWatch.Stop();

        //    _chunkConfiguration.GetOnChunkIndexed?.Invoke(result, indexName, typeName, chunkWatch.Elapsed);

        //    workingResults.Clear();

        //    chunkWatch.Restart();

        //    return result;
        //}

        //public DocumentIndexingResult IndexDocuments<T>(IReadOnlyList<T> documents, string indexName, string typeName)
        //    where T:class 
        //{
        //    var response = _client.IndexMany(documents, indexName, typeName);

        //    if (!response.ApiCall.Success)
        //    {
        //        return HandleFailedApiCall(documents, indexName, typeName);
        //    }

        //    return new DocumentIndexingResult(response.Items.Count(), response.ItemsWithErrors.Count());
        //}

        //private DocumentIndexingResult HandleFailedApiCall<T>(IReadOnlyList<T> documents, string indexName, string typeName)
        //    where T : class
        //{
        //    var tries = 0;

        //    while (tries < _chunkConfiguration.GetChunkRetries)
        //    {
        //        tries++;

        //        if (_chunkConfiguration.GetPartChunkWhenFail)
        //        {
        //            return TryIndexPartingChunks(documents, indexName, typeName);
        //        }

        //        var response = _client.IndexMany(documents, indexName, typeName);

        //        if (response.ApiCall.Success)
        //        {
        //            return new DocumentIndexingResult(response.Items.Count(), response.ItemsWithErrors.Count()); ;
        //        }
        //    }

        //    // to-do write working result part failed in text file maybe?

        //    return new DocumentIndexingResult(0, documents.Count);
        //}

        //private DocumentIndexingResult TryIndexPartingChunks<T>(IReadOnlyList<T> documents, string indexName, string typeName) 
        //    where T : class
        //{
        //    var documentsParts = documents.Split(_chunkConfiguration.GetNumberPartsToDivideWhenChunkFail).ToList();

        //    var documentsNotIndexed = 0;
        //    var documentsIndexed = 0;

        //    foreach (var workingDocumentsPart in documentsParts)
        //    {
        //        var workingDocumentsPartList = workingDocumentsPart.ToList();

        //        var response = _client.IndexMany(workingDocumentsPartList, indexName, typeName);

        //        if (!response.ApiCall.Success)
        //        {
        //            documentsNotIndexed += workingDocumentsPartList.Count;
        //            // to-do write working result part failed in text file maybe?
        //        }
        //        else
        //        {
        //            documentsIndexed += response.Items.Count();
        //        }
        //    }

        //    return new DocumentIndexingResult(documentsIndexed, documentsNotIndexed);
        //}
    }
}
