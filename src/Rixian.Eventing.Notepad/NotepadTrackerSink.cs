// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Sinks.Notepad
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Rixian.CloudEvents;
    using Rixian.Eventing;
    using Rixian.Eventing.Abstractions;
    using Serilog;
    using Serilog.Core;

    /// <summary>
    /// Tracker that writes to Azure Event Hubs.
    /// </summary>
    public class NotepadTrackerSink : ITrackerSink
    {
        private readonly Logger sink;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotepadTrackerSink"/> class.
        /// </summary>
        public NotepadTrackerSink()
        {
            this.sink = new LoggerConfiguration()
                .WriteTo.Notepad()
                .CreateLogger();
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task FlushAsync(IEnumerable<CloudEvent> events)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (events is null)
            {
                throw new System.ArgumentNullException(nameof(events));
            }

            foreach (CloudEvent? value in events)
            {
                this.sink.Information("{event}", Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}
