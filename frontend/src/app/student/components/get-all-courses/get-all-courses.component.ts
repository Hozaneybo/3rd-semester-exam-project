import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../../../shared/Models/CourseModel";
import {ToastController} from "@ionic/angular";
import {Router} from "@angular/router";
import {StudentService} from "../../services/student.service";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-get-all-courses',
  templateUrl: './get-all-courses.component.html',
  styleUrls: ['./get-all-courses.component.scss'],
})
export class GetAllCoursesComponent  implements OnInit {
  courses?: AllCoursesView[] = [];

  constructor(
    private studentService: StudentService,
    private toastController: ToastController,
    private router: Router,
    private toastService : ToastService
  ) {}

  ngOnInit() {
    this.studentService.getAllCourses().subscribe({
      next: (response) => {
        this.courses = response.responseData;
        this.toastService.showSuccess(response.messageToClient || 'Courses loaded successfully');
        },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Failed to load courses. Please try again later.');
      }
    });
  }

  navigateToCourseDetails(courseId: number) {
    this.router.navigate(['/student/course-details', courseId]);
  }

}
