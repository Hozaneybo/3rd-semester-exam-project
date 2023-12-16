import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {ResponseDto, User} from "../../../shared/Models/LoginModels";
import {AccountServiceService} from "../../../shared/services/account-service.service";
import {ToastService} from "../../../shared/services/toast.service";

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss'],
})
export class EditProfileComponent implements OnInit {
  userId: number | undefined;
  updateUserForm: FormGroup;
  user!: ResponseDto<User>;

  constructor(
    private route: ActivatedRoute,
    private accountService: AccountServiceService,
    private fb: FormBuilder,
    private router: Router,
    private toastService: ToastService
  ) {
    this.updateUserForm = this.fb.group({
      fullName: ['', Validators.required],
      email: [''],
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.accountService.whoAmI().subscribe({
      next: (userInfo) => {
        this.user = userInfo;
        this.updateUserForm.patchValue({
          fullName: this.user.responseData?.fullname,
          email: userInfo.responseData.email,
        });
      },
      error: (error) => this.toastService.showError(error.messageToClient || 'Error fetching user data.')
    });
  }

  onFileSelect(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.updateUserForm.get('avatar')?.setValue(file);
    }
  }

  onSubmit(): void {
    this.accountService.updateUser(this.userId!, this.updateUserForm.value).subscribe({
      next: (response) => {this.toastService.showSuccess(response.messageToClient || 'User updated successfully.');
        this.router.navigate(['/student/my-profile'])},
      error: (error) => this.toastService.showError(error.messageToClient || 'Error occurs under updating')
    });
  }
}
