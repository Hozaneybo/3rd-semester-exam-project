import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import { User } from "../../../../shared/Models/LoginModels";
import {ToastController} from "@ionic/angular";

@Component({
  selector: 'app-user-by-role',
  templateUrl: './users-by-role.component.html',
  styleUrls: ['./users-by-role.component.scss'],
})
export class UsersByRoleComponent implements OnInit {
  users!: User[] | undefined;
  selectedRole: string = 'Student';
  message: string | undefined;

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastController: ToastController) {}

  ngOnInit(): void {
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
    this.adminService.getUsersByRole(role).subscribe({
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

  deleteUser(userId: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.adminService.deleteUser(userId).subscribe({
        next: () => {
          this.users = this.users?.filter(user => user.id !== userId);
          // Display a success message here
        },
        error: (err) => {
          console.error('Failed to delete user', err);
          // Display an error message here
        }
      });
    }
  }

  updateUser(userId: number): void {
    this.router.navigate(['/admin/update-user', userId]);
  }
  goBack(): void {
    this.router.navigate(['/admin/users']);
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
