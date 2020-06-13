// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Configuration for an event tracker.
    /// </summary>
    public class TrackerOptions
    {
        /// <summary>
        /// Gets a dictionary of fixed tags that are applied to all events.
        /// </summary>
        public IDictionary<string, object?> GlobalTags { get; } = new Dictionary<string, object?>();

        /// <summary>
        /// Gets a dictionary of delegates that generate tags that are applied to all events within a single scope.
        /// </summary>
        public IDictionary<string, Func<IServiceProvider, object?>> ScopedTags { get; } = new Dictionary<string, Func<IServiceProvider, object?>>();
    }
}
