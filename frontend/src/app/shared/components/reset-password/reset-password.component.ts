import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { HttpErrorResponse } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { ToastController } from '@ionic/angular';
import { firstValueFrom } from "rxjs";

import { CustomValidators } from "../../CustomValidators";
import { AccountServiceService } from "../../services/account-service.service";
import { ResponseDto } from "../../../admin/components/LoginModels";
import {ToastService} from "../../services/toast.service";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent  {

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
    private toastService : ToastService
  ) {
    this.token = this.route.snapshot.queryParams['token'];
  }



  async resetPassword() {
    if (this.resetPasswordForm.invalid) {
      await this.toastService.showError("Please fill in the form correctly");
      return;
    }

    try {
      const response: ResponseDto<any> = await firstValueFrom(this.service.resetPassword(this.token, this.resetPasswordForm.value.newPassword));
      await this.toastService.showSuccess(response.messageToClient || "Password reset successful");
      this.router.navigate(['/login']);
    } catch (error) {
      const errorMessage = (error as HttpErrorResponse).error.messageToClient || "Error resetting password";
      await this.toastService.showError(errorMessage);
    }
  }

}
