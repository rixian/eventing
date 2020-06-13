// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rixian.CloudEvents;

    /// <summary>
    /// Interface the represents a sink for writing events.
    /// </summary>
    public interface ITrackerSink
    {
        /// <summary>
        /// Flushes events out to the underlying store.
        /// </summary>
        /// <param name="events">The list of events to write.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task FlushAsync(IEnumerable<CloudEvent> events);
    }
}
