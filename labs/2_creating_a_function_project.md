# Creating a Function App Project

## Goal

The goal of this lab is to create a basic Function App with a Servicebus triggered function and to determine it's connected correctly and is receiving events.

## Steps

### 1. Create a Function App
With your IDE of choice create a Function App with an Servicebus triggered function.

- When asked specly that you want to use the Storage Emulator which is used to run the app locally.
- Specify the following connectionstring either through the IDE or direcly in local.settings.json:

    `"NEOEventsTopic":"<ConnectionString>"`

### 2. Run the Function App locally
Put a breakpoint in the Servicebus triggered function and run the Function App locally.

The breakpoint should be hit after a couple of seconds since the NEO event generator.

