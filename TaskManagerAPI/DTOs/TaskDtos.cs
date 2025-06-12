// using System.ComponentModel.DataAnnotations;

// namespace TaskManagerAPI.DTOs
// {
//     public class CreateTaskDto
//     {
//         [Required]
//         [StringLength(200)]
//         public string Title { get; set; } = string.Empty;
        
//         public string? Description { get; set; }
        
//         public string Priority { get; set; } = "Medium";
        
//         public DateTime? DueDate { get; set; }
        
//         public int? AssignedToId { get; set; }
//     }
    
//     public class UpdateTaskDto
//     {
//         [StringLength(200)]
//         public string? Title { get; set; }
        
//         public string? Description { get; set; }
        
//         public string? Status { get; set; }
        
//         public string? Priority { get; set; }
        
//         public DateTime? DueDate { get; set; }
        
//         public int? AssignedToId { get; set; }
//     }
    
//     public class TaskDto
//     {
//         public int Id { get; set; }
//         public string Title { get; set; } = string.Empty;
//         public string? Description { get; set; }
//         public string Status { get; set; } = string.Empty;
//         public string Priority { get; set; } = string.Empty;
//         public DateTime CreatedAt { get; set; }
//         public DateTime UpdatedAt { get; set; }
//         public DateTime? DueDate { get; set; }
//         public int? AssignedToId { get; set; }
//         public string? AssignedToUsername { get; set; }
//         public int CreatedById { get; set; }
//         public string CreatedByUsername { get; set; } = string.Empty;
//     }
// }

