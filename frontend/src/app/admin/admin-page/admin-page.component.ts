import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { SearchResultDto } from "../../shared/Models/SearchTerm";
import { ToastService } from "../../shared/services/toast.service";

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
    private toastService: ToastService
  ) { }

  async logout() {
    try {
      const response = await this.accountService.logout();
      if (response) {
        await this.router.navigate(['']);
        this.toastService.showSuccess(response.messageToClient || 'Logged out successfully');
      } else {
        this.toastService.showError('Logout failed');
      }
    } catch (error) {
      this.toastService.showError('Logout request failed: ' + ( 'Unknown error'));
    }
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
