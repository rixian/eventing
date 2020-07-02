// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Threading.Channels;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;
    using Rixian.Eventing.AspNetCore;

    /// <summary>
    /// Extensions for configuring the ITracker.
    /// </summary>
    public static class TrackerServiceCollectionExtensions
    {
        /// <summary>
        /// Registers IHttpTracker.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithHttpTracker(this ITrackerBuilder trackerBuilder)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder.Services.AddScoped<ITracker, AspNetCoreTracker>();
            trackerBuilder.Services.AddScoped<IHttpTracker, DefaultHttpTracker>();
            var channel = Channel.CreateUnbounded<TrackerSession>(new UnboundedChannelOptions { SingleReader = true });
            trackerBuilder.Services.AddSingleton(channel.Reader);
            trackerBuilder.Services.AddSingleton(channel.Writer);
            trackerBuilder.Services.AddHostedService<TrackingFlushService>();

            return trackerBuilder;
        }
    }
}
