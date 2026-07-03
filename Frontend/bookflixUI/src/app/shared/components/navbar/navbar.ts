import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { LocalePipe } from '../../pipes/locale-pipe';
import { TokenHelper } from '../../helpers/token-helper';
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
        // 2. Once cookies are deleted, bounce the user to the login page
        this.router.navigate(['login']);
      },
      error: () => {
        // Fallback safety route if the network drops during logout
        this.authService.forceLogout();
      },
    });
  }
}
