import { Component, OnInit } from '@angular/core';
import {LessonView} from "../../../shared/Models/CourseModel";
import {Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {StudentService} from "../../services/student.service";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-lesson-view',
  templateUrl: './lesson-view.component.html',
  styleUrls: ['./lesson-view.component.scss'],
})
export class LessonViewComponent  implements OnInit {
  lesson: LessonView | null = null;
  private subscriptions: Subscription = new Subscription();

  constructor(
    private studentService: StudentService,
    private route: ActivatedRoute,
    private toastService: ToastService,
  ) {}

  ngOnInit(): void {
    this.subscriptions.add(this.route.paramMap.subscribe(params => {
      const courseId = params.get('courseId');
      const lessonId = params.get('lessonId');
      //console.log(`Course ID: ${courseId}, Lesson ID: ${lessonId}`);

      if (courseId && lessonId) {
        this.loadLesson(+courseId, +lessonId);
      } else {
        //console.error('Course ID or Lesson ID is missing or invalid.');
        this.toastService.showError('Invalid course or lesson ID');
      }
    }));
  }


  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }


  loadLesson(courseId: number, lessonId: number): void {
    this.studentService.getLessonById(courseId, lessonId).subscribe(responseDto => {
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
}
