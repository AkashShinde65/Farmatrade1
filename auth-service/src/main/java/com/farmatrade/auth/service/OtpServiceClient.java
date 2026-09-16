package com.farmatrade.auth.service;

import com.farmatrade.auth.dto.OtpServiceResponse;
import com.farmatrade.auth.exception.OtpServiceException;
import java.util.Map;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.client.HttpClientErrorException;
import org.springframework.web.client.RestClient;
import org.springframework.web.client.RestClientException;

@Service
public class OtpServiceClient {
    private final RestClient restClient = RestClient.create();
    private final String baseUrl;
    private final String internalToken;

    public OtpServiceClient(
            @Value("${otp.service.base-url}") String baseUrl,
            @Value("${otp.service.internal-token}") String internalToken
    ) {
        this.baseUrl = baseUrl;
        this.internalToken = internalToken;
    }

    public void sendOtp(String email) {
        callOtp("/api/otp/send", Map.of("email", email), false);
    }

    public void verifyOtp(String email, String otp) {
        callOtp("/api/otp/verify", Map.of("email", email, "otp", otp), true);
    }

    private void callOtp(String path, Map<String, String> body, boolean verification) {
        try {
            OtpServiceResponse response = restClient.post()
                    .uri(baseUrl + path)
                    .header("X-Internal-Token", internalToken)
                    .body(body)
                    .retrieve()
                    .body(OtpServiceResponse.class);

            if (response == null || !response.success()) {
                throw new OtpServiceException(
                        502,
                        response == null ? "OTP service returned an empty response" : response.message()
                );
            }
        } catch (HttpClientErrorException.TooManyRequests ex) {
            throw new OtpServiceException(429, "Please wait before requesting another OTP");
        } catch (HttpClientErrorException.BadRequest ex) {
            throw new OtpServiceException(400,
                    verification ? "Invalid or expired OTP" : "Invalid email");
        } catch (RestClientException ex) {
            throw new OtpServiceException(502, "OTP service is unavailable");
        }
    }
}
