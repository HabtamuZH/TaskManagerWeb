// using System.ComponentModel.DataAnnotations;

// namespace TaskManagerAPI.DTOs
// {
//     public class LoginDto
//     {
//         [Required]
//         public string Username { get; set; } = string.Empty;
        
//         [Required]
//         public string Password { get; set; } = string.Empty;
//     }
    
//     public class RegisterDto
//     {
//         [Required]
//         [StringLength(50)]
//         public string Username { get; set; } = string.Empty;
        
//         [Required]
//         [EmailAddress]
//         public string Email { get; set; } = string.Empty;
        
//         [Required]
//         [MinLength(6)]
//         public string Password { get; set; } = string.Empty;
        
//         public string Role { get; set; } = "User";
//     }
    
//     public class UserDto
//     {
//         public int Id { get; set; }
//         public string Username { get; set; } = string.Empty;
//         public string Email { get; set; } = string.Empty;
//         public string Role { get; set; } = string.Empty;
//         public DateTime CreatedAt { get; set; }
//     }
    
//     public class AuthResponseDto
//     {
//         public string Token { get; set; } = string.Empty;
//         public UserDto User { get; set; } = null!;
//     }
// }

