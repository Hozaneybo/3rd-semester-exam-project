import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AccountServiceService } from "../../shared/services/account-service.service";
import { AdminService } from "../services/admin.service";
import {UserProfile} from "../components/LoginModels";

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
    private adminService: AdminService
  ) {
    this.editForm = this.fb.group({
      fullname: ['', [Validators.required]],
      role: ['', [Validators.required]],
      email: [{value: '', disabled: true}], // Email is not editable
      avatar: [null] // For now, we will not set validators for the avatar
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    // Call the API to get the user details
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
        console.error('Error fetching user data:', error);
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
      console.error('User ID or role is not set');
      return;
    }

    const formData = new FormData();
    formData.append('fullname', this.editForm.get('fullname')?.value);
    if (this.editForm.get('avatar')?.value) {
      formData.append('avatar', this.editForm.get('avatar')?.value);
    }

    this.accountService.updateUser(this.user, this.user.role, formData).subscribe({
      next: (response) => {
        console.log('User updated successfully:', response);
        // Handle successful update here
      },
      error: (error) => {
        console.error('Error updating user:', error);
        // Handle error here
      }
    });
  }
}
