param location string
param environmentId string
param acrLoginServer string

resource authApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-auth'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8081
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'auth-service'
          image: '${acrLoginServer}/farmatrade-auth-service:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource lotApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-lot'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8082
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'lot-service'
          image: '${acrLoginServer}/farmatrade-lot-service:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource biddingApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-bidding'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8083
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'bidding-service'
          image: '${acrLoginServer}/farmatrade-bidding-service:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource logisticsApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-logistics'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8084
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'logistics-service'
          image: '${acrLoginServer}/farmatrade-logistics-service:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource billingApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-billing'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8085
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'billing-service'
          image: '${acrLoginServer}/farmatrade-billing-service:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource otpApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-otp'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8086
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'otp-service'
          image: '${acrLoginServer}/farmatrade-otp-service:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

resource frontendApp 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'farmatrade-frontend'
  location: location
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 80
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'frontend'
          image: '${acrLoginServer}/farmatrade-frontend:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

output frontendFqdn string = frontendApp.properties.configuration.ingress.fqdn
