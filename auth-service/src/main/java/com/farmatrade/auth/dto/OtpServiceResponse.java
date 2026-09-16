package com.farmatrade.auth.dto;

public record OtpServiceResponse(
        boolean success,
        String message
) {
}
