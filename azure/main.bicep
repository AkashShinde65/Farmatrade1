targetScope = 'resourceGroup'

@description('Azure region for FarmaTrade')
param location string = resourceGroup().location

@description('Globally unique Azure Container Registry name')
param acrName string

@description('Container Apps environment name')
param containerAppsEnvironmentName string = 'farmatrade-env'

resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: acrName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: false
    publicNetworkAccess: 'Enabled'
  }
}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2026-01-01' = {
  name: containerAppsEnvironmentName
  location: location
  properties: {}
}

output acrLoginServer string = acr.properties.loginServer
output acrResourceId string = acr.id
output containerAppsEnvironmentId string = containerAppsEnvironment.id
