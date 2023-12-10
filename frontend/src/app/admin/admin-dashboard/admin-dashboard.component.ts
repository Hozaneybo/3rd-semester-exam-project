import { Component, OnInit } from '@angular/core';
import {catchError} from "rxjs/operators";
import {of} from "rxjs";
import {ResponseDto, User, UserProfile} from "../components/LoginModels";
import {AccountServiceService} from "../../shared/services/account-service.service";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent implements OnInit {

  user!: UserProfile;

  constructor(private accountService: AccountServiceService) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.accountService.setupClock();
  }

  loadUserProfile() {
    this.accountService.whoAmI().pipe(
      catchError(err => {
        this.accountService.presentToast('An error occurred while loading your profile.', 'warning');
        return of({} as ResponseDto<User>);
      })
    ).subscribe(response => {
      if (response && response.responseData) {
        this.user = response.responseData;
      } else {
        this.accountService.presentToast(response.messageToClient || 'No profile data available.', 'danger');
      }
    }, error => {
      this.accountService.presentToast(error.error.messageToClient || 'An unexpected error occurred.', 'warning');
    });
  }



}
