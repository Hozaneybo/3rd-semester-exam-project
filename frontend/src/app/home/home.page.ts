import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../shared/Models/CourseModel";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AlertController} from "@ionic/angular";
import {ToastService} from "../shared/services/toast.service";


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})


  export class HomePage implements OnInit {

  selectedSection: string = 'home';


  readonly url =  '/api/Guest/courses';
  courses: AllCoursesView[] = [];

  constructor(private http: HttpClient,
              private alertController: AlertController,
              private toastService : ToastService
  ) {
  }

  ngOnInit() {
    this.getCourses().subscribe(
      (response) => {
        this.courses = response.responseData;
      },
      (error) => {
        this.toastService.showError(error.messageToClient || 'Error fetching courses')
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
