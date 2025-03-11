namespace cash.hub.authentication.api.Adapters.Inbound.Rest.Response;

public record struct AuthenticateResponse(string Token, DateTime Expiration);