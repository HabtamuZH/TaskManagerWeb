using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        Role = u.Role,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();
                
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to get users: {ex.Message}" });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                
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
                return StatusCode(500, new { message = $"Failed to get user: {ex.Message}" });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                
                if (!string.IsNullOrEmpty(updateUserDto.Email))
                {
                    if (await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != id))
                    {
                        return BadRequest(new { message = "Email already exists" });
                    }
                    user.Email = updateUserDto.Email;
                }
                
                if (!string.IsNullOrEmpty(updateUserDto.Role))
                {
                    user.Role = updateUserDto.Role;
                }
                
                if (!string.IsNullOrEmpty(updateUserDto.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
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
                return StatusCode(500, new { message = $"Failed to update user: {ex.Message}" });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                
                // Don\"t allow deleting the last admin
                if (user.Role == "Admin")
                {
                    var adminCount = await _context.Users.CountAsync(u => u.Role == "Admin");
                    if (adminCount <= 1)
                    {
                        return BadRequest(new { message = "Cannot delete the last admin user" });
                    }
                }
                
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to delete user: {ex.Message}" });
            }
        }
    }
}


