# gtk-sqs
sample project for integrating AWS SQS into ASP.NET Core API. it uses a basic AWS authorization to a queue named `orders` and has two main functionalities - send a message and receive a message. once the message is received, it gets processed and deleted from the queue. this is basic logic that is offered by `AWS SQS`

### scaffolding
- I have changed the `launchSettings.json` to run in `Local` environment under `sqs.core` start
- `Start.cs` injects the environment settings needed to create basic authorization to `AWS`
- `services.AddAWSService<IAmazonSQS>()` adds the SQS functionality needed into the `IServiceCollection`
- you can then inject `IAmazonSQS` anywhere in your code and use its functionality
- the `SqsRepository.cs` uses the SQS client to send and receive messages

### how to run it
first, you'll need to add the environment settings that are referenced in `Startup.cs` here

``` csharp
Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS_ACCESS_KEY_ID"]);
Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS_SECRET_ACCESS_KEY"]);
Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS_REGION"]);
```

by adding a `appsettings.Local.json` (or if you'd like, add them straight to your `appsettings.Development.json` and run the app). mine looks like this
``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "AWS_REGION": "your region here",
  "AWS_ACCESS_KEY_ID": "your access key id here",
  "AWS_SECRET_ACCESS_KEY": "your secret access key here",
  "AWS_QUEUE_URL": "your queue url here"
}
```

the `launchSettings.json` custom entry point is here
``` json
"sqs.core": {
  "commandName": "Project",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Local"
  },
  "applicationUrl": "http://localhost:5000"
}
```
if you opt-in to use `appsettings.Local.json`, then you'd just need to follow the above instructions to add the file and you can then run the app. Otherwise, you'd have to change the `ASPNETCORE_ENVIRONMENT` here to `Development` and add the `AWS_REGION`, `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY` and `AWS_QUEUE_URL` environment variables to the `appsettings.Development.json` file

### docs
- https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
- https://docs.aws.amazon.com/sdk-for-net/v2/developer-guide/how-to-sqs.html
