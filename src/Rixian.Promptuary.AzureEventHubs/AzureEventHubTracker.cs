// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary.AzureEventHubs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.Retry;
    using Rixian.Promptuary;

    /// <summary>
    /// Tracker that writes to Azure Event Hubs.
    /// </summary>
    public class AzureEventHubTracker : BaseTracker
    {
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly IOptions<AzureEventHubTrackerConfig> options;
        private readonly ITrackerProperties? trackerProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventHubTracker"/> class.
        /// </summary>
        /// <param name="options">The configuration options for the AzureEventHubTracker.</param>
        /// <param name="trackerProperties">Extra properties to include in the tracked data.</param>
        public AzureEventHubTracker(IOptions<AzureEventHubTrackerConfig> options, ITrackerProperties? trackerProperties = null)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options;
            this.trackerProperties = trackerProperties;
            this.retryPolicy = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <inheritdoc/>
        public override async Task FlushAsync()
        {
            await using var producerClient = new EventHubProducerClient(this.options.Value.EventHubConnectionString);
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync().ConfigureAwait(false);

            foreach (var value in this.TrackedValues)
            {
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventObject()
                {
                    Data = value,
                    Properties = this.trackerProperties?.Properties,
                }))));
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

        /// <summary>
        /// Models an event in Event Hub.
        /// </summary>
        internal class EventObject
        {
            /// <summary>
            /// Gets or sets the event data.
            /// </summary>
            [System.Text.Json.Serialization.JsonPropertyName("data")]
            public object? Data { get; set; }

            /// <summary>
            /// Gets or sets the extra event properties.
            /// </summary>
            [System.Text.Json.Serialization.JsonExtensionData]
            public IDictionary<string, object>? Properties { get; set; }
        }
    }
}
