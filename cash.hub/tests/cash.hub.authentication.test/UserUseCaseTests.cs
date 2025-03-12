using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using cash.hub.authentication.api.Application.UseCases.UserPort;
using cash.hub.authentication.api.Application.Services;
using cash.hub.authentication.api.Application.Common.FactoryBaseReturn;
using cash.hub.authentication.api.Domain.Entities;
using cash.hub.authentication.api.Domain.Ports;
using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;

namespace cash.hub.authentication.test;

public class UserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<UserUseCase>> _loggerMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IResponseFactory> _responseFactoryMock;
    private readonly UserUseCase _userUseCase;

    public UserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserUseCase>>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _responseFactoryMock = new Mock<IResponseFactory>();

        _userUseCase = new UserUseCase(
            _userRepositoryMock.Object,
            _loggerMock.Object,
            _passwordServiceMock.Object,
            _responseFactoryMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnError_WhenUserAlreadyExists()
    {
        // Arrange
        string username = "testuser";
        string password = "password123";

        _userRepositoryMock
            .Setup(repo => repo.UserExistsAsync(username))
            .ReturnsAsync(true);

        _responseFactoryMock
            .Setup(factory => factory.Error<bool>(It.IsAny<ErrorType>(), It.IsAny<string>()))
            .Returns(new BaseReturnApplication<bool>(false, default, ErrorType.Business,"User already exists"));

        // Act
        var result = await _userUseCase.RegisterAsync(username, password);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("User already exists");

        _userRepositoryMock.Verify(repo => repo.UserExistsAsync(username), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Never);
        _responseFactoryMock.Verify(factory => factory.Error<bool>(ErrorType.Business, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        // Arrange
        string username = "newuser";
        string password = "securePassword";
        string salt = "randomSalt";
        string passwordHash = "hashedPassword";

        _userRepositoryMock
            .Setup(repo => repo.UserExistsAsync(username))
            .ReturnsAsync(false);

        _passwordServiceMock
            .Setup(service => service.GenerateSalt())
            .Returns(salt);

        _passwordServiceMock
            .Setup(service => service.HashPassword(password, salt))
            .Returns(passwordHash);

        _responseFactoryMock
            .Setup(factory => factory.Success<bool>(true))
            .Returns(new BaseReturnApplication<bool>(true, true, null,"User created successfully"));

        // Act
        var result = await _userUseCase.RegisterAsync(username, password);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("User created successfully");

        _userRepositoryMock.Verify(repo => repo.UserExistsAsync(username), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u =>
            u.Username == username &&
            u.PasswordHash == passwordHash &&
            u.Salt == salt
        )), Times.Once);
        
        _responseFactoryMock.Verify(factory => factory.Success<bool>(true), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        string username = "errorUser";
        string password = "errorPass";

        _userRepositoryMock
            .Setup(repo => repo.UserExistsAsync(username))
            .ThrowsAsync(new Exception("Database connection failed"));

        _responseFactoryMock
            .Setup(factory => factory.Error<bool>(ErrorType.System, "Database connection failed"))
            .Returns(new BaseReturnApplication<bool>(false, false, ErrorType.System,"Database connection failed"));

        // Act
        var result = await _userUseCase.RegisterAsync(username, password);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Database connection failed");

        _userRepositoryMock.Verify(repo => repo.UserExistsAsync(username), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Never);
        
        _responseFactoryMock.Verify(factory => factory.Error<bool>(ErrorType.System, "Database connection failed"), Times.Once);
    }
}
