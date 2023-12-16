import { Component, OnInit } from '@angular/core';
import {AccountServiceService} from "../../services/account-service.service";
import {ActivatedRoute} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {ToastService} from "../../services/toast.service";


@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss'],
})
export class VerifyEmailComponent  implements OnInit {
  emailVerified = false;
  message = '';

  constructor(private service: AccountServiceService,
              private route: ActivatedRoute,
              private toastService : ToastService) {
  }

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.service.verifyEmail(token).subscribe({
        next: (response) => {
          this.toastService.showSuccess('Email verification successful!');
          this.emailVerified = true;
          this.message = 'Your email has been successfully verified!';
        },
        error: (error) => {
          this.toastService.showError(error.error.messageToClient || 'Email verification failed!');
          this.message = 'Failed to verify email. Please try again.';
        }
      });
    }
  }
}
