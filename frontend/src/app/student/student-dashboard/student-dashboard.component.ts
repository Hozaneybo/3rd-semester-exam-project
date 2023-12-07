import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.scss'],
})
export class StudentDashboardComponent  {

  constructor(private router: Router) {}

  showCourses() {
    this.router.navigate(['/student/all-courses']);
  }

}
