namespace TaskManagerAPI.DTOs
{
    // Auth DTOs
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }

    public class UpdateProfileDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    // Task DTOs
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedToId { get; set; }
        public string? AssignedToUsername { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Medium";
        public DateTime? DueDate { get; set; }
        public int? AssignedToId { get; set; }
    }

    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedToId { get; set; }
    }

    // User Management DTOs (for Admin)
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
    }
}

