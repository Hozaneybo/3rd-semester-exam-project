import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TeacherService } from "../../../services/teacher.service";
import { CreateLessonCommand } from "../../../../shared/Models/CourseModel";
import { ToastController } from '@ionic/angular';
import {ActivatedRoute} from "@angular/router";

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
      private toastController: ToastController,
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
            await this.presentToast(response.messageToClient || 'Lesson created successfully', 'success');
          },
          async error => {
            await this.presentToast('Error creating lesson. Please try again.', 'danger');
            console.error('Error creating lesson', error);
          }
      );
    }
  }

  async presentToast(message: string, color: string = 'primary') {
    const toast = await this.toastController.create({
      message: message,
      color: color,
      duration: 5000
    });
    toast.present();
  }
}
