import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';
import { Task } from '../../models/task.model';
import { User } from '../../models/auth.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  currentUser: User | null = null;
  tasks: Task[] = [];
  users: User[] = [];
  loading = true;
  error = '';

  // Statistics
  totalTasks = 0;
  pendingTasks = 0;
  inProgressTasks = 0;
  completedTasks = 0;
  totalUsers = 0;

  displayedColumns = ['title', 'status', 'priority', 'dueDate'];

  constructor(
    public authService: AuthService,
    private taskService: TaskService,
    private userService: UserService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
      if (user) {
        this.loadData();
      }
    });
  }

  loadData() {
    this.loading = true;
    this.error = '';

    // Load tasks
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.calculateTaskStatistics();
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load tasks';
        this.loading = false;
      },
    });

    // Load users if admin
    if (this.isAdmin()) {
      this.userService.getUsers().subscribe({
        next: (users) => {
          this.users = users;
          this.totalUsers = users.length;
        },
        error: (error) => {
          console.error('Failed to load users:', error);
        },
      });
    }
  }

  refreshData() {
    this.loadData();
  }

  calculateTaskStatistics() {
    this.totalTasks = this.tasks.length;
    this.pendingTasks = this.tasks.filter((t) => t.status === 'Pending').length;
    this.inProgressTasks = this.tasks.filter((t) => t.status === 'InProgress').length;
    this.completedTasks = this.tasks.filter((t) => t.status === 'Completed').length;
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  navigateToTasks(filter: string) {
    this.router.navigate(['/tasks'], { queryParams: { status: filter } });
  }

  navigateToUsers() {
    this.router.navigate(['users']);
  }

  navigateToProfile() {
    this.router.navigate(['profile']);
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending':
        return 'status-pending';
      case 'InProgress':
        return 'status-in-progress';
      case 'Completed':
        return 'status-completed';
      default:
        return '';
    }
  }

  getPriorityClass(priority: string): string {
    switch (priority) {
      case 'Low':
        return 'priority-low';
      case 'Medium':
        return 'priority-medium';
      case 'High':
        return 'priority-high';
      default:
        return '';
    }
  }
}
