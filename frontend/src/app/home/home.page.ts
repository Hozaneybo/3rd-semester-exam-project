import { Component, OnInit } from '@angular/core';
import { AllCoursesView } from '../shared/Models/CourseModel';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})


  export class HomePage implements OnInit {

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
