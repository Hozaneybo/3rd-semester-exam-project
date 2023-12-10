import {Component, OnInit} from '@angular/core';
import {ResponseDto, User, UserProfile} from "../../admin/components/LoginModels";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {catchError} from "rxjs/operators";
import {of} from "rxjs";

@Component({
  selector: 'app-teacher-dashboard',
  templateUrl: './TeacherDashBoard.component.html',
  styleUrls: ['./TeacherDashBoard.component.scss'],
})

export class TeacherDashBoardComponent implements OnInit {
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
        this.accountService.presentToast(response.messageToClient || 'No profile data available.', 'warning');
      }
    }, error => {
      this.accountService.presentToast(error.error.messageToClient || 'An unexpected error occurred.', 'warning');
    });
  }
}
