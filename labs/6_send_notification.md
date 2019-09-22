# Lab 5 -  Call Bruce

## Goal

The goal of this lab is to implement the activity which sends an email to Bruce Willis, since he's obviously [the only one who can save the planet](https://en.wikipedia.org/wiki/Armageddon_(1998_film)). 

Make sure you only send out an email for detected NEOs with a Torino impact of 8 or higher. Bruce will not come out of his bed for non-Earth-threatening events.

## Steps

### 1. SendGrid account

In order to send emails we're going to use a service to take care of this. Go to  [signup.sendgrid.com/](https://signup.sendgrid.com/) in order to create a (free) account.

### 2. SendGrid email template

SendGrid offers several types of emails that can be sent. We want a transactional type. 

- In the SendGrid Dahboard go to Templates > Transactional 
- Create a new template and give it a name  
- Add a version to the template
- Choose which editor style you want to compose the template
- Add some elements to the email.
- Save your template

> Every template has an ID, which is visible underneath the template name. You'll need this in the activity function you're goint to write.

### 3. SendGrid API key

In order to use the SendGrid service an API key is required.

- In the SendGrid Dahboard go to Settings > API keys
- Create a new API key
- Enter a name for it
- Select restricted access and configure full access for only the Mail Send category.
- Click Create & View and copy it to the `local.settings.json` of your Function App




### 4. SendGrid extension

Lets start by adding the following NuGet package to the project: `Microsoft.Azure.WebJobs.Extensions.Storage` so we can work with SendGrid resources.

### 3. Adding a new class for the activity

Microsoft.Azure.WebJobs.Extensions.Storage

### 2. Calling the activity from the orchestration


### 3. Build & run locally



