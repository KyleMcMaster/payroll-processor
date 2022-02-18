param appName string
param env string
param functionRuntime string = 'dotnet'
param location string
param team string

var functionAppName = '${env}-${team}-${appName}-func-${location}'
// var appServicePlanName = '${env}-${team}-${appName}-func-appservice-${location}'
var storageAccountName = format('{0}sta', appName)

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

// Blob Services for Storage Account
resource blobServices 'Microsoft.Storage/storageAccounts/blobServices@2019-06-01' = {
  parent: storageAccount

  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

// App Service Plan
// resource appService 'Microsoft.Web/serverFarms@2020-06-01' = {
//   name: appServicePlanName
//   location: location
//   kind: 'functionapp'
//   sku: {
//     name: 'Y1'
//     tier: 'Dynamic'
//     size: 'Y1'
//     family: 'Y'
//     capacity: 0
//   }
//   properties: {
//     perSiteScaling: false
//     maximumElasticWorkerCount: 1
//     isSpot: false
//     reserved: false
//     isXenon: false
//     hyperV: false
//     targetWorkerCount: 0
//     targetWorkerSizeId: 0
//   }
// }

// Function App
resource functionApp 'Microsoft.Web/sites@2020-06-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${functionAppName}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${functionAppName}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
    ]
    //serverFarmId: appService.id
    reserved: false
    isXenon: false
    hyperV: false
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionRuntime
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
      ]
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    hostNamesDisabled: false
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
  }
}
