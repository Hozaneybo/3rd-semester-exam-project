import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../shared/Models/CourseModel";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})


  export class HomePage implements OnInit {

  courses: AllCoursesView[] = [];
  readonly url = environment.apiEndpoint + 'api/Guest/courses';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<any>(this.url).subscribe(
      (response) => {
        this.courses = response.responseData;
      },
      (error) => {
        console.error('Error fetching courses', error);
      }
    );
  }

}
