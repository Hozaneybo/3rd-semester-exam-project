import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { catchError } from "rxjs/operators";
import { SearchResultDto } from "../../Models/SearchTerm";
import { debounceTime, distinctUntilChanged, Subject } from "rxjs";
import { AccountServiceService } from "../../services/account-service.service";
import { ToastController } from "@ionic/angular";

@Component({
  selector: 'app-shared-search',
  templateUrl: './shared-search.component.html',
  styleUrls: ['./shared-search.component.scss'],
})
export class SharedSearchComponent implements OnInit {
  private searchSubject = new Subject<string>();
  searchResults: SearchResultDto[] = [];

  @Output() searchResultsEmitter = new EventEmitter<SearchResultDto[]>();

  constructor(private service: AccountServiceService, private toastController: ToastController) { }

  ngOnInit() {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(searchTerm => {
      this.performSearch(searchTerm);
    });
  }

  performSearch(searchTerm: string) {
    searchTerm = searchTerm.trim();
    if (searchTerm) {
      this.service.search(searchTerm).pipe(
        catchError(err => {
          this.service.presentToast('An error occurred while searching.', 'warning');
          return [];
        })
      ).subscribe(response => {
        this.searchResults = response.responseData || [];
        this.searchResultsEmitter.emit(this.searchResults);
      }, err => {
        this.service.presentToast(err.error.messageToClient || 'Error fetching search results.', 'warning');
      });
    } else {
      this.searchResults = [];
      this.searchResultsEmitter.emit(this.searchResults);
    }
  }

  onSearchChange(searchTerm: string) {
    this.searchSubject.next(searchTerm);
  }

}
