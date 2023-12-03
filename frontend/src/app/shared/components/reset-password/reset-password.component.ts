import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { HttpErrorResponse } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { ToastController } from '@ionic/angular';
import { firstValueFrom } from "rxjs";

import { CustomValidators } from "../../CustomValidators";
import { AccountServiceService } from "../../services/account-service.service";
import { ResponseDto } from "../../Models/LoginModels";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm = this.fb.group({
    newPassword: ['', Validators.required],
    confirmPassword: ['', [Validators.required, CustomValidators.matchOther('newPassword')]]
  });

  token: string;

  constructor(
    private fb: FormBuilder,
    private service: AccountServiceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastController: ToastController
  ) {
    this.token = this.route.snapshot.queryParams['token'];
  }

  ngOnInit() {}

  async resetPassword() {
    if (this.resetPasswordForm.invalid) {
      await this.presentToast("Please fill in the form correctly", "danger");
      return;
    }

    try {
      const response: ResponseDto<any> = await firstValueFrom(this.service.resetPassword(this.token, this.resetPasswordForm.value.newPassword));
      await this.presentToast(response.messageToClient || "Password reset successful", "success");
      this.router.navigate(['/login']);
    } catch (error) {
      const errorMessage = (error as HttpErrorResponse).error.messageToClient || "Error resetting password";
      await this.presentToast(errorMessage, "danger");
    }
  }

  private async presentToast(message: string, color: "success" | "danger") {
    const toast = await this.toastController.create({
      message: message,
      color: color,
      duration: 5000
    });
    toast.present();
  }
}
