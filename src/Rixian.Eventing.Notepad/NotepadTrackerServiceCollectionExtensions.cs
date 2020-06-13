// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Eventing.Abstractions;
    using Rixian.Eventing.Sinks.Notepad;

    /// <summary>
    /// Extensions for configuring the NotepadTracker.
    /// </summary>
    public static class NotepadTrackerServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the AzureEventHubTracker.
        /// </summary>
        /// <param name="trackerBuilder">The ITrackerBuilder instance.</param>
        /// <returns>The updated ITrackerBuilder instance.</returns>
        public static ITrackerBuilder WithNotepadSink(this ITrackerBuilder trackerBuilder)
        {
            if (trackerBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(trackerBuilder));
            }

            trackerBuilder.Services.AddScoped<ITrackerSink, NotepadTrackerSink>();

            return trackerBuilder;
        }
    }
}
