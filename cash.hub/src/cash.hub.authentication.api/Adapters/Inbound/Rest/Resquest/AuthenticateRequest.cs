namespace cash.hub.authentication.api.Adapters.Inbound.Rest.Resquest;

public record struct AuthenticateRequest(string UserName, string Password);