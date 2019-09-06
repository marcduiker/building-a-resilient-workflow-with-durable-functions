# Building a resilient workflow using Durable Functions

A satellite is scanning the skies for Near Earth Objects (NEOs). The satellite transmits its findings to ground stations which in turn send the data to Azure.

It your job to ensure the incomming data is stored, analyzed to assess the risk and the required organizations are notified of this risk.

## Function Apps involved

### NEO Event Generator
The NEO Generator is a function app which generates NEO events (see below) and pushes these events to a Azure Service Bus Topic.

NEO event sample:
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


## NEO Event Processor

This function app picks up the events from the Service Bus Topic and will then do the neccesary processing and alerting.

## Labs

1. [Check Prerequisites](labs/prerequisites.md)
2. [Creating a new fucntion project](labs/creating_a_function_project.md)
3. [Receiving events](labs/receiving_events.md)
4. []()