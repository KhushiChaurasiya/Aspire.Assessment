import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AccountService } from '../_services/account.service';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private accountService: AccountService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const user = this.accountService.userValue;
        if (user) {
            // debugger;
            // authorised so return true
            // if(user.roleId == 1)
            // {
            //     this.router.navigate(['/adminDash'], { queryParams: { returnUrl: state.url }});
            // }else if(user.roleId == 2)
            // {
            //     this.router.navigate(['/devDash'], { queryParams: { returnUrl: state.url }});
            // }
            // else if(user.roleId == 3)
            // {
            //     this.router.navigate(['/userDash'], { queryParams: { returnUrl: state.url }});
            // }

            return true;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/account/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}