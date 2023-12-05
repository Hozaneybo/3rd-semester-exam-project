import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AdminService } from "../../../services/admin.service";

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
    private router: Router
  ) {
    this.updateCourseForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      courseImgUrl: ['']
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.courseId = parseInt(id, 10);
      this.loadCourseData(this.courseId)

    } else {
      this.router.navigate(['/admin/courses']).then(() => {
      });
    }
  }

  loadCourseData(courseId: number): void {
    this.adminService.getCourseById(courseId).subscribe({
      next: (response) => {
        const course = response.responseData;
        if (course) {
          this.updateCourseForm.patchValue({
            title: course.title,
            description: course.description,
            courseImgUrl: course.courseImgUrl
          });
        }
      },
      error: (error) => {
        console.error('Error fetching course details:', error);
      }
    });
  }

  submitUpdate(): void {
    if (this.updateCourseForm.valid && this.courseId) {
      this.adminService.updateCourse(this.courseId, this.updateCourseForm.value).subscribe({
        next: (response) => {
          console.log('Course updated successfully:', response);
          this.successMessage = 'Course updated successfully.';
          this.router.navigate(['/admin/courses']);
        },
        error: (error) => {
          console.error('Error updating course:', error);
          this.successMessage = undefined;
        }
      });
    } else {
      console.error('Form is invalid or courseId is missing');
    }
  }

}
