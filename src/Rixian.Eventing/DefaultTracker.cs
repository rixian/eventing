﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json.Linq;
    using Rixian.CloudEvents;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Shared base tracker.
    /// </summary>
    public class DefaultTracker : ITracker
    {
        private readonly IEnumerable<ITrackerSink> sinks;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTracker"/> class.
        /// </summary>
        /// <param name="serviceProvider">The IServiceProvider used for scoped tags.</param>
        /// <param name="options">Options for this tracker instance.</param>
        /// <param name="sinks">The sinks for writing event data.</param>
        public DefaultTracker(IServiceProvider serviceProvider, IOptions<TrackerOptions> options, IEnumerable<ITrackerSink>? sinks = null)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (options?.Value?.GlobalTags is object)
            {
                foreach (KeyValuePair<string, object?> tag in options.Value.GlobalTags)
                {
                    this.Tags[tag.Key] = tag.Value;
                }
            }

            if (options?.Value?.ScopedTags is object)
            {
                foreach (KeyValuePair<string, Func<IServiceProvider, object?>> tag in options.Value.ScopedTags)
                {
                    this.Tags[tag.Key] = tag.Value?.Invoke(serviceProvider);
                }
            }

            this.sinks = sinks ?? new List<ITrackerSink>();
        }

        /// <inheritdoc/>
        public IDictionary<string, object?> Tags { get; } = new Dictionary<string, object?>();

        /// <summary>
        /// Gets or sets the values that are curently tracked.
        /// </summary>
        protected ImmutableList<CloudEvent> TrackedValues { get; set; } = ImmutableList.Create<CloudEvent>();

        /// <inheritdoc/>
        public void Track(CloudEvent value)
        {
            this.TrackedValues = this.TrackedValues.Add(value);
        }

        /// <inheritdoc/>
        public async Task FlushAsync()
        {
            IEnumerable<CloudEvent>? values = this.TrackedValues.Select(value =>
            {
                if (this.Tags is object)
                {
                    if (value.ExtensionAttributes is null)
                    {
                        value.ExtensionAttributes = new Dictionary<string, JToken>();
                    }

                    foreach (KeyValuePair<string, object?> tag in this.Tags)
                    {
                        value.ExtensionAttributes[tag.Key] = JToken.FromObject(tag.Value);
                    }
                }

                return value;
            });

            this.TrackedValues = this.TrackedValues.Clear();

            foreach (ITrackerSink? sink in this.sinks)
            {
                try
                {
                    await sink.FlushAsync(values).ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // Eat the exception. These should be caught by the sink.
                }
            }
        }
    }
}
