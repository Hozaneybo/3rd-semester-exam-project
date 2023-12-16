import { Component} from '@angular/core';
import {AdminService} from "../../../services/admin.service";
import {Router} from "@angular/router";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html',
  styleUrls: ['./create-course.component.scss'],
})
export class CreateCourseComponent  {
  course = {
    title: '',
    description: '',
    courseImgUrl: ''
  };

  constructor(
    private adminService: AdminService,
    private router : Router,
    private toastService: ToastService
  ) {}

  onSubmit() {
    if (this.course.title && this.course.description) {
      this.adminService.createCourse(this.course).subscribe({
        next: (response) => {
          this.toastService.showSuccess(response.messageToClient ||'Course created successfully');
          this.goBack();
        },
        error: (error) => {
          let errorMessage = 'Error creating course';
          if (error.error && error.error.messageToClient) {
            errorMessage = error.error.messageToClient;
          }
          this.toastService.showError(errorMessage);
        }
      });
    } else {
      this.toastService.showWarning('Please fill in all fields');
    }
  }



  goBack(): void {
    this.router.navigate(['/admin/courses']);
  }
}
