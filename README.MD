# Vidly

Vidly is a web-application using ASP.NET MVC 5 created as a learning project following [Mosh Hamedanis](https://github.com/mosh-hamedani) Udemy course as a base line.

## Prerequisites

To deploy this application it is assumed that two files exist in the project root:

* Web.Secrets.AppSettings.config
* Web.Secrets.Connections.config

They should follow these templates:

### Web.Secrets.AppSettings.config

```xml
  <appSettings>
    <add key="webpages:Enabled" value="false" />
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />

    <add key="facebookAppID" value="{facebook-app-id}"/>
    <add key="facebookAppSecret" value="{facebook-app-secret}"/>

    <add key="googleAppID" value="{google-app-id}"/>
    <add key="googleAppSecret" value="{google-app-secret}"/>

    <add key="twitterAppID" value="{twitter-app-id}"/>
    <add key="twitterAppSecret" value="{twitter-app-secret}"/>

    <add key="microsoftAppID" value="{microsoft-app-id}"/>
    <add key="microsoftAppSecret" value="{microsoft-app-secret}"/>

    <add key="twilioID" value="{twilio-id}" />
    <add key="twilioKey" value="{twilio-key}" />
    <add key="twilioFromNumber" value="{twilio-from-number}" />

    <add key="sendGridKey" value="{send-grid-key}" />
  </appSettings>
```

### Web.Secrets.Connections.config

```xml
  <connectionStrings>
    <add name="DefaultConnection" connectionString="{connection-string}" providerName="System.Data.SqlClient" />
  </connectionStrings>
```