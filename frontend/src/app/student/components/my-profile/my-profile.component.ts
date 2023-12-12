import { Component, OnInit } from '@angular/core';
import {ResponseDto, User, UserProfile} from "../../../shared/Models/LoginModels";
import {ToastController} from "@ionic/angular";
import {catchError} from "rxjs/operators";
import {of} from "rxjs";
import {AccountServiceService} from "../../../shared/services/account-service.service";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss'],
})
export class MyProfileComponent  implements OnInit {

  userProfile: UserProfile | undefined;

  constructor(
    private accountService: AccountServiceService,
    private toastService : ToastService
  ){ }

  ngOnInit() {
    this.loadUserProfile();
  }

  loadUserProfile() {
    this.accountService.whoAmI().pipe(
      catchError(err => {
        this.toastService.showError(err.messageToClient ||'An error occurred while loading your profile.');
        return of({} as ResponseDto<User>);
      })
    ).subscribe(response => {
      if (response && response.responseData) {
        this.userProfile = response.responseData;
      } else {
        this.toastService.showError(response.messageToClient || 'No profile data available.');
      }
    }, error => {
      this.toastService.showError(error.error.messageToClient || 'An unexpected error occurred.');
    });
  }

  editProfile(){

  }
}
