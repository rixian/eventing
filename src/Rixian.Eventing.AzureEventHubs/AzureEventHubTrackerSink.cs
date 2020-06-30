// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Sinks.AzureEventHubs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polly;
    using Polly.Retry;
    using Rixian.CloudEvents;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Tracker that writes to Azure Event Hubs.
    /// </summary>
    public partial class AzureEventHubTrackerSink : ITrackerSink
    {
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly IOptions<AzureEventHubTrackerConfig> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventHubTrackerSink"/> class.
        /// </summary>
        /// <param name="options">The configuration options for the AzureEventHubTracker.</param>
        public AzureEventHubTrackerSink(IOptions<AzureEventHubTrackerConfig> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options;
            this.retryPolicy = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <inheritdoc/>
        public async Task FlushAsync(IEnumerable<CloudEvent> events)
        {
            if (events is null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            await using var producerClient = new EventHubProducerClient(this.options.Value.EventHubConnectionString);
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync().ConfigureAwait(false);

            Action<CloudEvent?>? previewCloudEvents = this.options?.Value?.PreviewCloudEvents;
            if (previewCloudEvents is object)
            {
                foreach (CloudEvent? value in events)
                {
                    previewCloudEvents.Invoke(value);
                }
            }

            foreach (CloudEvent? value in events)
            {
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))));
            }

            // When the producer sends the event, it will receive an acknowledgment from the Event Hubs service; so
            // long as there is no exception thrown by this call, the service is now responsible for delivery.  Your
            // event data will be published to one of the Event Hub partitions, though there may be a (very) slight
            // delay until it is available to be consumed.
            await this.retryPolicy.ExecuteAsync(async () =>
            {
                await producerClient.SendAsync(eventBatch).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
