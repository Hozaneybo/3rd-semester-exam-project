import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {CustomValidators} from "../../CustomValidators";
import {AccountServiceService} from "../../services/account-service.service";
import {ToastService} from "../../services/toast.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent   {


  readonly form = this.fb.group({
    fullName: [null, Validators.required],
    email: [null, [Validators.required, Validators.email]],
    password: [null, [Validators.required, Validators.minLength(8)]],
    passwordRepeat: [null, [Validators.required, CustomValidators.matchOther('password')]],
    avatarUrl: [null],
  });

  constructor(private fb: FormBuilder,
              private service : AccountServiceService,
              private toastService : ToastService,
              private router : Router
  ) { }

  async submit() {
    if (this.form.valid) {
      try {
        const response = await firstValueFrom(this.service.register(this.form.value));
        if(response) {
          this.toastService.showSuccess(response.messageToClient || 'Registration successful');
          this.router.navigate(['/login'])
        }
      } catch (e) {
        if (e instanceof HttpErrorResponse) {
          this.toastService.showError(e.error.messageToClient || 'Registration failed');
        } else {
          this.toastService.showError('An unexpected error occurred');
        }
      }
    } else {
      this.toastService.showWarning('Please fill in all required fields correctly.');
    }
  }
}
