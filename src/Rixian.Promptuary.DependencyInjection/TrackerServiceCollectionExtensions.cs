// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Promptuary;

    /// <summary>
    /// Extensions for registering ITracker with the ServiceCollection.
    /// </summary>
    public static class TrackerServiceCollectionExtensions
    {
        /// <summary>
        /// Registers ITracker with the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The ITrackerBuilder instance.</returns>
        public static ITrackerBuilder AddTracking(this IServiceCollection services)
        {
            services.AddScoped<ITrackerProperties, DefaultTrackerProperties>();
            return new InternalTrackerBuilder(services);
        }
    }
}
