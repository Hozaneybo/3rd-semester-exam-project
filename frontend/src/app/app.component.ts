import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {environment} from "../environments/environment";
import {AllCoursesView} from "./shared/Models/CourseModel";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})

export class AppComponent  {

  constructor(private router: Router) {}


}

