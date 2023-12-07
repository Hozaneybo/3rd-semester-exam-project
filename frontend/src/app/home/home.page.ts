import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../shared/Models/CourseModel";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable} from "rxjs";


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})


  export class HomePage implements OnInit {

  /*selectedSection: string = 'home';

  courses: Course[] = [];
  readonly url = environment.apiEndpoint + 'api/Guest/courses';

  constructor(private http: HttpClient, private router: Router) {}

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

  navigate(section: string) {
    this.router.navigate( [/'home'/ +section]);
  }*/

  selectedSection: string = 'home';


  readonly url = environment.apiEndpoint + 'api/Guest/courses';
  courses: AllCoursesView[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.getCourses().subscribe(
      (response) => {
        this.courses = response.responseData;
      },
      (error) => {
        console.error('Error fetching courses', error);
      }
    );
  }

  getCourses(): Observable<any> {
    return this.http.get<any>(this.url);
  }

}
