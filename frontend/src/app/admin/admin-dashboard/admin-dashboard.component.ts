import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent  implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
    // Here you could load summary data like user count, course count, etc.
  }

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
