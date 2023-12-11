import { Component, OnInit } from '@angular/core';
import {AllCoursesView} from "../../../../shared/Models/CourseModel";
import {Router} from "@angular/router";
import {AdminService} from "../../../services/admin.service";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-course-for-lesson',
  templateUrl: './course-for-lesson.component.html',
  styleUrls: ['./course-for-lesson.component.scss'],
})
export class CourseForLessonComponent  implements OnInit {

  courses?: AllCoursesView[] = [];

  constructor(
    private adminService: AdminService,
    private router: Router,
    private toast : ToastService
  ) {}

  ngOnInit() {
    this.adminService.getAllCourses().subscribe({
      next: (response) => {
        this.courses = response.responseData;
      },
      error: (error) => {
        this.toast.showError('Error updating course: ' + (error.error?.messageToClient || 'Unknown error'));
      }
    });
  }

  navigateToCourseDetails(courseId: number) {
    this.router.navigate(['/admin/course-details', courseId]);
  }

}
