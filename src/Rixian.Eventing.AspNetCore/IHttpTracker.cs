// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.AspNetCore
{
    using System;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Tracks HTTP specific events.
    /// </summary>
    public interface IHttpTracker : ITracker
    {
        /// <summary>
        /// Tracks a generic CloudEvent.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="payload">The JSON-serializable payload.</param>
        void TrackCloudEvent(string eventType, object payload);

        /// <summary>
        /// Tracks a single api invocation.
        /// </summary>
        /// <param name="operationName">The name of the api operation.</param>
        void TrackApiInvocation(string operationName);

        /// <summary>
        /// Tracks a single page view.
        /// </summary>
        /// <param name="pageName">The name of the viewed page.</param>
        void TrackPageView(string pageName);

        /// <summary>
        /// Tracks bandwidth ingress.
        /// </summary>
        /// <param name="bytes">The number of bandwidth bytes.</param>
        void TrackBandwidthIngress(long bytes);

        /// <summary>
        /// Tracks bandwidth egress.
        /// </summary>
        /// <param name="bytes">The number of bandwidth bytes.</param>
        void TrackBandwidthEgress(long bytes);
    }
}
