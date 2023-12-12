import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { AdminService } from "../services/admin.service";
import {UserProfile} from "../../shared/Models/LoginModels";
import {ToastService} from "../../shared/services/toast.service";

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss'],
})
export class EditProfileComponent implements OnInit {
  editForm: FormGroup;
  userId: number | undefined;
  role: string | undefined;
  user : UserProfile | undefined;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountServiceService,
    private adminService: AdminService,
    private toastService : ToastService
  ) {
    this.editForm = this.fb.group({
      fullname: ['', [Validators.required]],
      role: ['', [Validators.required]],
      email: [{value: '', disabled: true}],
      avatar: [null]
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.accountService.whoAmI().subscribe({
      next: (response) => {
        const userInfo = response.responseData;
        this.user = response.responseData;
        console.log(this.user)
        this.editForm.patchValue({
          fullname: userInfo.fullname,
          email: userInfo.email,
          // avatar: userInfo.avatarUrl // to display the avatar, handle it separately
        });
      },
      error: (error) => {
        this.toastService.showError(error.messageToClient || 'Error fetching user data.')
      }
    });
  }

  onFileSelect(event: any): void {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.editForm.get('avatar')?.setValue(file);
    }
  }

  onSubmit(): void {
    if (!this.user?.id ) {
      this.toastService.showError('User ID or role is not set')
      return;
    }

    const formData = new FormData();
    formData.append('fullname', this.editForm.get('fullname')?.value);
    if (this.editForm.get('avatar')?.value) {
      formData.append('avatar', this.editForm.get('avatar')?.value);
    }

    this.accountService.updateUser(this.user, this.user.role, formData).subscribe({
      next: (response) => {
        this.toastService.showSuccess(response.messageToClient || 'User updated successfully')
      },
      error: (error) => {
       this.toastService.showError(error.messageToClient || 'Error updating user')
      }
    });
  }
}
