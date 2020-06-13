// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using FluentAssertions;
using Rixian.CloudEvents;
using Rixian.Eventing;
using Rixian.Eventing.Abstractions;
using Xunit;
using Xunit.Abstractions;

public class TestTrackerSink : ITrackerSink
{
    public Task FlushAsync(IEnumerable<CloudEvent> events)
    {
        return Task.CompletedTask;
    }
}
