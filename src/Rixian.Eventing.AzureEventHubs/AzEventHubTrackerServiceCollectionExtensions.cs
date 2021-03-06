﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Rixian.CloudEvents;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;
    using Rixian.Eventing.Sinks.AzureEventHubs;

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
            trackerBuilder.Services.AddScoped<ITrackerSink, AzureEventHubTrackerSink>();

            return trackerBuilder;
        }

        /// <summary>
        /// Configures the AzureEventHubTracker.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <param name="eventHubConnectionString">The Event Hub connection string. Must include the name of the Event Hub.</param>
        /// <param name="previewCloudEvents">Delegate for previewing Cloud Events.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithAzureEventHubSink(this ITrackerBuilder trackerBuilder, string eventHubConnectionString, Action<CloudEvent?> previewCloudEvents)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder.Services.Configure<AzureEventHubTrackerConfig>(o =>
            {
                o.EventHubConnectionString = eventHubConnectionString;
                o.PreviewCloudEvents = previewCloudEvents;
            });
            trackerBuilder.Services.AddScoped<ITrackerSink, AzureEventHubTrackerSink>();

            return trackerBuilder;
        }
    }
}
