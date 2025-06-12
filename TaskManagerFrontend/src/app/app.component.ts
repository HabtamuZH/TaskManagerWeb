import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from './services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Task Manager';

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  shouldShowNavbar(): boolean {
    const currentRoute = this.router.url;
    const authRoutes = ['/login', '/register'];
    return !authRoutes.includes(currentRoute) && this.authService.isAuthenticated();
  }
}

