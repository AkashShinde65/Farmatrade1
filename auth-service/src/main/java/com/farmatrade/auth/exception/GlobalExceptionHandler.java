package com.farmatrade.auth.exception;

import java.util.LinkedHashMap;
import java.util.Map;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class GlobalExceptionHandler {
    @ExceptionHandler(MethodArgumentNotValidException.class)
    ResponseEntity<Map<String, Object>> validation(MethodArgumentNotValidException ex) {
        return ResponseEntity.badRequest().body(error("invalid_request", "Request validation failed"));
    }

    @ExceptionHandler(IllegalArgumentException.class)
    ResponseEntity<Map<String, Object>> badRequest(IllegalArgumentException ex) {
        return ResponseEntity.badRequest().body(error("invalid_request", ex.getMessage()));
    }

    @ExceptionHandler({DuplicateResourceException.class, DataIntegrityViolationException.class})
    ResponseEntity<Map<String, Object>> duplicate(RuntimeException ex) {
        return ResponseEntity.status(HttpStatus.CONFLICT)
                .body(error("duplicate_account", "An account with the supplied identity already exists"));
    }

    @ExceptionHandler({InvalidCredentialsException.class, BadCredentialsException.class})
    ResponseEntity<Map<String, Object>> unauthorized(RuntimeException ex) {
        return ResponseEntity.status(HttpStatus.UNAUTHORIZED)
                .body(error("invalid_authentication", "Invalid credentials"));
    }


    @ExceptionHandler(OtpServiceException.class)
    ResponseEntity<Map<String, Object>> otpService(OtpServiceException ex) {
        HttpStatus status = switch (ex.status()) {
            case 400 -> HttpStatus.BAD_REQUEST;
            case 429 -> HttpStatus.TOO_MANY_REQUESTS;
            default -> HttpStatus.BAD_GATEWAY;
        };
        String code = ex.status() == 400 ? "invalid_otp" : "otp_service_error";
        return ResponseEntity.status(status).body(error(code, ex.getMessage()));
    }

    @ExceptionHandler(AccessDeniedException.class)
    ResponseEntity<Map<String, Object>> forbidden(AccessDeniedException ex) {
        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(error("forbidden", "Insufficient permissions"));
    }

    @ExceptionHandler(ForbiddenOperationException.class)
    ResponseEntity<Map<String, Object>> forbiddenOperation(ForbiddenOperationException ex) {
        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(error("forbidden", "Insufficient permissions"));
    }

    private Map<String, Object> error(String code, String message) {
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("code", code);
        body.put("message", message);
        return body;
    }
}
