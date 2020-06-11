// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an object that tracks objects containing data for later consumption.
    /// </summary>
    public interface ITracker
    {
        /// <summary>
        /// Flushed the currently tracked objects to the underlying store.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task FlushAsync();

        /// <summary>
        /// Tracks a particular object.
        /// </summary>
        /// <param name="value">The object to track.</param>
        void Track(object value);
    }
}
