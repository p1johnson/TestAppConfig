targetScope = 'resourceGroup'

param location string
param vnetName string
param vnetAddress string
param endpointSubnetName string
param endpointSubnetAddress string
param caeSubnetName string
param caeSubnetAddress string

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2024-05-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddress
      ]
    }
    subnets: [
      {
        name: endpointSubnetName
        properties: {
          addressPrefix: endpointSubnetAddress
        }
      }
      {
        name: caeSubnetName
        properties: {
          addressPrefix: caeSubnetAddress
          delegations: [
            {
              name: 'envdelegation'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
        }
      }
    ]
  }
}
