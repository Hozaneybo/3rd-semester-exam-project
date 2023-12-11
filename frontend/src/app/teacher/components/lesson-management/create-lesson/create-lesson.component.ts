import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TeacherService } from "../../../services/teacher.service";
import { CreateLessonCommand } from "../../../../shared/Models/CourseModel";
import {ActivatedRoute} from "@angular/router";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrls: ['./create-lesson.component.scss'],
})
export class CreateLessonComponent {
  lessonForm: FormGroup;


  constructor(
      private fb: FormBuilder,
      private teacherService: TeacherService,
      private toastService : ToastService,
      private route: ActivatedRoute
  ) {
    this.lessonForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      content: ['', Validators.required],
      courseId: ['', Validators.required],
      pictureUrls: [''],
      videoUrls: ['']
    });
    this.route.params.subscribe(params => {
      this.lessonForm.controls['courseId'].setValue(params['id']);
    });
  }

  createLesson() {
    if (this.lessonForm.valid) {
      const command: CreateLessonCommand = {
        title: this.lessonForm.value.title,
        content: this.lessonForm.value.content,
        courseId: this.lessonForm.value.courseId,
        pictureUrls: this.lessonForm.value.pictureUrls ? this.lessonForm.value.pictureUrls.split(',') : [],
        videoUrls: this.lessonForm.value.videoUrls ? this.lessonForm.value.videoUrls.split(',') : []
      };

      this.teacherService.createLesson(command.courseId.toString(), command).subscribe(
          async response => {
            await this.toastService.showSuccess(response.messageToClient || 'Lesson created successfully');
          },
          async error => {
            await this.toastService.showError('Error creating lesson. Please try again.');
          }
      );
    }
  }
}
