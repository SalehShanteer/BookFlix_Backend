import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { LocalePipe } from '../../pipes/locale-pipe';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-navbar',
  imports: [LocalePipe, RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}
  signOut() {
    this.authService.logoutBackend().subscribe({
      next: () => {
        this.router.navigate(['login']);
      },
      error: () => {
        this.authService.forceLogout();
      },
    });
  }
}
