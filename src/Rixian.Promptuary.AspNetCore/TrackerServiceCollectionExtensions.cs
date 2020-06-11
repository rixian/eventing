﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Promptuary.AspNetCore;

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
        public static ITrackerBuilder WithHttpProperties(this ITrackerBuilder trackerBuilder)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder.Services.AddScoped<IHttpTracker, DefaultHttpTracker>();

            return trackerBuilder;
        }
    }
}
