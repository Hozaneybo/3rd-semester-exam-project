import { Component, OnInit } from '@angular/core';
import {TeacherService} from "../../services/teacher.service";
import {User} from "../../../shared/Models/LoginModels";
import {ActivatedRoute} from "@angular/router";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-show-users-by-role',
  templateUrl: './show-users-by-role.component.html',
  styleUrls: ['./show-users-by-role.component.scss'],
})
export class ShowUsersByRoleComponent implements OnInit {
  users!: User[] | undefined;
  selectedRole: string = 'Student';

  constructor(
    private teacherService: TeacherService,
    private route: ActivatedRoute,
    private toastService : ToastService
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
    this.teacherService.getUsersByRole(role).subscribe({
      next: (response) => {
        this.users = response.responseData;
        this.toastService.showSuccess(response.messageToClient || `${role} users fetched successfully.`);
      },
      error: (error) => {
        const errorMessage = error.error?.messageToClient || `Failed to fetch ${role} users. Please try again later.`;
        this.toastService.showError(errorMessage);
      }
    });
  }

}
