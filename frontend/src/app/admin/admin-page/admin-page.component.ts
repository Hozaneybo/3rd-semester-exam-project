import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent {

  constructor(private router : Router, private accountService: AccountServiceService) { }


  navigate(section: string) {
    this.router.navigate(['/admin/' + section]);
  }

  onLogout() {
    this.accountService.logout().subscribe({
      next: (response) => {
        // Handle successful logout, e.g., navigate to login page
        console.log(response.messageToClient);
        this.router.navigate([''])
      },
      error: (error) => {
        // Handle error
        console.error('Logout failed', error);
      }
    });
  }

  navigateToProfile(){

  }

}
