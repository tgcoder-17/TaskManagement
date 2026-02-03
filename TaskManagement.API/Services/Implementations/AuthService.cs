using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Common.Exceptions;
using TaskManagement.API.DTOs.Auth;
using TaskManagement.API.Helpers;
using TaskManagement.API.Models;
using TaskManagement.API.Repositories.Interfaces;
using TaskManagement.API.Services.Interfaces;

namespace TaskManagement.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(
            IUserRepository userRepository,
            JwtTokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new BadRequestException(
                        "Validation failed",
                        new List<string> { "Email already exists" });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateToken(user, out var expiresAt);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new BadRequestException(
                        "Validation failed",
                        new List<string> { "Invalid credentials" }); 

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException(
                        "Validation failed",
                        new List<string> { "Invalid credentials" });

            var token = _tokenService.GenerateToken(user, out var expiresAt);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}
