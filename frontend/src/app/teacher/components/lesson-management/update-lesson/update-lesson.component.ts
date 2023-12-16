import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {TeacherService} from "../../../services/teacher.service";
import {UpdateLessonCommand} from "../../../../shared/Models/CourseModel";
import {ToastService} from "../../../../shared/services/toast.service";

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
    private toastService : ToastService,
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
        this.toastService.showError('Invalid course or lesson ID');
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
          this.toastService.showError('No lesson data available.');
        }
      },
      error: err => {
        this.toastService.showError(err.messageToClient || 'Error fetching lesson details');
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
          this.toastService.showSuccess('Lesson updated successfully');
          this.router.navigate(['/teacher/course-details', this.courseId]);
        },
        error: err => {
          this.toastService.showError(err.messageToClient || 'Error updating lesson');
        }
      });
    }
  }
}
