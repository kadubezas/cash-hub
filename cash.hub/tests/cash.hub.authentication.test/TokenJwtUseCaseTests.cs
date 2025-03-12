using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;
using cash.hub.authentication.api.Application.Common.FactoryBaseReturn;
using cash.hub.authentication.api.Application.Dto;
using cash.hub.authentication.api.Application.Services;
using cash.hub.authentication.api.Application.UseCases.TokenJwt;
using cash.hub.authentication.api.Domain.Entities;
using cash.hub.authentication.api.Domain.Ports;
using FluentAssertions;
using Moq;

namespace cash.hub.authentication.test;

public class TokenJwtUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IResponseFactory> _responseFactoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IGenerateTokenService> _generateTokenServiceMock;
    private readonly TokenJwtUseCase _tokenJwtUseCase;

    public TokenJwtUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _responseFactoryMock = new Mock<IResponseFactory>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _generateTokenServiceMock = new Mock<IGenerateTokenService>();

        _tokenJwtUseCase = new TokenJwtUseCase(
            _userRepositoryMock.Object,
            _responseFactoryMock.Object,
            _passwordServiceMock.Object,
            _generateTokenServiceMock.Object
        );
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        string userName = "testuser";
        string password = "password123";

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameAsync(userName))
            .ReturnsAsync((User?)null); // Simula usuário inexistente

        _responseFactoryMock
            .Setup(factory => factory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos"))
            .Returns(new BaseReturnApplication<TokenOut>(false, default, ErrorType.Business,"Usuário ou senha inválidos"));

        // Act
        var result = await _tokenJwtUseCase.GenerateJwtTokenAsync(userName, password);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Usuário ou senha inválidos");

        _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync(userName), Times.Once);
        _passwordServiceMock.Verify(service => service.HashPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _generateTokenServiceMock.Verify(service => service.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<DateTime>.IsAny), Times.Never);
        _responseFactoryMock.Verify(factory => factory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos"), Times.Once);
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        // Arrange
        string userName = "testuser";
        string password = "wrongPassword";
        string correctPasswordHash = "hashedPassword";
        string salt = "randomSalt";

        var user = new User
        {
            Id = 1,
            Username = userName,
            PasswordHash = correctPasswordHash,
            Salt = salt
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameAsync(userName))
            .ReturnsAsync(user);

        _passwordServiceMock
            .Setup(service => service.HashPassword(password, salt))
            .Returns("incorrectHash");

        _responseFactoryMock
            .Setup(factory => factory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos"))
            .Returns(new BaseReturnApplication<TokenOut>(false, default, ErrorType.Business,"Usuário ou senha inválidos"));

        // Act
        var result = await _tokenJwtUseCase.GenerateJwtTokenAsync(userName, password);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Usuário ou senha inválidos");

        _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync(userName), Times.Once);
        _passwordServiceMock.Verify(service => service.HashPassword(password, salt), Times.Once);
        _generateTokenServiceMock.Verify(service => service.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<DateTime>.IsAny), Times.Never);
        _responseFactoryMock.Verify(factory => factory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos"), Times.Once);
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        string userName = "validuser";
        string password = "validPassword";
        string passwordHash = "hashedPassword";
        string salt = "randomSalt";
        string jwtToken = "mocked.jwt.token";
        DateTime expiration = DateTime.UtcNow.AddMinutes(60);

        var user = new User
        {
            Id = 1,
            Username = userName,
            PasswordHash = passwordHash,
            Salt = salt
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameAsync(userName))
            .ReturnsAsync(user);

        _passwordServiceMock
            .Setup(service => service.HashPassword(password, salt))
            .Returns(passwordHash);

        _generateTokenServiceMock
            .Setup(service => service.GenerateToken(user.Id.ToString(), user.Username, out expiration))
            .Returns(jwtToken);

        _responseFactoryMock
            .Setup(factory => factory.Success(It.IsAny<TokenOut>()))
            .Returns((TokenOut tokenOut) => new BaseReturnApplication<TokenOut>(true, tokenOut, null, "Token gerado com sucesso"));

        // Act
        var result = await _tokenJwtUseCase.GenerateJwtTokenAsync(userName, password);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Token.Should().Be(jwtToken);
        result.Data.ExpiresAt.Should().Be(expiration);
        result.Message.Should().Be("Token gerado com sucesso");

        _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync(userName), Times.Once);
        _passwordServiceMock.Verify(service => service.HashPassword(password, salt), Times.Once);
        _generateTokenServiceMock.Verify(service => service.GenerateToken(user.Id.ToString(), user.Username, out expiration), Times.Once);
        _responseFactoryMock.Verify(factory => factory.Success(It.IsAny<TokenOut>()), Times.Once);
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        string userName = "errorUser";
        string password = "errorPass";

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameAsync(userName))
            .ThrowsAsync(new Exception("Erro inesperado"));

        _responseFactoryMock
            .Setup(factory => factory.Error<TokenOut>(ErrorType.System, "Erro inesperado"))
            .Returns(new BaseReturnApplication<TokenOut>(false, default, ErrorType.System,"Erro inesperado"));

        // Act
        var result = await _tokenJwtUseCase.GenerateJwtTokenAsync(userName, password);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Erro inesperado");

        _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync(userName), Times.Once);
        _passwordServiceMock.Verify(service => service.HashPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _generateTokenServiceMock.Verify(service => service.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<DateTime>.IsAny), Times.Never);
        _responseFactoryMock.Verify(factory => factory.Error<TokenOut>(ErrorType.System, "Erro inesperado"), Times.Once);
    }
}