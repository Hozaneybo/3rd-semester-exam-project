import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { AdminService } from "../../../services/admin.service";
import { ResponseDto, User } from "../../../../shared/Models/LoginModels";
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
})
export class UserDetailsComponent implements OnInit, OnDestroy {

  user: User | undefined;
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe(params => {
      const userId = params['id'];
      if (userId) {
        this.adminService.getUserById(userId).subscribe({
          next: (response: ResponseDto<User>) => {
            this.user = response.responseData;
          },
          error: (error) => {
            console.error('Error fetching user details:', error);
          }
        });
      }
    });
  }

  deleteUser(): void {
    if (this.user?.id && confirm('Are you sure you want to delete this user?')) {
      this.adminService.deleteUser(this.user.id).subscribe({
        next: () => {
          this.router.navigate(['/admin/users']);
        },
        error: (err) => {
          console.error('Failed to delete user', err);
        }
      });
    }
  }

  navigateToUpdateUser(): void {
    if(this.user?.id) {
      this.router.navigate(['/admin/update-user', this.user.id]);
    }
  }

  goBack(): void {
    this.router.navigate(['/admin/users']);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
