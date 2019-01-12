using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Taskboard.Commands.Repositories
{
    public class ListRepository : IListRepository
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;

        public ListRepository(IDocumentClient documentClient, string db,
            string collection)
        {
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public async Task<List> GetById(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, id);

                var document = await documentClient.ReadDocumentAsync<List>(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id)
                });

                return document.Document;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw ResourceNotFoundException.FromResourceId(id);
                }

                throw DataAccessException.FromInnerException(ex);
            }
        }

        public async Task<string> Create(List list)
        {
            try
            {
                var uri = UriFactory.CreateDocumentCollectionUri(db, collection);
                var result = await documentClient.CreateDocumentAsync(uri, list);

                return result.Resource.Id;
            }
            catch (DocumentClientException ex)
            {
                throw DataAccessException.FromInnerException(ex);
            }
        }

        public Task Replace(List list)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, list.Id);

                return documentClient.ReplaceDocumentAsync(uri, list, new RequestOptions
                {
                    PartitionKey = new PartitionKey(list.Id)
                });
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw ResourceNotFoundException.FromResourceId(list.Id);
                }

                throw DataAccessException.FromInnerException(ex);
            }
        }

        public Task Delete(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, id);

                return documentClient.DeleteDocumentAsync(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id)
                });
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw ResourceNotFoundException.FromResourceId(id);
                }

                throw DataAccessException.FromInnerException(ex);
            }
        }
    }
}