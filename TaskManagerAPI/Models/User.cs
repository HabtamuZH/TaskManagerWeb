// using System.ComponentModel.DataAnnotations;

// namespace TaskManagerAPI.Models
// {
//     public class User
//     {
//         public int Id { get; set; }
        
//         [Required]
//         [StringLength(50)]
//         public string Username { get; set; } = string.Empty;
        
//         [Required]
//         [EmailAddress]
//         [StringLength(100)]
//         public string Email { get; set; } = string.Empty;
        
//         [Required]
//         public string PasswordHash { get; set; } = string.Empty;
        
//         [Required]
//         [StringLength(20)]
//         public string Role { get; set; } = "User"; // "Admin" or "User"
        
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
//         // Navigation properties
//         public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
//         public virtual ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
//     }
// }

