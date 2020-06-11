// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    /// <summary>
    /// Shared base tracker.
    /// </summary>
    public abstract class BaseTracker : ITracker
    {
        /// <summary>
        /// Gets or sets the values that are curently tracked.
        /// </summary>
        protected ImmutableList<object> TrackedValues { get; set; } = ImmutableList.Create<object>();

        /// <inheritdoc/>
        public void Track(object value)
        {
            this.TrackedValues = this.TrackedValues.Add(value);
        }

        /// <inheritdoc/>
        public abstract Task FlushAsync();
    }
}
