using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                IQueryable<TaskItem> query = _context.Tasks
                    .Include(t => t.AssignedTo)
                    .Include(t => t.CreatedBy);
                
                // If user is not admin, only show their assigned tasks or tasks they created
                if (userRole != "Admin")
                {
                    query = query.Where(t => t.AssignedToId == userId || t.CreatedById == userId);
                }
                
                var tasks = await query.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    DueDate = t.DueDate,
                    AssignedToId = t.AssignedToId,
                    AssignedToUsername = t.AssignedTo != null ? t.AssignedTo.Username : null,
                    CreatedById = t.CreatedById,
                    CreatedByUsername = t.CreatedBy.Username
                }).ToListAsync();
                
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to get tasks: {ex.Message}" });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                var task = await _context.Tasks
                    .Include(t => t.AssignedTo)
                    .Include(t => t.CreatedBy)
                    .FirstOrDefaultAsync(t => t.Id == id);
                
                if (task == null)
                {
                    return NotFound(new { message = "Task not found" });
                }
                
                // Check if user has access to this task
                if (userRole != "Admin" && task.AssignedToId != userId && task.CreatedById != userId)
                {
                    return Forbid();
                }
                
                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    DueDate = task.DueDate,
                    AssignedToId = task.AssignedToId,
                    AssignedToUsername = task.AssignedTo?.Username,
                    CreatedById = task.CreatedById,
                    CreatedByUsername = task.CreatedBy.Username
                };
                
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to get task: {ex.Message}" });
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                // Validate assigned user exists if provided
                if (createTaskDto.AssignedToId.HasValue)
                {
                    var assignedUser = await _context.Users.FindAsync(createTaskDto.AssignedToId.Value);
                    if (assignedUser == null)
                    {
                        return BadRequest(new { message = "Assigned user not found" });
                    }
                }
                
                var task = new TaskItem
                {
                    Title = createTaskDto.Title,
                    Description = createTaskDto.Description,
                    Priority = createTaskDto.Priority,
                    DueDate = createTaskDto.DueDate,
                    AssignedToId = createTaskDto.AssignedToId,
                    CreatedById = userId
                };
                
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                
                // Reload task with navigation properties
                task = await _context.Tasks
                    .Include(t => t.AssignedTo)
                    .Include(t => t.CreatedBy)
                    .FirstAsync(t => t.Id == task.Id);
                
                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    DueDate = task.DueDate,
                    AssignedToId = task.AssignedToId,
                    AssignedToUsername = task.AssignedTo?.Username,
                    CreatedById = task.CreatedById,
                    CreatedByUsername = task.CreatedBy.Username
                };
                
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to create task: {ex.Message}" });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(int id, UpdateTaskDto updateTaskDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                var task = await _context.Tasks
                    .Include(t => t.AssignedTo)
                    .Include(t => t.CreatedBy)
                    .FirstOrDefaultAsync(t => t.Id == id);
                
                if (task == null)
                {
                    return NotFound(new { message = "Task not found" });
                }
                
                // Check permissions: Admin can update any task, users can only update their assigned tasks
                if (userRole != "Admin" && task.AssignedToId != userId)
                {
                    return Forbid();
                }
                
                // Update fields
                if (!string.IsNullOrEmpty(updateTaskDto.Title))
                    task.Title = updateTaskDto.Title;
                
                if (updateTaskDto.Description != null)
                    task.Description = updateTaskDto.Description;
                
                if (!string.IsNullOrEmpty(updateTaskDto.Status))
                    task.Status = updateTaskDto.Status;
                
                if (!string.IsNullOrEmpty(updateTaskDto.Priority))
                    task.Priority = updateTaskDto.Priority;
                
                if (updateTaskDto.DueDate.HasValue)
                    task.DueDate = updateTaskDto.DueDate;
                
                // Only admin can reassign tasks
                if (userRole == "Admin" && updateTaskDto.AssignedToId.HasValue)
                {
                    var assignedUser = await _context.Users.FindAsync(updateTaskDto.AssignedToId.Value);
                    if (assignedUser == null)
                    {
                        return BadRequest(new { message = "Assigned user not found" });
                    }
                    task.AssignedToId = updateTaskDto.AssignedToId;
                }
                
                task.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    DueDate = task.DueDate,
                    AssignedToId = task.AssignedToId,
                    AssignedToUsername = task.AssignedTo?.Username,
                    CreatedById = task.CreatedById,
                    CreatedByUsername = task.CreatedBy.Username
                };
                
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to update task: {ex.Message}" });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                
                if (task == null)
                {
                    return NotFound(new { message = "Task not found" });
                }
                
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                
                return Ok(new { message = "Task deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to delete task: {ex.Message}" });
            }
        }
    }
}


