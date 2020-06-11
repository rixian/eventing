// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary
{
    using System.Collections.Generic;

    /// <summary>
    /// Default implementation of ITrackerProperties.
    /// </summary>
    public class DefaultTrackerProperties : ITrackerProperties
    {
        /// <inheritdoc/>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }
}
