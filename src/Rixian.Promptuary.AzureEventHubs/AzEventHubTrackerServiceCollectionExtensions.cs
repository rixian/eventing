// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Promptuary;
    using Rixian.Promptuary.AzureEventHubs;

    /// <summary>
    /// Extensions for configuring the AzureEventHubTracker.
    /// </summary>
    public static class AzEventHubTrackerServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the AzureEventHubTracker.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <param name="eventHubConnectionString">The Event Hub connection string. Must include the name of the Event Hub.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithAzureEventHubSink(this ITrackerBuilder trackerBuilder, string eventHubConnectionString)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder.Services.Configure<AzureEventHubTrackerConfig>(o =>
            {
                o.EventHubConnectionString = eventHubConnectionString;
            });
            trackerBuilder.Services.AddScoped<ITracker, AzureEventHubTracker>();

            return trackerBuilder;
        }
    }
}
