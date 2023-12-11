import { Component, OnInit } from '@angular/core';
import { AllCoursesView, LessonView } from "../../../../shared/Models/CourseModel";
import { AdminService } from "../../../services/admin.service";
import { Router } from "@angular/router";
import { ToastService } from "../../../../shared/services/toast.service"; // Import ToastService

@Component({
  selector: 'app-all-courses',
  templateUrl: './all-courses.component.html',
  styleUrls: ['./all-courses.component.scss'],
})
export class AllCoursesComponent implements OnInit {

  courses: AllCoursesView[] = [];
  lessons: LessonView[] = [];

  constructor(
    private adminService: AdminService,
    private router: Router,
    private toastService: ToastService // Inject ToastService
  ) {}

  ngOnInit() {
    this.adminService.getAllCourses().subscribe({
      next: (response) => {
        if (response && response.responseData) {
          this.courses = response.responseData;
        } else {
          this.toastService.showError(response.messageToClient || 'No courses found.');
        }
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Error fetching courses')
      }
    });
  }

  deleteCourse(courseId: number, event: Event): void {
    event.stopPropagation();

    if (confirm('Are you sure you want to delete this course?')) {
      this.adminService.deleteCourse(courseId).subscribe({
        next: (response) => {
          if(response) {
            this.toastService.showSuccess(response.messageToClient || 'Course deleted successfully');
            this.courses = this.courses.filter(course => course.id !== courseId);
          }

        },
        error: (error) => {
          this.toastService.showError(error.messageToClient || 'Error deleting courses')

        }
      });
    }
  }

  createCourse() {
    this.router.navigate(['/admin/create-course']);
  }

  updateCourse(courseId: number, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/admin/course/update', courseId]);
  }

}
