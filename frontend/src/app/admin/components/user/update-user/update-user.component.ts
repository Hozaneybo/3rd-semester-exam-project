import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AdminService } from "../../../services/admin.service";

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss'],
})
export class UpdateUserComponent implements OnInit {

  userId: number | undefined;
  updateUserForm: FormGroup;
  successMessage: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService,
    private fb: FormBuilder,
    private router: Router,

  ) {
    this.updateUserForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: [''],
      role: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userId = parseInt(id, 10);
      this.loadUserData(this.userId);
    } else {
      this.router.navigate(['/admin/users']).then(() => {
      });
    }
  }

  loadUserData(userId: number): void {
    this.adminService.getUserById(userId).subscribe({
      next: (response) => {
        const user = response.responseData;
        if (user) {
          this.updateUserForm.patchValue({
            fullName: user.fullname,
            email: user.email,
            role: user.role
          });
        }
      },
      error: (error) => {
        console.error('Error fetching user details:', error);
      }
    });
  }

  submitUpdate(): void {
    if (this.updateUserForm.valid && this.userId) {
      this.adminService.updateUser(this.userId, this.updateUserForm.value).subscribe({
        next: (response) => {
          console.log('User updated successfully:', response);
          this.successMessage = 'User updated successfully.';
          this.router.navigate(['/admin/users']);
        },
        error: (error) => {
          console.error('Error updating user:', error);
          this.successMessage = undefined;
        }
      });
    } else {
      console.error('Form is invalid or userId is missing');
    }
  }

  goBack(): void {
    this.router.navigate(['/admin/users']);
  }

}
