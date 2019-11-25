# Serverless Architecture - PoC for booking flights and hotels

## Azure DevOps Status

## Requirements
## Add Build pipeline to your Azure DevOps
Use the azure-pipelines.yml and import it to a build pipeline.

## Add Release Pipeline to your Azure DevOps
// Deploy will come later. The following content will include:
- Azure Infrastructure setup
- Deploy Functions to the specific Function Apps

## How to run locally
Add following to your local.settings.json:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
	"SkyScannerHostApi": "<Uri host>"
    "KeyVaultUri": "<KeyVault URI>",
    "ServicebusConnectionString": "<Storage URI>"
  },

  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  }
 }
  ```

## Use case
Showing a proof of concept for suggestions for booking flights and hotel. This will include Azure Functions and Durable Functions as a concept to fetch relevant data by getting a message from service 
bus and call different apis.

When the functions are done, the relevant user will get an email for suggestions of the given city, date and price.


### Architecture approach
<p align="center">
<img src="Documents/Images/BookingArchitecture.png"/>
</p>

## Powerpoint slides
Not available