import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-teacher-dashboard',
  templateUrl: './TeacherDashBoard.component.html',
  styleUrls: ['./TeacherDashBoard.component.scss'],
})
export class TeacherDashBoardComponent {

  constructor(private router: Router) {}

  showCourses() {
    this.router.navigate(['/teacher/all-courses']);
  }

}
