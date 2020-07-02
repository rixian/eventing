// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Rixian.Eventing.Abstractions;

    /// <summary>
    /// Background service for flushing records to the underlying sinks.
    /// </summary>
    public class TrackingFlushService : BackgroundService
    {
        private readonly ChannelReader<TrackerSession> channelReader;
        private readonly ILogger<TrackingFlushService> logger;
        private readonly IServiceProvider services;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingFlushService"/> class.
        /// </summary>
        /// <param name="channelReader">The channel reader.</param>
        /// <param name="logger">The ILogger instance.</param>
        /// <param name="services">The service provider.</param>
        public TrackingFlushService(ChannelReader<TrackerSession> channelReader, ILogger<TrackingFlushService> logger, IServiceProvider services)
        {
            this.channelReader = channelReader;
            this.logger = logger;
            this.services = services;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (this.channelReader is null)
            {
                return;
            }

            try
            {
                while (await this.channelReader.WaitToReadAsync().ConfigureAwait(false))
                {
                    if (this.channelReader.TryRead(out TrackerSession session))
                    {
                        if (session.Events is null ||
                            session.Events.Any() == false)
                        {
                            // Skip message
                            this.logger.LogDebug(Properties.Resources.FlushServiceNoEventsErrorMessage);
                            continue;
                        }

                        using IServiceScope? scope = this.services.CreateScope();
                        IEnumerable<ITrackerSink>? sinks = scope.ServiceProvider.GetServices<ITrackerSink>();
                        if (sinks is null ||
                            sinks.Any() == false)
                        {
                            // Skip message
                            this.logger.LogDebug(Properties.Resources.FlushServiceNoSinksErrorMessage);
                            continue;
                        }

                        foreach (ITrackerSink? sink in sinks)
                        {
                            try
                            {
                                await sink.FlushAsync(session.Events).ConfigureAwait(false);
                            }
#pragma warning disable CA1031 // Do not catch general exception types
                            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                            {
                                // Eat the exception. These should be caught by the sink.
                            }
                        }
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.logger.LogError(ex, Properties.Resources.FlushServiceGenericErrorMessage);
            }
        }
    }
}
