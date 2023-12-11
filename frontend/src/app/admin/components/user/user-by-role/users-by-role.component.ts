import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import { User } from "../../LoginModels";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-user-by-role',
  templateUrl: './users-by-role.component.html',
  styleUrls: ['./users-by-role.component.scss'],
})
export class UsersByRoleComponent implements OnInit {
  users!: User[] | undefined;
  selectedRole: string = 'Student';
  message : Promise<void> | undefined;

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private toastService : ToastService) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const roleFromUrl = params['role'];
      if (['Admin', 'Teacher', 'Student'].includes(roleFromUrl)) {
        this.selectedRole = roleFromUrl;
        this.fetchUsersByRole(this.selectedRole);
      } else {
        this.selectedRole = 'Select';
        this.message = this.toastService.showInfo('Form is invalid. Please select user role')
        this.users = [];
      }
    });
  }

  fetchUsersByRole(role: string): void {
    this.adminService.getUsersByRole(role).subscribe({
      next: (response) => {
        this.users = response.responseData;
        this.toastService.showSuccess(response.messageToClient || `${role} users fetched successfully.`);
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || `Failed to fetch ${role} users. Please try again later.`)
      }
    });
  }

}
