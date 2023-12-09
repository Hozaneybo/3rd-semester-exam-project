import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../../../../shared/Models/CourseModel";
import {TeacherService} from "../../../../teacher/services/teacher.service";
import {ToastController} from "@ionic/angular";
import {Router} from "@angular/router";
import {AdminService} from "../../../services/admin.service";

@Component({
  selector: 'app-course-for-lesson',
  templateUrl: './course-for-lesson.component.html',
  styleUrls: ['./course-for-lesson.component.scss'],
})
export class CourseForLessonComponent  implements OnInit {

  courses?: AllCoursesView[] = [];

  constructor(
    private adminService: AdminService,
    private toastController: ToastController,
    private router: Router
  ) {}

  ngOnInit() {
    this.adminService.getAllCourses().subscribe({
      next: (response) => {
        this.courses = response.responseData;
      },
      error: (error) => {
      }
    });
  }

  navigateToCourseDetails(courseId: number) {
    this.router.navigate(['/admin/course-details', courseId]);
  }

}
