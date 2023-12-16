import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {catchError} from "rxjs/operators";
import {TeacherService} from "../services/teacher.service";
import {SearchResultDto} from "../../shared/Models/SearchTerm";
import {ToastService} from "../../shared/services/toast.service";

@Component({
  selector: 'app-teacher-layout',
  templateUrl: './teacher-layout.component.html',
  styleUrls: ['./teacher-layout.component.scss'],
})
export class TeacherLayoutComponent{

  searchResults: SearchResultDto[] = [];

  constructor(
    private router: Router,
    private accountService: AccountServiceService,
    private teacherService: TeacherService,
    private toastService : ToastService) { }


  logout() {
    this.accountService.logout();
    this.router.navigate([''])
  }

  openProfile() {
    this.router.navigate(['/teacher/my-profile']);
  }
  navigateToUsersByRole(role: string) {
    this.router.navigate([`/teacher/users/role/${role}`]);
  }

  performSearch(searchTerm: string) {
    searchTerm = searchTerm.trim();
    if (searchTerm) {
      this.teacherService.search(searchTerm).pipe(
        catchError(err => {
          this.toastService.showError(err.messageToClient || 'An error occurred while searching.');
          return [];
        })
      ).subscribe(response => {
        this.searchResults = response.responseData || [];
      }, err => {
        this.toastService.showError(err.error.messageToClient || 'Error fetching search results.');
      });
    } else {
      this.searchResults = [];
    }
  }

  handleSearchResults(results: SearchResultDto[]) {
    this.searchResults = results;
  }

}
