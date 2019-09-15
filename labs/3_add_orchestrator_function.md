# Lab 3 - Creating the orchestrator function

## Goal

The goal of this lab is to create a workflow in the orchestrator function which orchestrates the following activities:

- Save the `DetectedNeoEvent` object to a table storage.
- Retrieve the kinetic energy for the object.
- Retrieve the the impact probability for the object.
- Retrieve the Torino impact for the object.
- Save the `ProcessedNeoEvent` result in Table Storage.
- Send notifications to other organizations.

## Steps

### 1. Calling activities from an orchestrator

Remember that the state of orchestrator functions is checkpointed to storage. This happens when activities are being scheduled, started and completed.

The syntax for calling an activity is:



Continue to the [next lab](4_add_activity_functions.md) to create activity functions.