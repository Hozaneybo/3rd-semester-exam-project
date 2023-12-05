import { Component, OnInit } from '@angular/core';
import { TeacherService } from "../../../services/teacher.service";
import { AllCoursesView } from "../../../../shared/Models/CourseModel";
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-get-all-courses',
  templateUrl: './get-all-courses.component.html',
  styleUrls: ['./get-all-courses.component.scss'],
})
export class GetAllCoursesComponent implements OnInit {
  courses?: AllCoursesView[] = [];

  constructor(
      private teacherService: TeacherService,
      private toastController: ToastController,
      private router: Router
  ) {}

  ngOnInit() {
    this.teacherService.getAllCourses().subscribe({
      next: (response) => {
        this.courses = response.responseData;
        this.presentToast(response.messageToClient || 'Courses loaded successfully', 'success');
      },
      error: (error) => {
        this.presentToast('Failed to load courses. Please try again later.', 'danger');
      }
    });
  }

  navigateToCourseDetails(courseId: number) {
    this.router.navigate(['/teacher/course-details', courseId]);
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
