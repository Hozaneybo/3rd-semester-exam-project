import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {SearchResultDto} from "../../shared/Models/SearchTerm";


@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent {

  searchResults: SearchResultDto[] = [];

  constructor(
    private router: Router,
    private accountService: AccountServiceService,
    ) { }

  logout() {
    this.accountService.logout();
    this.router.navigate([''])
  }

  openProfile() {
    this.router.navigate(['/admin/my-profile']);
  }
  navigateToUsersByRole(role: string) {
    this.router.navigate([`/admin/users/role/${role}`]);
  }

  handleSearchResults(results: SearchResultDto[]) {
    this.searchResults = results;
  }

}
