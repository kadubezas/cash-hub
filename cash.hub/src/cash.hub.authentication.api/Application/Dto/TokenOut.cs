namespace cash.hub.authentication.api.Application.Dto;

public record struct TokenOut(string Token, DateTime ExpiresAt)
{
    
}