using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Common;
using TaskManagement.API.Controllers;
using TaskManagement.API.DTOs.Auth;
using TaskManagement.API.Services.Interfaces;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123" };
            var authResponse = new AuthResponseDto { Token = "jwt_token", UserId = Guid.NewGuid() };

            _mockAuthService.Setup(x => x.RegisterAsync(registerDto))
                .ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<AuthResponseDto>>().Subject;
            apiResponse.Data.Token.Should().Be("jwt_token");
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "Password123" };
            var authResponse = new AuthResponseDto { Token = "jwt_token", UserId = Guid.NewGuid() };

            _mockAuthService.Setup(x => x.LoginAsync(loginDto))
                .ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<ApiResponse<AuthResponseDto>>().Subject;
            apiResponse.Data.Token.Should().Be("jwt_token");
        }
    }
}
