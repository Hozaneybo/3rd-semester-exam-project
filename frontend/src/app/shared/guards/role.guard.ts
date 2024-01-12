import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router} from '@angular/router';
import {firstValueFrom} from 'rxjs';
import {AccountServiceService} from "../services/account-service.service";
import {ToastService} from "../services/toast.service";

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private accountService: AccountServiceService,
    private router: Router,
    private toastService : ToastService
  ) {}

  async canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {
    const expectedRole = route.data['expectedRole'];

    try {
      const userInfo = await firstValueFrom(this.accountService.whoAmI());
      const currentRole = userInfo.responseData.role;

      if (currentRole && currentRole === expectedRole) {
        return true;
      }
    } catch (error) {
      this.toastService.showError('Not being logged in')
    }

    this.router.navigate(['/login']);
    return false;
  }
}
