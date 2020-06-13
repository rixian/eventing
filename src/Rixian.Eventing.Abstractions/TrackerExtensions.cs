// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Abstractions
{
    using System;

    /// <summary>
    /// Extension methods for working with an event tracker.
    /// </summary>
    public static class TrackerExtensions
    {
        /// <summary>
        /// Provides a temporary tag that is removed when the IDisposable instance is disposed.
        /// </summary>
        /// <param name="tracker">The ITracker instance.</param>
        /// <param name="key">The tag key.</param>
        /// <param name="value">The tag value.</param>
        /// <returns>An IDisposable that will remove the tag from the tracker when disposed.</returns>
        public static IDisposable WithTag(this ITracker tracker, string key, object? value)
        {
            if (tracker is null)
            {
                throw new ArgumentNullException(nameof(tracker));
            }

            tracker.Tags[key] = value;

            return new ActionDisposable(() => tracker.Tags.Remove(key));
        }
    }
}
