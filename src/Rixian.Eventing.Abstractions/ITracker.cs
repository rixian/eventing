// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Abstractions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rixian.CloudEvents;

    /// <summary>
    /// Defines an object that tracks events containing data for later consumption.
    /// </summary>
    public interface ITracker
    {
        /// <summary>
        /// Gets the collection of properties to include with all tracked data.
        /// </summary>
        IDictionary<string, object?> Tags { get; }

        /// <summary>
        /// Flushed the currently tracked events to the underlying store.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task FlushAsync();

        /// <summary>
        /// Tracks a particular event.
        /// </summary>
        /// <param name="value">The event to track.</param>
        void Track(CloudEvent value);
    }
}
