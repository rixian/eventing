// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Telemetry.AspNetCore
{
    using System;
    using Rixian.Telemetry;
    using Rixian.Telemetry.Abstractions;

    /// <summary>
    /// Tracks HTTP specific events.
    /// </summary>
    public interface IHttpTracker : ITracker
    {
        /// <summary>
        /// Tracks a generic CloudEvent.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="source">The event source.</param>
        /// <param name="payload">The JSON-serializable payload.</param>
        void TrackCloudEvent(string eventType, Uri source, object payload);

        /// <summary>
        /// Tracks a single api invocation.
        /// </summary>
        /// <param name="operationName">The name of the api operation.</param>
        /// <param name="source">The event source.</param>
        void TrackApiInvocation(string operationName, Uri source);

        /// <summary>
        /// Tracks a single page view.
        /// </summary>
        /// <param name="pageName">The name of the viewed page.</param>
        /// <param name="source">The event source.</param>
        void TrackPageView(string pageName, Uri source);
    }
}
