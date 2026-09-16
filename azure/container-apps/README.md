# Azure Container Apps

## FarmaTrade Application Containers

The application will be deployed as separate Azure Container Apps:

1. auth-service — 8081
2. lot-service — 8082
3. bidding-service — 8083
4. logistics-service — 8084
5. billing-service — 8085
6. otp-service — 8086
7. frontend — 80

## Service Communication

Backend services will communicate using Azure Container Apps internal service-to-service networking.

The frontend will communicate with the public application endpoints.

## External Dependencies

- Azure Database for MySQL
- Azure Container Registry
- Gmail SMTP for OTP email
- Razorpay for payment processing

## Production Secrets

Secrets such as database passwords, JWT RSA private key, internal service token, Gmail App Password, and Razorpay credentials must be supplied through Azure secrets/environment configuration.

No production secrets should be committed to GitHub.
