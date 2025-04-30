targetScope = 'subscription'

param location string
param resourceGroupName string
param vnetName string
param vnetAddress string
param endpointSubnetName string
param endpointSubnetAddress string
param caeSubnetName string
param caeSubnetAddress string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-11-01' = {
  name: resourceGroupName
  location: location
}

module network 'modules/network/main.bicep' = {
  name: take('network-${deployment().name}-deployment', 64)
  scope: resourceGroup
  params: {
    location: location
    caeSubnetAddress: caeSubnetAddress
    caeSubnetName: caeSubnetName
    endpointSubnetAddress: endpointSubnetAddress
    endpointSubnetName: endpointSubnetName
    vnetAddress: vnetAddress
    vnetName: vnetName
  }
}
