import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountServiceService} from "../../shared/services/account-service.service";
import {ToastController} from "@ionic/angular";
import {debounceTime, distinctUntilChanged, Subject} from "rxjs";
import {catchError} from "rxjs/operators";
import {SearchResultDto} from "../../shared/Models/SearchTerm";
import {AdminService} from "../services/admin.service";

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent {
  searchResults: SearchResultDto[] = [];
  private searchSubject = new Subject<string>();

  constructor(
    private router: Router,
    private accountService: AccountServiceService,
    private adminService: AdminService,
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
    this.router.navigate(['/admin/my-profile']);
  }
  navigateToUsersByRole(role: string) {
    this.router.navigate([`/admin/users/role/${role}`]);
  }

  performSearch(searchTerm: string) {
    searchTerm = searchTerm.trim();
    if (searchTerm) {
      this.adminService.search(searchTerm).pipe(
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
