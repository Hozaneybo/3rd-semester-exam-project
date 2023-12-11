import {Component, OnInit} from '@angular/core';
import {catchError} from "rxjs/operators";
import {of} from "rxjs";
import {ResponseDto, User, UserProfile} from "../../admin/components/LoginModels";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {ToastService} from "../../shared/services/toast.service";

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.scss'],
})
export class StudentDashboardComponent implements OnInit{
  user!: UserProfile;

  constructor(private accountService: AccountServiceService,
              private toastService : ToastService
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.accountService.setupClock();
  }

  loadUserProfile() {
    this.accountService.whoAmI().pipe(
      catchError(err => {
        this.toastService.showError(err.messageToClient ||'An error occurred while loading your profile.');
        return of({} as ResponseDto<User>);
      })
    ).subscribe(response => {
      if (response && response.responseData) {
        this.user = response.responseData;
      } else {
        this.toastService.showError(response.messageToClient || 'No profile data available.');
      }
    }, error => {
      this.toastService.showError(error.error.messageToClient || 'An unexpected error occurred.');
    });
  }
}
