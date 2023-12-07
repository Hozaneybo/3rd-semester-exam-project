import { Component, OnInit } from '@angular/core';
import {AdminService} from "../../../services/admin.service";
import {ActivatedRoute, Router} from "@angular/router";
import {CourseView} from "../../../../shared/Models/CourseModel";

@Component({
  selector: 'app-course-lessons',
  templateUrl: './course-lessons.component.html',
  styleUrls: ['./course-lessons.component.scss'],
})
export class CourseLessonsComponent implements OnInit {
  courseId!: number;
  courses: CourseView | undefined;


  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.params['courseId'];
    if (id) {
      this.courseId = +id;
      this.loadLessons();
    }
  }

  loadLessons(): void {
    this.adminService.getCourseById(this.courseId).subscribe({
      next: (response) => {
        if(response)
        this.courses = response.responseData;
      },
      error: (error) => {
        console.error('Error fetching lessons:', error);
      }
    });
  }

  navigateToLesson(courseId: number, lessonId: number) {
    this.router.navigate([`/admin/courses/${courseId}/lessons/${lessonId}`]);
  }
}
