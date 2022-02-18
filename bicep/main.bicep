/*
based on tutorial here
https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/deploy-github-actions?tabs=CLI
source:
https://github.com/Azure/bicep/blob/main/docs/examples/101/function-app-create/main.bicep
*/

// resources using the format environment-team-appName-appType-region, excluding storage account
param appName string = 'payrollprocessor'
param env string = 'q'
param location string = resourceGroup().location
param team string = 'nitrodevs'

// App Service Plan
var appServicePlanName = '${env}-${team}-${appName}-appservice-${location}'
resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: resourceGroup().location
  sku: {
    name: 'F1'
  }
  kind: ('windows')
  tags: null
}

// API App Service
module apiAppService './api.bicep' = {
  name: 'apiAppService'
  params: {
    appName: appName
    appServicePlanId: appServicePlan.id
    env: env
    location: resourceGroup().location
    team: team
  }
}

// Azure Function
module functionApp './function.bicep' = {
  name: 'functionApp'
  params: {
    appName: appName
    env: env
    location: resourceGroup().location
    team: team
  }
}
