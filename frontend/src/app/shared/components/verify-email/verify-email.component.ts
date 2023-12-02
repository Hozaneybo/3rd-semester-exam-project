import { Component, OnInit } from '@angular/core';
import {AccountServiceService} from "../../services/account-service.service";
import {ActivatedRoute} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss'],
})
export class VerifyEmailComponent  implements OnInit {
  emailVerified = false;
  message = '';

  constructor(private service: AccountServiceService, private route: ActivatedRoute, private toast: ToastController) {
  }

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.service.verifyEmail(token).subscribe(
        async response => {
          const successToast = await this.toast.create({
            message: 'Email verification successful!',
            color: "success",
            duration: 5000
          });
          successToast.present();
          this.emailVerified = true;
          this.message = 'Your email has been successfully verified!';
        },
        async error => {
          const errorToast = await this.toast.create({
            message: error.error.messageToClient || 'Email verification failed!',
            color: "danger",
            duration: 5000
          });
          errorToast.present();
          this.message = 'Failed to verify email. Please try again.'; // Update message on failure
        }
      );
    }
  }

}
