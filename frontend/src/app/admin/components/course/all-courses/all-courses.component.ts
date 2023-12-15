import { Component, OnInit } from '@angular/core';
import { AllCoursesView, LessonView } from "../../../../shared/Models/CourseModel";
import { AdminService } from "../../../services/admin.service";
import { Router } from "@angular/router";
import { ToastService } from "../../../../shared/services/toast.service";
import {AlertController} from "@ionic/angular"; // Import ToastService

@Component({
  selector: 'app-all-courses',
  templateUrl: './all-courses.component.html',
  styleUrls: ['./all-courses.component.scss'],
})
export class AllCoursesComponent implements OnInit {

  courses: AllCoursesView[] = [];
  lessons: LessonView[] = [];

  constructor(
    private adminService: AdminService,
    private router: Router,
    private toastService: ToastService,
    private alertCtrl: AlertController
  ) {}

  ngOnInit() {
    this.adminService.getAllCourses().subscribe({
      next: (response) => {
        if (response && response.responseData) {
          this.courses = response.responseData;
        } else {
          this.toastService.showError(response.messageToClient || 'No courses found.');
        }
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Error fetching courses')
      }
    });
  }

  async presentConfirmation(courseId: number) {
    const alert = await this.alertCtrl.create({
      header: 'Confirm Delete',
      message: 'Do you really want to delete this course?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary'
        }, {
          text: 'Delete',
          handler: () => {
            this.confirmDelete(courseId);
          }
        }
      ]
    });

    await alert.present();
  }

  confirmDelete(courseId: number) {
    this.adminService.deleteCourse(courseId).subscribe({
      next: (response) => {
        this.toastService.showSuccess(response.messageToClient || 'Course deleted successfully');
        this.courses = this.courses.filter(course => course.id !== courseId);
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Error deleting course');
      }
    });
  }

  deleteCourse(courseId: number, event: Event) {
    event.stopPropagation();
    this.presentConfirmation(courseId);
  }

  createCourse() {
    this.router.navigate(['/admin/create-course']);
  }

  updateCourse(courseId: number, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/admin/course/update', courseId]);
  }

}
