import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent {

  constructor(private router : Router) { }


  navigate(section: string) {
    this.router.navigate(['/admin/' + section]);
  }

}