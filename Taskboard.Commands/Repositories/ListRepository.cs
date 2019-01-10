using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Optional;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Repositories
{
    public class ListRepository : IListRepository
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;
        private readonly TelemetryClient telemetryClient;

        public ListRepository(TelemetryClient telemetryClient, IDocumentClient documentClient, string db,
            string collection)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public async Task<Option<string, CosmosFailure>> Create(List list)
        {
            try
            {
                var uri = UriFactory.CreateDocumentCollectionUri(db, collection);
                var result = await documentClient.CreateDocumentAsync(uri, list);

                return Option.Some<string, CosmosFailure>(result.Resource.Id);
            }
            catch (DocumentClientException ex)
            {
                telemetryClient.TrackException(ex);

                return Option.None<string, CosmosFailure>(CosmosFailure.Error);
            }
        }

        public async Task<Option<CosmosFailure>> Delete(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, id);

                await documentClient.DeleteDocumentAsync(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id)
                });

                return Option.None<CosmosFailure>();
            }
            catch (DocumentClientException ex)
            {
                telemetryClient.TrackException(ex);

                return Option.Some(CosmosFailure.Error);
            }
        }
    }
}