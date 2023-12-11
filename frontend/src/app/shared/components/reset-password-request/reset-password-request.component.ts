import { Component} from '@angular/core';
import { FormControl, Validators} from "@angular/forms";
import {AccountServiceService} from "../../services/account-service.service";
import {ResponseDto} from "../../../admin/components/LoginModels";
import {ToastService} from "../../services/toast.service";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password-request.component.html',
  styleUrls: ['./reset-password-request.component.scss'],
})
export class ResetPasswordRequestComponent  {


  email = new FormControl('',[Validators.required, Validators.email]);
  constructor( private service: AccountServiceService, private toastService : ToastService) { }


  async requestReset() {
    if (this.email.valid) {
      this.service.requestResetPassword(this.email.value).subscribe(
        async (response: ResponseDto<any>) => {
          this.toastService.showSuccess(response.messageToClient || 'If an account with that email exists, a password reset link has been sent.')
        },
        async (error: any) => {
          this.toastService.showError(error.error.messageToClient || 'There was an error processing your request.')
        }
      );
    }
  }
}
