// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using Rixian.CloudEvents;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Default implementation of IHttpTracker.
    /// </summary>
    public class DefaultHttpTracker : IHttpTracker
    {
        private readonly ITracker tracker;
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHttpTracker"/> class.
        /// </summary>
        /// <param name="tracker">The ITracker instance.</param>
        /// <param name="httpContextAccessor">The IHttpContextAccessor instance.</param>
        public DefaultHttpTracker(ITracker tracker, IHttpContextAccessor httpContextAccessor)
        {
            this.tracker = tracker;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public IDictionary<string, object?> Tags => this.tracker.Tags;

        /// <inheritdoc/>
        public Task FlushAsync()
        {
            return this.tracker.FlushAsync();
        }

        /// <inheritdoc/>
        public void Track(CloudEvent value)
        {
            this.tracker.Track(value);
        }

        /// <inheritdoc/>
        public void TrackCloudEvent(string eventType, object payload)
        {
            var sourceUri = new Uri(this.httpContextAccessor.HttpContext.Request.Path, UriKind.Relative);
            this.tracker.Track(CloudEvent.CreateCloudEvent(eventType, sourceUri, JObject.FromObject(payload)));
        }

        /// <inheritdoc/>
        public void TrackApiInvocation(string operationName)
        {
            this.TrackCloudEvent("api.invocation", new
            {
                operation = operationName,
            });
        }

        /// <inheritdoc/>
        public void TrackPageView(string pageName)
        {
            this.TrackCloudEvent("page.view", new
            {
                page = pageName,
            });
        }

        /// <inheritdoc/>
        public void TrackBandwidthIngress(long bytes)
        {
            this.TrackCloudEvent("bandwidth.ingress", new
            {
                bytes,
            });
        }

        /// <inheritdoc/>
        public void TrackBandwidthEgress(long bytes)
        {
            this.TrackCloudEvent("bandwidth.egress", new
            {
                bytes,
            });
        }
    }
}
