# Lab 0.B Servicebus & API Management keys

## Goal

The goals is to obtain the connectionstring and keys which you need to use in later labs to connect to the Servicebus topic and to the API management endpoints.

## Steps

Do a GET request to this endpoint: `https://demo-neo.azure-api.net/neo/subscribe?name=YOURNAME` and replace YOURNAME with your actual name. Your name will be used to create a personal subscriptionkey which will be used to listen to the Servicebus topic where the `DetectedNeoEvents` are being pushed to.

The result of the request should look like this:

```json
{
  "serviceBusTopicName": "neo-events",
  "serviceBusConnectionString": "<actual_connectionstring>",
  "servicebusTopicSubscriptionName": "<your_personal_topic_subscription>",
  "apiManagementKey": "<actual_apimanagement_key>"
}
```

Make sure you save the output of this request so you can use the connectionstring and keys in your function app later.