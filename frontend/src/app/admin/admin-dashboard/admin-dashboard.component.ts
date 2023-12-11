import { Component, OnInit } from '@angular/core';
import { catchError } from "rxjs/operators";
import { of } from "rxjs";
import { ResponseDto, User, UserProfile } from "../components/LoginModels";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { ToastService } from "../../shared/services/toast.service";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent implements OnInit {

  user!: UserProfile;

  constructor(
    private accountService: AccountServiceService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.accountService.setupClock();
  }

  loadUserProfile() {
    this.accountService.whoAmI().subscribe({
      next: (response) => {
        if (response && response.responseData) {
          this.user = response.responseData;
        } else {
          this.toastService.showError(response.messageToClient || 'No profile data available.');
        }
      },
      error: (error) => {
        let errorMessage = 'An unexpected error occurred';
        if (error && error.error && error.error.messageToClient) {
          errorMessage = error.error.messageToClient;
        }
        this.toastService.showError(errorMessage);
      }
    });
  }

}
