import { Component, OnInit } from '@angular/core';
import { FormControl, Validators} from "@angular/forms";
import {AccountServiceService} from "../../services/account-service.service";
import {ToastController} from "@ionic/angular";
import {ResponseDto} from "../../../admin/components/LoginModels";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password-request.component.html',
  styleUrls: ['./reset-password-request.component.scss'],
})
export class ResetPasswordRequestComponent implements OnInit {


  email = new FormControl('',[Validators.required, Validators.email]);
  constructor( private service: AccountServiceService, private toast : ToastController) { }

  ngOnInit() { }

  async requestReset() {
    if (this.email.valid) {
      this.service.requestResetPassword(this.email.value).subscribe(
        async (response: ResponseDto<any>) => {
          const toast = await this.toast.create({
            message: response.messageToClient || 'If an account with that email exists, a password reset link has been sent.',
            color: 'success',
            duration: 5000
          });
          toast.present();
        },
        async (error: any) => {
          const errorMessage = error.error.messageToClient || 'There was an error processing your request.';
          const toast = await this.toast.create({
            message: errorMessage,
            color: 'danger',
            duration: 5000
          });
          toast.present();
        }
      );
    }
  }
}
