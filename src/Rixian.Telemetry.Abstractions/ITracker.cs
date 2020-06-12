// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Telemetry.Abstractions
{
    using System.Threading.Tasks;
    using Rixian.CloudEvents;

    /// <summary>
    /// Defines an object that tracks events containing data for later consumption.
    /// </summary>
    public interface ITracker
    {
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
