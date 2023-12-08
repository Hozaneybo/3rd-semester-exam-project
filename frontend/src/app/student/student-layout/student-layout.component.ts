import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { SearchResultDto } from "../../shared/Models/SearchTerm";
import { StudentService } from "../services/student.service";
import { debounceTime, distinctUntilChanged, Subject } from "rxjs";
import { catchError } from 'rxjs/operators';
import { ToastController } from "@ionic/angular";

@Component({
  selector: 'app-student-layout',
  templateUrl: './student-layout.component.html',
  styleUrls: ['./student-layout.component.scss'],
})
export class StudentLayoutComponent  implements OnInit {

  searchResults: SearchResultDto[] = [];
  private searchSubject = new Subject<string>();

  constructor(
    private router: Router,
    private accountService: AccountServiceService,
    private studentService: StudentService,
    private toastController: ToastController) { }

  ngOnInit() {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(searchTerm => {
      this.performSearch(searchTerm);
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate([''])
  }

  openProfile() {
    // Logic to navigate to the profile page
    this.router.navigate(['/student/profile']);
  }

  navigateToUsersByRole(role: string) {
    this.router.navigate([`/student/users/role/${role}`]);
  }

  performSearch(searchTerm: string) {
    searchTerm = searchTerm.trim();
    if (searchTerm) {
      this.studentService.search(searchTerm).pipe(
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

  onSearchChange(searchTerm: string) {
    this.searchSubject.next(searchTerm);
  }

  async presentToast(message: string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
    });
    toast.present();
  }

}
