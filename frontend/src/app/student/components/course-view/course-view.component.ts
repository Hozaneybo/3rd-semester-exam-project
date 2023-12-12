import { Component, OnInit } from '@angular/core';
import {CourseView} from "../../../shared/Models/CourseModel";
import {ActivatedRoute, Router} from "@angular/router";
import {ResponseDto} from "../../../shared/Models/LoginModels";
import {StudentService} from "../../services/student.service";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-course-view',
  templateUrl: './course-view.component.html',
  styleUrls: ['./course-view.component.scss'],
})
export class CourseViewComponent  implements OnInit {
  course: CourseView | undefined;

  constructor(
    private studentService: StudentService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService : ToastService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const courseId = params['id'];
      this.studentService.getCourseById(courseId).subscribe({
        next: (response: ResponseDto<CourseView>) => {
          this.course = response.responseData;
          },
        error: (error) => {
          this.toastService.showError(error.messageToClient || 'An error occurred while fetching the course details.');
        }
      });
    });
  }

  navigateToLesson(courseId: number, lessonId: number) {
    this.router.navigate([`/student/courses/${courseId}/lessons/${lessonId}`]);
  }

}
