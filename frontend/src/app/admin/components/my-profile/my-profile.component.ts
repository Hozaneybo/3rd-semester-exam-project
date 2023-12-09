import { Component, OnInit } from '@angular/core';
import {ResponseDto, User, UserProfile} from "../LoginModels";
import {AccountServiceService} from "../../../shared/services/account-service.service";
import {ToastController} from "@ionic/angular";
import {catchError} from "rxjs/operators";
import {of} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss'],
})
export class MyProfileComponent  implements OnInit {

  userProfile: UserProfile | undefined;

  constructor(
    private accountService: AccountServiceService,
    private toastController: ToastController,
    private router : Router) {
  }

  ngOnInit() {
    this.loadUserProfile();
  }

  loadUserProfile() {
    this.accountService.whoAmI().pipe(
      catchError(err => {
        this.presentToast('An error occurred while loading your profile.');
        return of({} as ResponseDto<User>);
      })
    ).subscribe(response => {
      if (response && response.responseData) {
        this.userProfile = response.responseData;
      } else {
        this.presentToast(response.messageToClient || 'No profile data available.');
      }
    }, error => {
      this.presentToast(error.error.messageToClient || 'An unexpected error occurred.');
    });
  }

  async presentToast(message: string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
    });
    toast.present();
  }

  editProfile(){}
}