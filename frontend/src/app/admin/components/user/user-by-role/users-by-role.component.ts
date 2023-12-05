import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import { User } from "../../../../shared/Models/LoginModels";

@Component({
  selector: 'app-user-by-role',
  templateUrl: './users-by-role.component.html',
  styleUrls: ['./users-by-role.component.scss'],
})
export class UsersByRoleComponent implements OnInit {
  users: User[] | undefined;
  selectedRole: string = 'Admin';
  successMessage: string | undefined;

  constructor(private adminService: AdminService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['role']) {
        this.selectedRole = params['role'];
      }
    });
    this.fetchUsersByRole(this.selectedRole);
  }

  fetchUsersByRole(role: string): void {
    this.adminService.getUsersByRole(role).subscribe({
      next: (response) => {
        this.users = response.responseData;
        this.successMessage = 'User updated successfully.';
      },
      error: (error) => {
        console.error('Failed to fetch users by role', error);
        this.successMessage = undefined;

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
}
