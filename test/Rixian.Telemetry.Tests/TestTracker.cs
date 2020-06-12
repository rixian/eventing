// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using FluentAssertions;
using Rixian.Telemetry;
using Xunit;
using Xunit.Abstractions;

public class TestTracker : BaseTracker
{
    public override Task FlushAsync()
    {
        return Task.CompletedTask;
    }

    public IEnumerable<object> ListTrackedValues() => this.TrackedValues;
}
