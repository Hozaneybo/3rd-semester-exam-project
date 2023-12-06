import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {TeacherService} from "../../../services/teacher.service";
import {ToastController} from "@ionic/angular";
import {UpdateLessonCommand} from "../../../../shared/Models/CourseModel";

@Component({
  selector: 'app-update-lesson',
  templateUrl: './update-lesson.component.html',
  styleUrls: ['./update-lesson.component.scss'],
})
export class UpdateLessonComponent implements OnInit {
  updateLessonForm: FormGroup;
  courseId!: number;
  lessonId!: number;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private toastController: ToastController,
    private router: Router
  ) {
    this.updateLessonForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      pictureUrls: [''],
      videoUrls: ['']
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const courseId = params.get('courseId');
      const lessonId = params.get('id');
      if (courseId !== null && lessonId !== null) {
        this.courseId = +courseId;
        this.lessonId = +lessonId;
        this.loadLesson();
      } else {
        // Handle the case where parameters are null
        this.showToast('Invalid course or lesson ID');
      }
    });

  }

  loadLesson() {
    this.teacherService.getLessonById(this.courseId, this.lessonId).subscribe({
      next: response => {
        if (response.responseData) {
          const lesson = response.responseData;
          this.updateLessonForm.patchValue({
            title: lesson.title,
            content: lesson.content,
            pictureUrls: lesson.imgUrls.map(img => img.pictureUrl).join(','),
            videoUrls: lesson.videoUrls.map(video => video.videoUrl).join(',')
          });
        } else {
          // Handle the scenario when response.responseData is undefined.
          this.showToast('No lesson data available.');
        }
      },
      error: err => {
        this.showToast('Error fetching lesson details');
        console.error(err);
      }
    });
  }


  updateLesson() {
    if (this.updateLessonForm.valid) {
      const command: UpdateLessonCommand = {
        id: this.lessonId,
        courseId: this.courseId,
        ...this.updateLessonForm.value,
        pictureUrls: this.updateLessonForm.value.pictureUrls.split(','),
        videoUrls: this.updateLessonForm.value.videoUrls.split(',')
      };

      this.teacherService.updateLesson(this.lessonId, command).subscribe({
        next: response => {
          this.showToast('Lesson updated successfully');
          this.router.navigate(['/teacher/course-details', this.courseId]);
        },
        error: err => {
          this.showToast('Error updating lesson');
          console.error(err);
        }
      });
    }
  }

  private showToast(message: string) {
    this.toastController.create({
      message: message,
      duration: 3000
    }).then(toast => toast.present());
  }
}
