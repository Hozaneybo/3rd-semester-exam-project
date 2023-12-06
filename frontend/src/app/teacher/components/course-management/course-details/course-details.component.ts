import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { TeacherService } from "../../../services/teacher.service";
import { CourseView } from "../../../../shared/Models/CourseModel";
import {ResponseDto} from "../../../../shared/Models/LoginModels";
import {ToastController} from "@ionic/angular";

@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.scss'],
})
export class CourseDetailsComponent implements OnInit {
  course: CourseView | undefined;

  constructor(
    private teacherService: TeacherService,
    private route: ActivatedRoute,
    private router: Router,
    private toastController: ToastController
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const courseId = params['id'];
      this.teacherService.getCourseById(courseId).subscribe({
        next: (response: ResponseDto<CourseView>) => {
          this.course = response.responseData;
        },
        error: (error) => {
          this.showToast(error.messageToClient || 'An error occurred while fetching the course details.');
        }
      });
    });
  }

  navigateToLesson(courseId: number, lessonId: number) {
    this.router.navigate([`/teacher/courses/${courseId}/lessons/${lessonId}`]);
  }

  navigateToCreateLesson() {
    if (this.course && this.course.id) {
      this.router.navigate(['/teacher/create-lesson', this.course.id]);
    } else {
      this.showToast('Course ID is not available.');
    }
  }

  private async showToast(message: string): Promise<void> {
    const toast = await this.toastController.create({
      message: message,
      duration: 3000
    });
    toast.present();
  }

}
