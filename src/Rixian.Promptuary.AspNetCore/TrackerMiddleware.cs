// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Promptuary.AspNetCore
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Middleware for flushing the current tracker after a request.
    /// </summary>
    public class TrackerMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware to execute.</param>
        public TrackerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="httpContext">The HttpContext.</param>
        /// <param name="tracker">The ITracker instance.</param>
        /// <param name="trackerProperties">The ITrackerProperties instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext httpContext, ITracker tracker, ITrackerProperties trackerProperties)
        {
            if (httpContext is null)
            {
                throw new System.ArgumentNullException(nameof(httpContext));
            }

            if (tracker is null)
            {
                throw new System.ArgumentNullException(nameof(tracker));
            }

            if (trackerProperties is null)
            {
                throw new System.ArgumentNullException(nameof(trackerProperties));
            }

            trackerProperties.Properties["aspnet.traceIdentifier"] = httpContext.TraceIdentifier;
            trackerProperties.Properties["aspnet.request.path"] = httpContext.Request.Path;
            trackerProperties.Properties["aspnet.request.method"] = httpContext.Request.Method;
            trackerProperties.Properties["aspnet.request.host"] = httpContext.Request.Host;

            var sw = Stopwatch.StartNew();
            await this.next(httpContext).ConfigureAwait(false);
            sw.Stop();

            trackerProperties.Properties["aspnet.response.status_code"] = httpContext.Response.StatusCode;
            trackerProperties.Properties["aspnet.elapsed_milliseconds"] = sw.ElapsedMilliseconds;

            await tracker.FlushAsync().ConfigureAwait(false);
        }
    }
}
