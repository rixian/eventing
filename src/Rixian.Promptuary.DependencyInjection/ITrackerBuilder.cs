// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder for configuring an ITracker.
    /// </summary>
    public interface ITrackerBuilder
    {
        /// <summary>
        /// Gets the ServiceCollection used to configure the ITracker instance.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
