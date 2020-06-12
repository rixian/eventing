// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Rixian.Telemetry;

    /// <summary>
    /// Default implementation of ITrackerBuilder.
    /// </summary>
    internal class InternalTrackerBuilder : ITrackerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalTrackerBuilder"/> class.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        public InternalTrackerBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; set; }
    }
}
