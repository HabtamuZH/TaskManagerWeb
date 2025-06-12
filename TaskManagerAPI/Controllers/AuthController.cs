using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        
        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }
                
                if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }
                
                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Role = registerDto.Role == "Admin" ? "Admin" : "User"
                };
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                // Generate token
                var token = _jwtService.GenerateToken(user);
                
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
                
                return Ok(new AuthResponseDto { Token = token, User = userDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Registration failed: {ex.Message}" });
            }
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
                
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                
                var token = _jwtService.GenerateToken(user);
                
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
                
                return Ok(new AuthResponseDto { Token = token, User = userDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Login failed: {ex.Message}" });
            }
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _context.Users.FindAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
                
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to get profile: {ex.Message}" });
            }
        }
        
        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateProfile(UpdateProfileDto updateDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _context.Users.FindAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                
                if (!string.IsNullOrEmpty(updateDto.Email))
                {
                    if (await _context.Users.AnyAsync(u => u.Email == updateDto.Email && u.Id != userId))
                    {
                        return BadRequest(new { message = "Email already exists" });
                    }
                    user.Email = updateDto.Email;
                }
                
                if (!string.IsNullOrEmpty(updateDto.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
                }
                
                await _context.SaveChangesAsync();
                
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
                
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to update profile: {ex.Message}" });
            }
        }
    }
}


