import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";

@Component({
  selector: 'app-student-layout',
  templateUrl: './student-layout.component.html',
  styleUrls: ['./student-layout.component.scss'],
})
export class StudentLayoutComponent  implements OnInit {

  constructor(private router: Router, private accountService: AccountServiceService) { }

  ngOnInit() {}

  logout() {
    this.accountService.logout();
    this.router.navigate([''])
  }

  openProfile() {
    // Logic to navigate to the profile page
    this.router.navigate(['/student/profile']);
  }

}
