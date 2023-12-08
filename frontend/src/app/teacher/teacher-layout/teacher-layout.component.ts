import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";

@Component({
  selector: 'app-teacher-layout',
  templateUrl: './teacher-layout.component.html',
  styleUrls: ['./teacher-layout.component.scss'],
})
export class TeacherLayoutComponent implements OnInit {

  constructor(private router: Router, private accountService: AccountServiceService) { }

  ngOnInit() {}

  logout() {
    this.accountService.logout();
    this.router.navigate([''])
  }

  openProfile() {
    // Logic to navigate to the profile page
    this.router.navigate(['/teacher/profile']);
  }
  navigateToUsersByRole(role: string) {
    this.router.navigate([`/teacher/users/role/${role}`]);
  }

}
