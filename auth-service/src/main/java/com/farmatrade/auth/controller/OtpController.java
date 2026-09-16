package com.farmatrade.auth.controller;

import com.farmatrade.auth.dto.OtpServiceResponse;
import com.farmatrade.auth.service.OtpServiceClient;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth/otp")
public class OtpController {
    private final OtpServiceClient otpServiceClient;

    public OtpController(OtpServiceClient otpServiceClient) {
        this.otpServiceClient = otpServiceClient;
    }

    @PostMapping("/send")
    public ResponseEntity<OtpServiceResponse> send(
            @Valid @RequestBody OtpSendRequest request) {
        otpServiceClient.sendOtp(request.email());
        return ResponseEntity.ok(new OtpServiceResponse(true, "OTP sent successfully."));
    }

    public record OtpSendRequest(
            @NotBlank @Email String email
    ) {
    }
}
