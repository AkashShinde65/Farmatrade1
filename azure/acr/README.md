# Azure Container Registry

## FarmaTrade Container Images

Replace `<ACR_NAME>` with the Azure Container Registry name.

- `<ACR_NAME>.azurecr.io/farmatrade-auth-service`
- `<ACR_NAME>.azurecr.io/farmatrade-lot-service`
- `<ACR_NAME>.azurecr.io/farmatrade-bidding-service`
- `<ACR_NAME>.azurecr.io/farmatrade-logistics-service`
- `<ACR_NAME>.azurecr.io/farmatrade-billing-service`
- `<ACR_NAME>.azurecr.io/farmatrade-otp-service`
- `<ACR_NAME>.azurecr.io/farmatrade-frontend`

## Build Contexts

| Service | Dockerfile | Port |
|---|---|---:|
| Auth | auth-service/Dockerfile | 8081 |
| Lot | lot-service/Dockerfile | 8082 |
| Bidding | bidding-service/Dockerfile | 8083 |
| Logistics | logistics-service/Dockerfile | 8084 |
| Billing | billing-service/Dockerfile | 8085 |
| OTP | otp-service/Dockerfile | 8086 |
| Frontend | frontend/Dockerfile | 80 |

Images will be built from the existing application Dockerfiles and stored in Azure Container Registry before deployment to Azure Container Apps.
