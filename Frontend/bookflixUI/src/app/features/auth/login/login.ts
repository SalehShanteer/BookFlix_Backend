import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ILogin } from '../../../core/models/auth/login.model';
import { AuthService } from '../../../core/services/auth-service';
import { PasswordField } from '../../../shared/components/password-field/password-field';
import { LocalePipe } from '../../../shared/pipes/locale-pipe';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, PasswordField, LocalePipe],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onLogin() {
    const loginRequest: ILogin = this.loginForm.value;
    this.authService.login(loginRequest).subscribe({
      next: () => {
        console.log('Login successful');
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Login failed:', err);
      },
    });
  }

  showRegisterScreen() {
    this.router.navigate(['register']);
  }
}
