import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {catchError} from "rxjs/operators";
import {TeacherService} from "../services/teacher.service";
import {ToastController} from "@ionic/angular";
import {SearchResultDto} from "../../shared/Models/SearchTerm";

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
    private toastController: ToastController) { }


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
          this.presentToast('An error occurred while searching.');
          return [];
        })
      ).subscribe(response => {
        this.searchResults = response.responseData || [];
      }, err => {
        this.presentToast(err.error.messageToClient || 'Error fetching search results.');
      });
    } else {
      this.searchResults = [];
    }
  }


  async presentToast(message: string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
    });
    toast.present();
  }

  handleSearchResults(results: SearchResultDto[]) {
    this.searchResults = results;
  }

}
