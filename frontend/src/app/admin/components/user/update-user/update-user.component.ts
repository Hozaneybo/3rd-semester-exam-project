import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import {ToastService} from "../../../../shared/services/toast.service";

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
    private toastService : ToastService

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
        this.toastService.showError(error.messageToClient || 'Error fetching user')
      }
    });
  }

  submitUpdate(): void {
    if (this.updateUserForm.valid && this.userId) {
      this.adminService.updateUser(this.userId, this.updateUserForm.value).subscribe({
        next: (response) => {
          if(response) {
            this.toastService.showSuccess(response.messageToClient ||'User updated successfully.');
            this.router.navigate(['/admin/users']);
          }
        },
        error: (error) => {
         this.toastService.showError(error.messageToClient || 'Error occurs under updating')
        }
      });
    } else {
      this.toastService.showError('Form is invalid or userId is missing')
    }
  }

  goBack(): void {
    this.router.navigate(['/admin/users']);
  }

}
