using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore] // Don't serialize password hash
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User"; // Default role

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for tasks created by this user
        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();

        // Navigation property for tasks assigned to this user
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // e.g., Pending, InProgress, Completed

        [Required]
        [MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // e.g., Low, Medium, High

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        // Foreign key for the user to whom the task is assigned
        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        // Foreign key for the user who created the task
        [Required]
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
    }
}

