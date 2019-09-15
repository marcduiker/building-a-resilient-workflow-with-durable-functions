# Building a resilient workflow using Durable Functions

# XASA Workshop

Congratulations! Today is your first job as a software engineer at XASA, the Xpirit Aeronautics and Space Administration. You are responsible for creating a system which reacts to detected Near-Earth Objects ([NEOs](https://cneos.jpl.nasa.gov/about/basics.html)).

A satellite is continuously scanning the skies for these NEOs. The satellite transmits its findings to ground stations which in turn send the data to Azure.

It's your job to ensure the incoming data is stored, analyzed to assess the risk of impact, and to notify the required organizations of this risk and possible counter-measures (think of Armageddon).

## Technical solution

You are tasked with implementing the solution using Azure Functions. The reason behind this is that the number of detected NEOs changes heavily over time. And when nothing is being detected XASA prefers not paying for any infrastructure.

The NEO data (of type`DetectedNEOEvent`) looks as follows:

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
- *Estimated diameter is measured in km. Usually between 0.0001 and 10 km.*

Another team was tasked with the ingestion of the NEO data and this data is already being pushed to an Azure Servicebus Topic.

### NEO Event Processor

You will be responsible for creating a Function App that is being triggered by messages pushed to the Servicebus topic.

The Function App needs to make several calls to other services in order to determine the following:

- The kinetic energy of a potential impact
- The probability of an impact
- The [Torino impact](https://cneos.jpl.nasa.gov/sentry/torino_scale.html)

In addition to these service calls, the data needs to be stored to blob storage and a notification needs to be sent out if the Torino impact is equal or greater than 8.

## Labs

1. [Check Prerequisites](labs/0_prerequisites.md)
2. [Creating a new function project](labs/1_creating_a_function_project.md)
3. [Creating an orchestration client](labs/2_create_orchestration_client.md)
4. [Creating the orchestrator function](labs/3_create_orchestrator_function.md)
5. [Creating the activity functions](labs/4_create_activity_functions.md)