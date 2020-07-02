// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing
{
    using System.Collections.Generic;
    using Rixian.CloudEvents;

    /// <summary>
    /// Session data for sending tacker messages.
    /// </summary>
    public class TrackerSession
    {
        /// <summary>
        /// Gets or sets a list of cloud events.
        /// </summary>
        public IEnumerable<CloudEvent>? Events { get; set; }
    }
}
