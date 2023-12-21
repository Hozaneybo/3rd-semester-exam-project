import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import {ToastService} from "../../../../shared/services/toast.service";


@Component({
  selector: 'app-update-course',
  templateUrl: './update-course.component.html',
  styleUrls: ['./update-course.component.scss'],
})
export class UpdateCourseComponent implements OnInit {

  courseId: number | undefined;
  updateCourseForm: FormGroup;
  successMessage: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService,
    private fb: FormBuilder,
    private router: Router,
    private toastService: ToastService
  ) {
    this.updateCourseForm = this.fb.group({
      courseId: [this.courseId],
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      courseImgUrl: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.courseId = parseInt(id, 10);
      this.loadCourseData(this.courseId);
    } else {
      this.toastService.showError('Invalid course ID');
      this.router.navigate(['/admin/courses']);
    }
  }

  loadCourseData(courseId: number): void {
    this.adminService.getCourseById(courseId).subscribe({
      next: (response) => {
        const course = response.responseData;
        if (course) {
          this.updateCourseForm.patchValue({
            courseId: course.id,
            title: course.title,
            description: course.description,
            courseImgUrl: course.courseImgUrl
          });
        }
      },
      error: (error) => {
        this.toastService.showError('Error fetching course details: ' + (error.error?.messageToClient || 'Unknown error'));
      }
    });
  }

  submitUpdate(): void {
    if (this.updateCourseForm.valid && this.courseId) {
      const currentFormValues = this.updateCourseForm.value;
      if (this.courseId === currentFormValues.courseId) {
        this.adminService.updateCourse(this.courseId, currentFormValues).subscribe({
          next: (response) => {
            this.toastService.showSuccess(response.messageToClient || 'Course updated successfully');
            this.router.navigate(['/admin/courses']);
          },
          error: (error) => {
            this.toastService.showError('Error updating course: ' + (error.error?.messageToClient || 'Unknown error'));
          }
        });
      } else {
        this.toastService.showWarning('Invalid form or missing course ID');
      }
    }
  }
}
