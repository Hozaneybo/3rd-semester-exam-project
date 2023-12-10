import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {catchError} from "rxjs/operators";
import {of} from "rxjs";
import {ResponseDto, User, UserProfile} from "../../admin/components/LoginModels";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {ToastController} from "@ionic/angular";

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.scss'],
})
export class StudentDashboardComponent implements OnDestroy {
  private intervalId: any;
  user!: UserProfile;

  constructor(private accountService: AccountServiceService, private toastController: ToastController) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.setupClock();
  }

  ngOnDestroy(): void {
    clearInterval(this.intervalId);
  }

  private setupClock(): void {
    const updateClock = () => {
      const now = new Date();

      const seconds = now.getSeconds();
      const secondsDegrees = ((seconds / 60) * 360) + 90;
      const secondHand = document.querySelector('.second-hand') as HTMLElement;

      const mins = now.getMinutes();
      const minsDegrees = ((mins / 60) * 360) + 90;
      const minsHand = document.querySelector('.min-hand') as HTMLElement;

      const hour = now.getHours();
      const hourDegrees = ((hour / 12) * 360) + 90;
      const hourHand = document.querySelector('.hour-hand') as HTMLElement;

      if (secondHand && minsHand && hourHand) {
        secondHand.style.transform = `rotate(${secondsDegrees}deg)`;
        minsHand.style.transform = `rotate(${minsDegrees}deg)`;
        hourHand.style.transform = `rotate(${hourDegrees}deg)`;
      }
    };

    this.intervalId = setInterval(updateClock, 1000);
    updateClock();
  }

  loadUserProfile() {
    this.accountService.whoAmI().pipe(
      catchError(err => {
        this.presentToast('An error occurred while loading your profile.');
        return of({} as ResponseDto<User>);
      })
    ).subscribe(response => {
      if (response && response.responseData) {
        this.user = response.responseData;
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
}
