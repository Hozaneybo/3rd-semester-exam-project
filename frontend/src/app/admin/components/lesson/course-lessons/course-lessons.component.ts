import { Component, OnInit } from '@angular/core';
import {AdminService} from "../../../services/admin.service";
import {ActivatedRoute} from "@angular/router";
import {LessonView} from "../../../../shared/Models/CourseModel";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-course-lessons',
  templateUrl: './course-lessons.component.html',
  styleUrls: ['./course-lessons.component.scss'],
})
export class CourseLessonsComponent  implements OnInit {

  courseId!: number;
  lessons: LessonView[] | undefined = [];

  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.params['courseId'];
    if (id) {
      this.courseId = +id; // The '+' converts the string to a number
      this.loadLessons();
    } else {
      console.error('Error fetching lessons:', error);
    }
  }

  loadLessons(): void {
    this.adminService.getLessonsByCourseId(this.courseId).subscribe({
      next: (response) => {
        this.lessons = response.responseData;
      },
      error: (error) => {
        console.error('Error fetching lessons:', error);
      }
    });
  }
}
