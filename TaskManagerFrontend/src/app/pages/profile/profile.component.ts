import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { User } from '../../models/auth.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  currentUser: User | null = null;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.profileForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      confirmPassword: ['']
    }, { validator: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user) {
        this.profileForm.patchValue({
          email: user.email
        });
      }
    });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { 'mismatch': true };
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    const updateData: any = {};
    if (this.profileForm.get('email')?.dirty) {
      updateData.email = this.profileForm.get('email')?.value;
    }
    if (this.profileForm.get('password')?.value) {
      updateData.password = this.profileForm.get('password')?.value;
    }

    if (Object.keys(updateData).length === 0) {
      this.snackBar.open('No changes to save.', 'Close', { duration: 3000 });
      this.loading = false;
      return;
    }

    this.authService.updateProfile(updateData).subscribe({
      next: (user) => {
        this.snackBar.open('Profile updated successfully!', 'Close', { duration: 3000 });
        // Optionally navigate or refresh user data
        this.profileForm.get('password')?.reset();
        this.profileForm.get('confirmPassword')?.reset();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to update profile: ' + (err.error?.message || err.message);
        this.loading = false;
        this.snackBar.open(this.error, 'Close', { duration: 5000 });
      }
    });
  }
}


