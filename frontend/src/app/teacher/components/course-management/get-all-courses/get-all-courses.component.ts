import { Component, OnInit } from '@angular/core';
import { TeacherService } from "../../../services/teacher.service";
import { AllCoursesView } from "../../../../shared/Models/CourseModel";
import { Router } from '@angular/router';
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-get-all-courses',
  templateUrl: './get-all-courses.component.html',
  styleUrls: ['./get-all-courses.component.scss'],
})
export class GetAllCoursesComponent implements OnInit {
  courses?: AllCoursesView[] = [];

  constructor(
      private teacherService: TeacherService,
      private toastService : ToastService,
      private router: Router
  ) {}

  ngOnInit() {
    this.teacherService.getAllCourses().subscribe({
      next: (response) => {
        this.courses = response.responseData;
        this.toastService.showSuccess(response.messageToClient || 'Courses loaded successfully');
      },
      error: (error) => {
        this.toastService.showError('Failed to load courses. Please try again later.');
      }
    });
  }

  navigateToCourseDetails(courseId: number) {
    this.router.navigate(['/teacher/course-details', courseId]);
  }

}
