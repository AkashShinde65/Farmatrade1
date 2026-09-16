package com.farmatrade.auth.exception;

public class OtpServiceException extends RuntimeException {
    private final int status;

    public OtpServiceException(int status, String message) {
        super(message);
        this.status = status;
    }

    public int status() {
        return status;
    }
}
