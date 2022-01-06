// resources using the format environment-team-appName-appType-region,
// excluding storage account

param location string = resourceGroup().location
param appName string = 'payrollprocessor'
param team string = 'nitrodevs'
param env string = 'q'

var appServicePlanName = '${env}-${team}-${appName}-api-appservice-${location}'
var appServiceName = '${env}-${team}-${appName}-api-${location}'

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: resourceGroup().location
  sku: {
    name: 'F1'
  }
  kind: ('windows')
  tags: null
}
resource appService 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      alwaysOn: true
      ftpsState: 'Disabled'
      netFrameworkVersion: 'v6.0'
      // appSettings: [
      //   {
      //     'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
      //     'value': appInsights.outputs.instrumentationKey
      //   }
      // ]
    }
    httpsOnly: true
  }
  tags: null
}
