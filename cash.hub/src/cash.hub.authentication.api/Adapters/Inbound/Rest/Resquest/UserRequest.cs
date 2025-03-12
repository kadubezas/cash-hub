namespace cash.hub.authentication.api.Adapters.Inbound.Rest.Resquest;

public record struct UserRequest(string UserName, string Password);