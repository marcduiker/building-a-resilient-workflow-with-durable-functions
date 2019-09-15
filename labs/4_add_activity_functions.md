# Lab 4 -  Adding activity functions to the orchestrator

## Goal

The goal of this lab is to create a workflow in the orchestrator function which does the following:

- Calls the endpoint to get the kinetic energy.
- Calls the endpoint to get the impact probability.
- Call the endpoint to get the Torino impact.
- Saves the `ProcessedNeoEvent` result in Table Storage.

## Steps




The syntax for calling an activity is:

Microsoft.Azure.WebJobs.Extensions.Storage