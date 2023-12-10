import { Component } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { ToastController } from "@ionic/angular";
import { AccountServiceService } from "../../services/account-service.service";
import { Router } from "@angular/router";
import {firstValueFrom} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {

  constructor(
    private fb: FormBuilder,
    private accountService: AccountServiceService,
    private toastController: ToastController,
    private router: Router
  ) {}

  readonly loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  async submit() {
    if (this.loginForm.valid) {
      try {
        const response = await firstValueFrom(
          this.accountService.login(
            this.loginForm.value.email as string,
            this.loginForm.value.password as string
          )
        );
        this.redirectUser(response.responseData.role);
      } catch (error) {
        if (error instanceof HttpErrorResponse) {
          const errorMsg = error.error?.message || 'username or password is incorrect';
          this.showToast(errorMsg, 'danger');
        }
      }
    } else {
      this.showToast('Please fill in all required fields correctly.', 'danger');
    }
  }

  private redirectUser(role: string) {
    switch(role) {
      case 'Admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'Teacher':
        this.router.navigate(['/teacher/dashboard']);
        break;
      case 'Student':
        this.router.navigate(['/student/dashboard']);
        break;
      default:
        this.showToast('Unknown user role', 'danger');
        break;
    }
  }

  private async showToast(message: string, color: string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
      color: color
    });
    toast.present();
  }
}
