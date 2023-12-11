import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {TeacherService} from "../../../services/teacher.service";
import {LessonView} from "../../../../shared/Models/CourseModel";
import {Subscription, take} from "rxjs";
import {AlertController} from "@ionic/angular";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-lesson-details',
  templateUrl: './lesson-details.component.html',
  styleUrls: ['./lesson-details.component.scss'],
})


export class LessonDetailsComponent implements OnInit, OnDestroy {
  lesson: LessonView | null = null;
  private subscriptions: Subscription = new Subscription();

  constructor(
    private teacherService: TeacherService,
    private route: ActivatedRoute,
    private toastService : ToastService,
    private router: Router,
    private alertCtrl: AlertController
  ) {}

  ngOnInit(): void {
    this.subscriptions.add(this.route.paramMap.subscribe(params => {
      const courseId = params.get('courseId');
      const lessonId = params.get('lessonId');
      console.log(`Course ID: ${courseId}, Lesson ID: ${lessonId}`); // Add logging to debug

      if (courseId && lessonId) {
        this.loadLesson(+courseId, +lessonId);
      } else {
        this.toastService.showError('Invalid course or lesson ID');
      }
    }));
  }


  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }


  loadLesson(courseId: number, lessonId: number): void {
    this.teacherService.getLessonById(courseId, lessonId).subscribe(responseDto => {
      if (responseDto && responseDto.responseData) {
        this.lesson = responseDto.responseData;
      } else {
        this.toastService.showError('No data found for this lesson.');
      }
    }, error => {
      this.handleHttpError(error);
    });
  }

  private handleHttpError(error: any): void {
    let errorMessage = 'An error occurred while fetching the lesson details.';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    this.toastService.showError(errorMessage);
  }


  async presentConfirmation(lessonId: number) {
    const alert = await this.alertCtrl.create({
      header: 'Confirm Delete',
      message: 'Do you really want to delete this lesson?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: (blah) => {
            console.log('Confirm Cancel: blah');
          }
        }, {
          text: 'Delete',
          handler: () => {
            this.confirmDelete(lessonId);
          }
        }
      ]
    });

    await alert.present();
  }

  confirmDelete(lessonId: number) {
    this.teacherService.deleteLesson(lessonId).subscribe({
      next: (response) => {
        this.toastService.showSuccess(response.messageToClient || 'Lesson deleted successfully');
        this.router.navigate([`/teacher/course-details/${this.lesson?.courseId}`])
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'An error occurred while deleting the lesson.');
      }
    });
  }

  deleteLesson() {
    if (this.lesson?.id != null) {
      this.presentConfirmation(this.lesson.id);
    } else {
      this.toastService.showError('Lesson ID is not available.');
    }
  }

  updateLesson() {
    this.route.paramMap.pipe(take(1)).subscribe(params => {
      const courseId = params.get('courseId');
      const lessonId = params.get('lessonId');
      if (courseId && lessonId) {
        this.router.navigate([`/teacher/courses/${courseId}/update-lesson/${lessonId}`]);
      } else {
        this.toastService.showError('Lesson ID is not available.');
      }
    });
  }

}
