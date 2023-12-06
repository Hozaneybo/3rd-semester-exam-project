import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent  {

  selectedSection: string = 'home';

  constructor(private router: Router) { }



  navigateToUsers() {
    this.router.navigate(['/admin/users']);
  }

  navigateToCourses() {
    this.router.navigate(['/admin/courses']);
  }

  navigateToLessons() {
    this.router.navigate(['/admin/lessons']);
  }


}
