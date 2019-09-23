# Building a resilient workflow using Durable Functions

# XASA Workshop

Congratulations! Today is your first job as a software engineer at XASA, the Xpirit Aeronautics and Space Administration. You are responsible for creating a system which reacts to detected Near-Earth Objects ([NEOs](https://cneos.jpl.nasa.gov/about/basics.html)).

![Known NEOs](https://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Asteroids-KnownNearEarthObjects-Animation-UpTo20180101.gif/320px-Asteroids-KnownNearEarthObjects-Animation-UpTo20180101.gif)

A satellite is continuously scanning the skies for these NEOs. The satellite transmits its findings to ground stations which in turn send the data to Azure.

It's your job to ensure the incoming data is analyzed to assess the risk of impact, stored, and to notify the required organizations of this risk and possible counter-measures (think Armageddon style).

## Technical solution

You are tasked with implementing the solution using Azure Functions. The reason behind this is that the number of detected NEOs changes heavily over time. And when nothing is being detected XASA prefers not paying for any infrastructure.

The NEO data (of type `DetectedNEOEvent`) looks as follows:

```json
{
    "id" : "77c924dc-883c-4f53-922f-7cddb7325121",
    "date" : "2019-04-23T18:25:43.511Z",
    "distance": 3.5,
    "velocity" : 10,
    "diameter" : 0.52
}
```

- *Distance is measured in Astronomical Units (AU). Usually between 1-5 AU.*
- *Velocity is measured in km/s. Usually between 5-30 km/s*
- *Diameter is measured in km. Usually between 0.0001 and 10 km.*

Another team was tasked with the ingestion of the NEO data and this data is already being pushed to an Azure Servicebus topic.

### NEO Event Processor Function App

You will be responsible for creating a Function App that is triggered by messages pushed to the Servicebus topic.

The Function App needs to make several calls to other services in order to determine the following:

- The kinetic energy of a potential impact
- The probability of an impact
- The [Torino impact](https://cneos.jpl.nasa.gov/sentry/torino_scale.html)

![Torino impact](https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Torino_scale.svg/320px-Torino_scale.svg.png)

In addition to these service calls, the processed data needs to be stored to blob storage (for events with a Torino impact >= 1) and a notification needs to be sent out to Bruce Willis (for events with a Torino impact >= 8).

> The final implementation is also in this repo. However, it is lots more fun, and you learn way more, by creating your own solution and following all the labs. Only peek at my solution if you're completely stuck.

>**I strongly suggest you team up with someone to do pair programming and discuss what you're doing.**

> Have fun! Marc

## Labs

0. [Check Prerequisites](labs/0_prerequisites.md) & [Get your subscription keys](labs/0_subscribe.md)
1. [Creating a new function project](labs/1_creating_a_function_project.md)
2. [Creating an orchestration client](labs/2_create_orchestration_client.md)
3. [Creating the orchestrator function](labs/3_create_orchestrator_function.md)
4. [Calling other services](labs/4_create_activity_functions_services.md)
5. [Storing the ProcessedNeoEvent](labs/5_create_activity_function_storage.md)
6. [Sending a notification](labs/6_send_notification.md)
7. [Unit testing](labs/7_unit_testing.md)
8. [Creating Azure resources](labs/8_azure_resources.md)
9. [Publish to Azure](labs/9_publish_to_azure.md)