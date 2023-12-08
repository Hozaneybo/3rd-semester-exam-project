import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../shared/Models/CourseModel";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AlertController} from "@ionic/angular";


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})


  export class HomePage implements OnInit {

  selectedSection: string = 'home';


  readonly url = environment.apiEndpoint + 'api/Guest/courses';
  courses: AllCoursesView[] = [];

  constructor(private http: HttpClient, private alertController: AlertController) {
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
  async onCourseClick() {
    const alert = await this.alertController.create({
      header: 'Access Restricted',
      message: 'To view the full content of the courses, you need to log in or create an account.',
      buttons: ['OK']
    });

    await alert.present();
  }



}
