namespace cash.hub.register.api.Adapters.Inbound.Rest.Responses;

public record struct ErrorResponse
{
    public int Code { get; set; }
    public string Message { get; set; }
    public ICollection<Error>? Errors { get; set; }
}

public record struct Error(string Field, string Message);