// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Telemetry
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Rixian.CloudEvents;
    using Rixian.Telemetry.Abstractions;

    /// <summary>
    /// Shared base tracker.
    /// </summary>
    public abstract class BaseTracker : ITracker
    {
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
        public abstract Task FlushAsync();
    }
}
