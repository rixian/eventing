// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Telemetry.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines extra properties that can be included with tracked data.
    /// </summary>
    public interface ITrackerTagger
    {
        /// <summary>
        /// Gets the collection of properties to include with all tracked data.
        /// </summary>
        IDictionary<string, object> Tags { get; }
    }
}
