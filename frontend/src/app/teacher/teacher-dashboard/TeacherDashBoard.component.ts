import {Component, OnDestroy, OnInit} from '@angular/core';
@Component({
  selector: 'app-teacher-dashboard',
  templateUrl: './TeacherDashBoard.component.html',
  styleUrls: ['./TeacherDashBoard.component.scss'],
})
export class TeacherDashBoardComponent implements OnInit, OnDestroy {
  private intervalId: any;

  constructor() {}

  ngOnInit(): void {
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

}
