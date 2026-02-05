using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.API.Common.Exceptions;
using TaskManagement.API.DTOs.Auth;
using TaskManagement.API.Helpers;
using TaskManagement.API.Models;
using TaskManagement.API.Repositories.Interfaces;
using TaskManagement.API.Services.Implementations;
using Xunit;

namespace TaskManagement.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            // Use real IConfiguration via ConfigurationBuilder
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:SecretKey", "super_secret_key_for_testing_only_12345"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:ExpirationMinutes", "60"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var realTokenService = new JwtTokenService(configuration);

            _authService = new AuthService(
                _mockUserRepo.Object,
                realTokenService,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnAuthResponse_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123", FullName = "Test User" };
            var user = new User { Id = Guid.NewGuid(), Email = registerDto.Email, FullName = registerDto.FullName };

            _mockUserRepo.Setup(x => x.EmailExistsAsync(registerDto.Email)).ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<User>(registerDto)).Returns(user);
            _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowBadRequestException_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "existing@example.com", Password = "Password123" };
            _mockUserRepo.Setup(x => x.EmailExistsAsync(registerDto.Email)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _authService.RegisterAsync(registerDto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Validation failed");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAuthResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "Password123" };
            var user = new User { Id = Guid.NewGuid(), Email = loginDto.Email };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, loginDto.Password);

            _mockUserRepo.Setup(x => x.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowBadRequestException_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "nonexistent@example.com", Password = "Password123" };
            _mockUserRepo.Setup(x => x.GetByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _authService.LoginAsync(loginDto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowBadRequestException_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "WrongPassword" };
            var user = new User { Id = Guid.NewGuid(), Email = loginDto.Email };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "CorrectPassword");

            _mockUserRepo.Setup(x => x.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _authService.LoginAsync(loginDto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }
    }
}
