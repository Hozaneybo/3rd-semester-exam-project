import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {firstValueFrom, Observable} from 'rxjs';
import {AccountServiceService} from "../services/account-service.service";

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private accountService: AccountServiceService,
    private router: Router
  ) {}

  async canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {
    const expectedRole = route.data['expectedRole']; // Use index access here

    try {
      const userInfo = await firstValueFrom(this.accountService.whoAmI());
      const currentRole = userInfo.responseData.role;

      if (currentRole && currentRole === expectedRole) {
        return true;
      }
    } catch (error) {
      // Handle any errors, like not being logged in
    }

    this.router.navigate(['login']);
    return false;
  }
}
