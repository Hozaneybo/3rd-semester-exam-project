import { Component, OnInit } from '@angular/core';
import {CourseView} from "../../../../shared/Models/CourseModel";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {ResponseDto} from "../../LoginModels";
import {AdminService} from "../../../services/admin.service";

@Component({
  selector: 'app-lesson-details',
  templateUrl: './lesson-details.component.html',
  styleUrls: ['./lesson-details.component.scss'],
})
export class LessonDetailsComponent  implements OnInit {

  course: CourseView | undefined;

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastController: ToastController
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const courseId = params['id'];
      this.adminService.getCourseById(courseId).subscribe({
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
    this.router.navigate([`/admin/courses/${courseId}/lessons/${lessonId}`]);
  }

  private async showToast(message: string): Promise<void> {
    const toast = await this.toastController.create({
      message: message,
      duration: 3000
    });
    toast.present();
  }

}