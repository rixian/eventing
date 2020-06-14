# Rixian Eventing

[![NuGet package](https://img.shields.io/nuget/v/Rixian.Eventing.svg)](https://nuget.org/packages/Rixian.Eventing)

## Features

* Simple event tracking with the `IEventTracker` interface.
* Event augmentation with custom fields.
* Event payload follows the [CloudEvent v1.0 spec](https://github.com/cloudevents/spec/blob/v1.0/spec.md).
* ASP.NET Core integration:
  * Auto-flush at the end of each request.
  * Automatically extend the payload with request information.
  * Extensions for tracking commonly used events, such as api invocation or page views.
* Various sink providers for writing events:
  * Azure Event Hubs
  * Notepad

## Getting Started

This will assume you are working with an ASP.NET Core application. The following steps will help you get the library installed and configured:

1. Install the NuGet package:

   ```bash
   dotnet add package Rixian.Eventing.AspNetCore
   ```

2. Register the event tracker services:

   ```csharp
   services
       .AddTracking("fabrikam-api")
       .WithHostInfo()
       .WithNotepadSink();
   ```

3. Register the middleware to flush events at the end of each request:

   ```csharp
   public void Configure(IApplicationBuilder app)
   {
       app.UseTracker();
       ...
   }
    ```

4. Use in a controller:

   ```
   public DefaultController(ITracker tracker)
   {
       CloudEvent cloudEvent = CloudEvent.CreateCloudEvent("test", new Uri("/", UriKind.Relative), "A test message");
       tracker.Track(cloudEvent);
   }
   ```
