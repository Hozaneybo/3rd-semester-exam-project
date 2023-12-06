import { Component} from '@angular/core';
import {AdminService} from "../../../services/admin.service";
import {Router} from "@angular/router";

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

  constructor(private adminService: AdminService, private router : Router) {}

  onSubmit() {
    if (this.course.title && this.course.description) {
      this.adminService.createCourse(this.course).subscribe(response => {
        // Handle response
        console.log(response);
        this.goBack();
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/admin/courses']);
  }
}
