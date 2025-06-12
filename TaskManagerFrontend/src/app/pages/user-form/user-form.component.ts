import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/auth.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  userId: number | null = null;
  loading = false;
  error = '';
  isEditMode = false;

  roles = ['User', 'Admin'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    public authService: AuthService
  ) {
    this.userForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required],
      password: [''], // Optional for edit, required for new
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.userId = +id;
        this.isEditMode = true;
        this.loadUser(this.userId);
      }
    });

    // If creating a new user, password is required
    if (!this.isEditMode) {
      this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
      this.userForm.get('password')?.updateValueAndValidity();
    }
  }

  loadUser(id: number): void {
    this.loading = true;
    this.userService.getUser(id).subscribe({
      next: (user) => {
        this.userForm.patchValue({
          email: user.email,
          role: user.role,
          // password is not loaded for security reasons
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load user: ' + (err.error?.message || err.message);
        this.loading = false;
        this.snackBar.open(this.error, 'Close', { duration: 5000 });
      }
    });
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.isEditMode && this.userId) {
      // Only send fields that have values
      const updateData: any = {};
      
      if (this.userForm.get('email')?.value) {
        updateData.email = this.userForm.get('email')?.value;
      }
      
      if (this.userForm.get('role')?.value) {
        updateData.role = this.userForm.get('role')?.value;
      }
      
      if (this.userForm.get('password')?.value) {
        updateData.password = this.userForm.get('password')?.value;
      }

      this.userService.updateUser(this.userId, updateData).subscribe({
        next: () => {
          this.snackBar.open('User updated successfully!', 'Close', { duration: 3000 });
          this.router.navigate(['/users']);
        },
        error: (err) => {
          this.error = 'Failed to update user: ' + (err.error?.message || err.message);
          this.loading = false;
          this.snackBar.open(this.error, 'Close', { duration: 5000 });
        }
      });
    } else {
      // This case should ideally be handled by register component, but adding for completeness
      // if a new user creation form is ever added for admin
      // Note: Backend register endpoint handles new user creation
      this.snackBar.open('New user creation is handled via the register endpoint.', 'Close', { duration: 5000 });
      this.loading = false;
    }
  }
}


