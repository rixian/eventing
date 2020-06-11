// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.AspNetCore.Builder
{
    using Rixian.Promptuary.AspNetCore;

    /// <summary>
    /// Extensions for registering the track middleware.
    /// </summary>
    public static class TrackerMiddlewareExtensions
    {
        /// <summary>
        /// Registers the tracker middleware.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <returns>The updated IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseTracker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TrackerMiddleware>();
        }
    }
}
