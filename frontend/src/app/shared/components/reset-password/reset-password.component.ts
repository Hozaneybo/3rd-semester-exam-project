import { Component, OnInit } from '@angular/core';
import { AccountServiceService } from "../../services/account-service.service";
import { ActivatedRoute, Router } from "@angular/router";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastController } from '@ionic/angular';
import {CustomValidators} from "../../CustomValidators";
import {ResponseDto} from "../../Models/LoginModels";


@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm: FormGroup;
  token: string;

  constructor(
    private service: AccountServiceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastController: ToastController // Inject ToastController
  ) {
    this.token = this.route.snapshot.queryParams['token'];
    this.resetPasswordForm = new FormGroup({
      newPassword: new FormControl('', [Validators.required]),
      confirmPassword: new FormControl('', [Validators.required, CustomValidators.matchOther('newPassword')])
    });
  }

  ngOnInit() {}

  async resetPassword() {
    if (this.resetPasswordForm.invalid) {
      await this.showToast("Please fill in the form correctly");
      return;
    }

    const { newPassword } = this.resetPasswordForm.value;

    this.service.resetPassword(this.token, newPassword).subscribe(
      async (response: ResponseDto<any>) => {
        await this.showToast(response.messageToClient || "Password reset successful");
        this.router.navigate(['/login']);
      },
      async (error) => {
        await this.showToast("Error resetting password");
      }
    );
  }

  private async showToast(message: string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 5000
    });
    toast.present();
  }
}
