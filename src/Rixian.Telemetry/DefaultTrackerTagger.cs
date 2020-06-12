// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Telemetry
{
    using System.Collections.Generic;
    using Rixian.Telemetry.Abstractions;

    /// <summary>
    /// Default implementation of ITrackerProperties.
    /// </summary>
    public class DefaultTrackerTagger : ITrackerTagger
    {
        /// <inheritdoc/>
        public IDictionary<string, object> Tags { get; } = new Dictionary<string, object>();
    }
}
