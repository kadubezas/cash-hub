using cash.hub.authentication.api.Application.Common.Enums;

namespace cash.hub.authentication.api.Application.Common.Dto;

public class BaseReturnApplication<T>(bool success, T? data, ErrorType? errorType, string? message)
{
    public bool Success { get; set; } = success;
    public T? Data { get; set; } = data;
    public ErrorType? ErrorType { get; set; } = errorType;
    public string? Message { get; set; } = message;
};

