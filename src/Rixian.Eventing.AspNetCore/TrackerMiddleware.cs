﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.AspNetCore
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Rixian.Eventing.Abstractions;

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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext httpContext, ITracker tracker)
        {
            if (httpContext is null)
            {
                throw new System.ArgumentNullException(nameof(httpContext));
            }

            if (tracker is null)
            {
                throw new System.ArgumentNullException(nameof(tracker));
            }

            tracker.Tags["http.traceIdentifier"] = httpContext.TraceIdentifier;
            tracker.Tags["http.request.path"] = httpContext.Request.Path;
            tracker.Tags["http.request.method"] = httpContext.Request.Method;

            if (httpContext.Request.Host.HasValue)
            {
                tracker.Tags["http.request.host"] = httpContext.Request.Host.Value;
            }

            var sw = Stopwatch.StartNew();
            await this.next(httpContext).ConfigureAwait(false);
            sw.Stop();

            tracker.Tags["http.response.status_code"] = httpContext.Response.StatusCode;
            tracker.Tags["http.elapsed_milliseconds"] = sw.ElapsedMilliseconds;

            // Identity tags
            if (httpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                var sub = httpContext.User.FindFirstValue("sub");
                if (string.IsNullOrWhiteSpace(sub) == false)
                {
                    tracker.Tags["http.identity.sub"] = sub;
                }

                var clientId = httpContext.User.FindFirstValue("client_id");
                if (string.IsNullOrWhiteSpace(clientId) == false)
                {
                    tracker.Tags["http.identity.client_id"] = clientId;
                }
            }

            await tracker.FlushAsync().ConfigureAwait(false);
        }
    }
}
