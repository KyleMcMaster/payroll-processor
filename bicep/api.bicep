param appName string
param appServicePlanId string
param env string
param location string
param team string

var appServiceName = '${env}-${team}-${appName}-api-${location}'

resource appService 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      alwaysOn: false
      ftpsState: 'Disabled'
      netFrameworkVersion: 'v6.0'
      // appSettings: [
      //   {
      //     'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
      //     'value': appInsights.outputs.instrumentationKey
      //   }
      // ]
    }
    httpsOnly: false
  }
  tags: null
}
