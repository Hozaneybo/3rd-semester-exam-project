import { Component, OnInit } from '@angular/core';
import { AdminService } from "../../../services/admin.service";
import { User } from "../../LoginModels";
import {Router} from "@angular/router";
import {ToastService} from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss'],
})
export class AllUsersComponent implements OnInit {

  users: User[] | undefined ;

  constructor(private adminService: AdminService,
              private router : Router,
              private toastService : ToastService) {}

  ngOnInit(): void {
    this.adminService.getAllUsers().subscribe({
      next: (response) => {
        this.users = response.responseData;
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Error fetching users');
      }
    });
  }


  deleteUser(userId: number): void {
    if(confirm('Are you sure you want to delete this user?')) {
      this.adminService.deleteUser(userId).subscribe({
        next: (response) => {
          //this.users = this.users?.filter(user => user.id !== userId);
          this.toastService.showSuccess(response.messageToClient || ' User successfully deleted')
        },
        error: (error) => {
          this.toastService.showError(error.messageToClient || 'Error occurs under deleting')
        }
      });
    }
  }

  viewDetails(userId: number): void {
    this.router.navigate(['/admin/user-details', userId]);
  }

  getUsersByRole(role: string): void {
    this.router.navigate(['/admin/users-by-role', role]);
  }

  updateUser(userId: number): void {
    this.router.navigate(['/admin/update-user', userId]);
  }
}
