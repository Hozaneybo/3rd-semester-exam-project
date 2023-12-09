import { Component, OnInit } from '@angular/core';
import { AdminService } from "../../../services/admin.service";
import { User } from "../../LoginModels";
import {Router} from "@angular/router";

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss'],
})
export class AllUsersComponent implements OnInit {

  users: User[] | undefined ;

  constructor(private adminService: AdminService, private router : Router) {}

  ngOnInit(): void {
    this.adminService.getAllUsers().subscribe({
      next: (response) => {
        this.users = response.responseData;
      },
      error: (error) => {
        console.error('Failed to fetch users', error);
      }
    });
  }


  deleteUser(userId: number): void {
    if(confirm('Are you sure you want to delete this user?')) {
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
