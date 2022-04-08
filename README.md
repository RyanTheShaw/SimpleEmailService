# Simple Email Service

Welcome to the simple email service! This application demos a simple .net 6 web api enabling basic CRUD operations against an in-memory database. 

The core breakdown of functionality comes in 3 layers allowing for proper separation of concerns. The `SimpleEmailService` project hosts the Api / web project responsibilities, which utilizes the `SimpleEmailService.Core` project. The `SimpleEmailService.Core` project represents the business logic of the application, running validations and filtering functionality on our data sets. Finally the `SimpleEmailService.DataAccess` project hosts our ORM (Object Relational Mapping) library which allows for us to interact with our entities in our in-memory database. 

## Assumptions made during development

- The requirements doc mentioned exposing CRUD operations against these shapes, but it didn't specifically request a dedicated controller for just the Email records. As the email records are able to be adjusted through the parent `Contact` shape, I decided to simply implement only the `ContactController`.

- Birth date range for searching didn't mention inclusive / exclusive, so I made the decision to make it inclusive.

- The requirement for "Business logic should be accessible to a separate Console application" didn't mention if that functionality should me implemented, so my approach to doing so is pretty barebones and linear. Please trust that if requested, I am absolutely capable of making a much more rich and interactive console application experience.

## Site configuration

- There is a configuration value in `appsettings.json` in the web project called `SeedDataOnStartup`, which toggles the dataseeding process based on a true or false value.

- Simply run the web application and navigate to `{baseUrl}/swagger/index.html` to start making example calls against the api.

## Unit testing

Comprehensive unit testing can be found in the `SimpleEmailService.Tests` project. These tests are carried against the `SimpleEmailService.Core` service layer. 

> Note: The unit tests were built utilizing the the ef InMemory database implementation, which some would argue makes them more of an integration test. I briefly explored using Moq to truely abstract away the functionality of our DbContext, but I figured this was fine for now.


## Local testing

Below are some values that are easy to copy and paste into swagger 

### Example Json Payloads

#### Post valid record - no id, one email
```
{
  "name": "Weird Al",
  "birthDate": "1959-10-23",
  "emails": [
    {
      "isPrimary": true,
      "address": "weirdalemail@example.com"
    }
  ]
}
```

#### Post invalid record - no id, two primary emails
```
{
  "name": "Weird Al",
  "birthDate": "1959-10-23",
  "emails": [
    {
      "isPrimary": true,
      "address": "weirdalemail@example.com"
    },
    {
      "isPrimary": true,
      "address": "weirdalemail2@example.com"
    }
  ]
}
```

#### Put valid record - by id, updated name
```
{
  "id": 1,
  "name": "Weird Al - Updated name!",
  "birthDate": "1959-10-23",
  "emails": [
    {
      "isPrimary": true,
      "address": "weirdalemail@example.com"
    }
  ]
}
```

#### Contact/Search Post endpoint - search by name
```
{
  "namePartial": "ryan"
}
```

