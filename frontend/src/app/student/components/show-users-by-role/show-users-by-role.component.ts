import { Component, OnInit } from '@angular/core';
import {User} from "../../../admin/components/LoginModels";
import {ActivatedRoute} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {StudentService} from "../../services/student.service";

@Component({
  selector: 'app-show-users-by-role',
  templateUrl: './show-users-by-role.component.html',
  styleUrls: ['./show-users-by-role.component.scss'],
})
export class ShowUsersByRoleComponent implements OnInit {
  users!: User[] | undefined;
  selectedRole: string = 'Student';

  constructor(
    private studentService: StudentService,
    private route: ActivatedRoute,
    private toastController: ToastController
  ) { }


  message: string | undefined;

  ngOnInit() {
    this.route.params.subscribe(params => {
      const roleFromUrl = params['role'];
      if (['Admin', 'Teacher', 'Student'].includes(roleFromUrl)) {
        this.selectedRole = roleFromUrl;
        this.fetchUsersByRole(this.selectedRole);
      } else {
        this.selectedRole = 'Select';
        this.message = 'Please select the group of users you are looking for.';
        this.users = [];
      }
    });
  }




  fetchUsersByRole(role: string): void {
    this.studentService.getUsersByRole(role).subscribe({
      next: (response) => {
        this.users = response.responseData;
        this.presentToast(response.messageToClient || `${role} users fetched successfully.`, 'success');
      },
      error: (error) => {
        const errorMessage = error.error?.messageToClient || `Failed to fetch ${role} users. Please try again later.`;
        this.presentToast(errorMessage, 'danger');
      }
    });
  }

  private async presentToast(message: string, color: 'success' | 'danger') {
    const toast = await this.toastController.create({
      message: message,
      duration: 3000,
      color: color
    });
    await toast.present();
  }
}
