// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Rixian.Promptuary;

    /// <summary>
    /// Default implementation of IHttpTracker.
    /// </summary>
    public class DefaultHttpTracker : IHttpTracker
    {
        private readonly ITracker tracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHttpTracker"/> class.
        /// </summary>
        /// <param name="tracker">The ITracker instance.</param>
        public DefaultHttpTracker(ITracker tracker)
        {
            this.tracker = tracker;
        }

        /// <inheritdoc/>
        public Task FlushAsync()
        {
            return this.tracker.FlushAsync();
        }

        /// <inheritdoc/>
        public void Track(object value)
        {
            this.tracker.Track(value);
        }

        /// <inheritdoc/>
        public void TrackCloudEvent(string eventType, Uri source, object payload)
        {
            this.tracker.Track(CloudEvents.CloudEventV0_2.CreateCloudEvent(eventType, source, JObject.FromObject(payload)));
        }

        /// <inheritdoc/>
        public void TrackApiInvocation(string operationName, Uri source)
        {
            this.TrackCloudEvent("api.invocation", source, new
            {
                name = operationName,
            });
        }

        /// <inheritdoc/>
        public void TrackPageView(string pageName, Uri source)
        {
            this.TrackCloudEvent("page.view", source, new
            {
                name = pageName,
            });
        }
    }
}
