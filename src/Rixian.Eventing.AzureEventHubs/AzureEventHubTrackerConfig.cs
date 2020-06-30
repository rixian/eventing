// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Sinks.AzureEventHubs
{
    using System;
    using Rixian.CloudEvents;

    /// <summary>
    /// Configuration for the AzureEventHubTracker class.
    /// </summary>
    public class AzureEventHubTrackerConfig
    {
        /// <summary>
        /// Gets or sets the Event Hub connection string. Must include the name of the Event Hub.
        /// </summary>
        public string? EventHubConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a delegate for previewing cloud events.
        /// </summary>
        public Action<CloudEvent?>? PreviewCloudEvents { get; set; }
    }
}
