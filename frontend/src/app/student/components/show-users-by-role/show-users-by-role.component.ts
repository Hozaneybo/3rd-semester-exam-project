import { Component, OnInit } from '@angular/core';
import {User} from "../../../admin/components/LoginModels";
import {ActivatedRoute} from "@angular/router";
import {StudentService} from "../../services/student.service";
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
    private studentService: StudentService,
    private route: ActivatedRoute,
    private toastService : ToastService
  ) { }


  message: Promise<void> | undefined;

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
    this.studentService.getUsersByRole(role).subscribe({
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
