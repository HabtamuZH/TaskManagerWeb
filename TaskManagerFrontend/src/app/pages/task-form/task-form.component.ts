import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';
import { Task } from '../../models/task.model';
import { User } from '../../models/auth.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  taskId: number | null = null;
  users: User[] = [];
  loading = false;
  error = '';
  isEditMode = false;

  statuses = ['Pending', 'InProgress', 'Completed'];
  priorities = ['Low', 'Medium', 'High'];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    public authService: AuthService
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      status: ['Pending', Validators.required],
      priority: ['Medium', Validators.required],
      dueDate: [null],
      assignedToId: [null]
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.taskId = +id;
        this.isEditMode = true;
        this.loadTask(this.taskId);
      }
    });
    this.loadUsers();
  }

  loadTask(id: number): void {
    this.loading = true;
    this.taskService.getTask(id).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          status: task.status,
          priority: task.priority,
          dueDate: task.dueDate ? new Date(task.dueDate) : null,
          assignedToId: task.assignedToId
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load task: ' + (err.error?.message || err.message);
        this.loading = false;
        this.snackBar.open(this.error, 'Close', { duration: 5000 });
      }
    });
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
      },
      error: (err) => {
        console.error('Failed to load users for assignment:', err);
        this.snackBar.open('Failed to load users for assignment.', 'Close', { duration: 5000 });
      }
    });
  }

  onSubmit(): void {
    if (this.taskForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.isEditMode && this.taskId) {
      this.taskService.updateTask(this.taskId, this.taskForm.value).subscribe({
        next: () => {
          this.snackBar.open('Task updated successfully!', 'Close', { duration: 3000 });
          this.router.navigate(['/tasks']);
        },
        error: (err) => {
          this.error = 'Failed to update task: ' + (err.error?.message || err.message);
          this.loading = false;
          this.snackBar.open(this.error, 'Close', { duration: 5000 });
        }
      });
    } else {
      this.taskService.createTask(this.taskForm.value).subscribe({
        next: () => {
          this.snackBar.open('Task created successfully!', 'Close', { duration: 3000 });
          this.router.navigate(['/tasks']);
        },
        error: (err) => {
          this.error = 'Failed to create task: ' + (err.error?.message || err.message);
          this.loading = false;
          this.snackBar.open(this.error, 'Close', { duration: 5000 });
        }
      });
    }
  }
}


