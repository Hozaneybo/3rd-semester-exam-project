import { Component, OnInit } from '@angular/core';
import {AllCoursesView, LessonView} from "../../../../shared/Models/CourseModel";
import {AdminService} from "../../../services/admin.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-all-courses',
  templateUrl: './all-courses.component.html',
  styleUrls: ['./all-courses.component.scss'],
})
export class AllCoursesComponent  implements OnInit {

  courses : AllCoursesView[] = [];
  lessons: LessonView[] = [];

  constructor(private adminService : AdminService, private router : Router) {}

  ngOnInit() {
    return this.adminService.getAllCourses().subscribe( response => {
      if (response && response.responseData) {
        this.courses = response.responseData
      }
    })
  }

  deleteCourse(courseId: number, event: Event): void {
    event.stopPropagation();

    if (confirm('Are you sure you want to delete this course?')) {
      this.adminService.deleteCourse(courseId).subscribe({
        next: (response) => {
          console.log('Course deleted successfully:', response);
          this.courses = this.courses.filter(course => course.id !== courseId);
        },
        error: (error) => {
          console.error('Error deleting course:', error);
        }
      });
    }
  }

  createCourse(){
    this.router.navigate(['/admin/create-course']);
  }

  updateCourse(courseId: number, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/admin/update-course', courseId]);
  }


  loadLessons(courseId: number): void {
    this.adminService.getLessonsByCourseId(courseId).subscribe({
      next: (response) => {
        if (response && response.responseData) {
          this.lessons = response.responseData;
        }
      },
      error: (error) => {
        console.error('Error fetching lessons:', error);
      }
    });
  }
  navigateToLessons(courseId: number) {
    // The actual path needs to match your Angular routing configuration
    this.router.navigate(['/admin/courses', courseId, 'lessons']);
  }
}
