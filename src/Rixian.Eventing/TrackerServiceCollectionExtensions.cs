// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;

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
            services.AddScoped<ITracker, DefaultTracker>();
            return new InternalTrackerBuilder(services);
        }

        /// <summary>
        /// Registers ITracker with the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="serviceName">A unique identifier for this service.</param>
        /// <returns>The ITrackerBuilder instance.</returns>
        public static ITrackerBuilder AddTracking(this IServiceCollection services, string serviceName)
        {
            return services.AddTracking()
                .WithGlobalTag("host.service", serviceName);
        }

        /// <summary>
        /// Attaches a tag to be included with all events.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <param name="key">The tag key name.</param>
        /// <param name="value">The  tag value.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithGlobalTag(this ITrackerBuilder trackerBuilder, string key, object? value)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            if (key is null)
            {
                throw new System.ArgumentNullException(nameof(key));
            }

            trackerBuilder.Services.Configure<TrackerOptions>(o =>
            {
                o.GlobalTags[key] = value;
            });

            return trackerBuilder;
        }

        /// <summary>
        /// Attaches a tag to be included with all events.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <param name="key">The tag key name.</param>
        /// <param name="getValue">Delegate to fetch a current tag value.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithScopedTag(this ITrackerBuilder trackerBuilder, string key, Func<IServiceProvider, object?> getValue)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            if (key is null)
            {
                throw new System.ArgumentNullException(nameof(key));
            }

            trackerBuilder.Services.Configure<TrackerOptions>(o =>
            {
                o.ScopedTags[key] = getValue;
            });

            return trackerBuilder;
        }

        /// <summary>
        /// Attaches a tag to be included with all events.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithHostInfo(this ITrackerBuilder trackerBuilder)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder = trackerBuilder
                .WithGlobalTag("host.framework", RuntimeInformation.FrameworkDescription)
                .WithGlobalTag("host.os", RuntimeInformation.OSDescription)
                .WithGlobalTag("host.running_in_container", Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is object ? true : false)
                .WithGlobalTag("host.framework", RuntimeInformation.FrameworkDescription);

            if (RuntimeInformation.OSDescription.StartsWith("Linux", StringComparison.OrdinalIgnoreCase) && Directory.Exists("/sys/fs/cgroup/memory"))
            {
                trackerBuilder = trackerBuilder.WithScopedTag("host.cgroup_memoryusage", svc => System.IO.File.ReadAllLines("/sys/fs/cgroup/memory/memory.usage_in_bytes")[0]);
            }

            // See: http://csharphelper.com/blog/2015/11/get-the-programs-memory-usage-in-c/
            trackerBuilder.Services.AddScoped<Process>(svc => Process.GetCurrentProcess());
            trackerBuilder.WithScopedTag("host.memory.current", svc => svc.GetRequiredService<Process>().WorkingSet64);
            trackerBuilder.WithScopedTag("host.memory.max", svc => svc.GetRequiredService<Process>().MaxWorkingSet.ToInt64());

            return trackerBuilder;
        }
    }
}
