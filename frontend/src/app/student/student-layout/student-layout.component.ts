import { Component} from '@angular/core';
import { Router } from "@angular/router";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { SearchResultDto } from "../../shared/Models/SearchTerm";


@Component({
  selector: 'app-student-layout',
  templateUrl: './student-layout.component.html',
  styleUrls: ['./student-layout.component.scss'],
})
export class StudentLayoutComponent  {

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
    this.router.navigate(['/student/my-profile']);
  }

  navigateToUsersByRole(role: string) {
    this.router.navigate([`/student/users/role/${role}`]);
  }

  handleSearchResults(results: SearchResultDto[]) {
    this.searchResults = results;
  }

}
