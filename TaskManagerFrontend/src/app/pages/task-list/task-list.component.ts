import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  loading = true;
  error = '';
  displayedColumns: string[] = ['title', 'status', 'priority', 'dueDate', 'assignedTo', 'createdBy', 'actions'];

  constructor(
    private taskService: TaskService,
    private snackBar: MatSnackBar,
    private router: Router,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading = true;
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load tasks: ' + (err.error?.message || err.message);
        this.loading = false;
        this.snackBar.open(this.error, 'Close', { duration: 5000 });
      }
    });
  }

  editTask(id: number): void {
    this.router.navigate(['/tasks/edit', id]);
  }

  deleteTask(id: number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.snackBar.open('Task deleted successfully!', 'Close', { duration: 3000 });
          this.loadTasks(); // Reload tasks after deletion
        },
        error: (err) => {
          this.error = 'Failed to delete task: ' + (err.error?.message || err.message);
          this.snackBar.open(this.error, 'Close', { duration: 5000 });
        }
      });
    }
  }

  // Helper to get status class for styling
  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'InProgress': return 'status-in-progress';
      case 'Completed': return 'status-completed';
      default: return '';
    }
  }

  // Helper to get priority class for styling
  getPriorityClass(priority: string): string {
    switch (priority) {
      case 'Low': return 'priority-low';
      case 'Medium': return 'priority-medium';
      case 'High': return 'priority-high';
      default: return '';
    }
  }

  // Navigate to create new task page
  createNewTask(): void {
    this.router.navigate(['/tasks/new']);
  }

  // Check if current user is admin
  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  // Check if task was created by current user
  isCreatedByCurrentUser(task: Task): boolean {
    return this.authService.currentUserValue?.id === task.createdById;
  }
}


