# Lab 2 - Creating a Function App Project

## Goal

The goal of this lab is to create a basic Function App with a Servicebus triggered function and to determine it's connected correctly and is receiving messages from the bus.

## Steps

### 1. Create a Function App

With your IDE of choice create a Function App with a Servicebus Topic triggered function.

- When asked, specify that you want to use the Storage Emulator which is used to run the app locally.
- Specify the following Servicebus connectionstring either through the IDE or direcly in local.settings.json:

    -   Connectionstring setting name: `NEOEventsTopic`
    -   Topic name: `neo-events`
    -   Subscription name: `<subscriptionname>` (will be different for each attendee)

The resulting function should look like this:

```

```

### 2. Convert the messages to DetectedNeoEvent objects



### 3. Run the Function App locally

Put a breakpoint in the Servicebus triggered function and run/debug your Function App on your local machine.

> The Azure Function Runtime should start in a console window and show some diagnostic information. If there are any issues with storage and/or service bus configuration you will see the errors in this console. 

The breakpoint should be hit after a couple of seconds since the NEO event generator pushes a couple of messages to the Servicebus every 10 seconds.

If the breakpoint is being hit you can move on to the [next lab]().
