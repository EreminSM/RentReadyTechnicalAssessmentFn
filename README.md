# Technical Assessment for Rent Ready
This is an Azure Function example which interacts with Dynamics 365  msdyn_timeentry entity.

# Setting up
For running of this function, you need to make some steps and adjustments.
- To deploy the function into [Azure account] (https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs?tabs=in-process#publish-to-azure)
- Need to prepare OAuth authorization, you can use [this good guidance](https://optimallogics.com/connect-to-ms-dynamics-365-crm-using-oauth-in-c-sharp/)
- To prepare and set-up on the Azure side the connection string for the Dynamics 365. 

## Connection string preapring and setting-up
The string has the next view:<br />
`Url=https://{environment}.dynamics.com;AuthType=ClientSecret;ClientId={clientId};ClientSecret={clientSecret};RequireNewInstance=true` <br />
The values for **{environment}**, **{clientId}** and **{clientSecret}** we have to take from the step **'Need to prepare OAuth authorization'**.<br /> 
Once the connection string is ready, we have to go Azure portal->Function app where the function is deployed->Configuration->Connection strings->New connection string, then set the name as 'ConnectToDynamics365' and choose the type as 'Custom', then click on Ok button and click on Save button (at the top).

# How to test the function
Go to Azure portal->Function app where the function is deployed->Functions->AddDates (the name of the function)->Code+Test.<br />
Then, on the top, click on 'Test/Run'. Then, in the right-bottom, put into text area 'Body' a valid JSON, like, for example:
```
{
  "StartOn": "2020-07-03",
  "EndOn": "2020-07-10"
}
```
And click 'Run'. After running, it should show a message like `This HTTP triggered function executed successfully. Added entries count: {X}`, where **{x}**  is count of added entries.