import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/auth.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  loading = true;
  error = '';
  displayedColumns: string[] = ['username', 'email', 'role', 'createdAt', 'actions'];

  constructor(
    private userService: UserService,
    private snackBar: MatSnackBar,
    private router: Router,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load users: ' + (err.error?.message || err.message);
        this.loading = false;
        this.snackBar.open(this.error, 'Close', { duration: 5000 });
      }
    });
  }

  editUser(id: number): void {
    this.router.navigate(['/users/edit', id]);
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(id).subscribe({
        next: () => {
          this.snackBar.open('User deleted successfully!', 'Close', { duration: 3000 });
          this.loadUsers(); // Reload users after deletion
        },
        error: (err) => {
          this.error = 'Failed to delete user: ' + (err.error?.message || err.message);
          this.snackBar.open(this.error, 'Close', { duration: 5000 });
        }
      });
    }
  }
}


