import { Component } from '@angular/core';
import { AccountService } from './_services/account.service';
import { TokenResponse, User } from './_models/user';


@Component({ selector: 'app-root', templateUrl: 'app.component.html' })
export class AppComponent {
    user?: User | null;
    role : any;
    userDetails: TokenResponse | null;
    constructor(private accountService: AccountService) {
        this.userDetails = this.accountService.userValue;
        this.accountService.user.subscribe(x => this.user = x);
        if(localStorage.user != null){
            var d =JSON.parse(localStorage.user);
            this.role = d.role;
        }
    }

    logout() {
        this.accountService.logout();
    }
}