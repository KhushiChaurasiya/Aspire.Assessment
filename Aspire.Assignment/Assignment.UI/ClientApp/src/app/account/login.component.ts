import { Component, OnInit, NgZone } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { AlertService } from '../_services/alert.service';
import { SocialAuthService  } from '@abacritt/angularx-social-login';
import { SocialUser } from '@abacritt/angularx-social-login';
import { GoogleLoginProvider } from '@abacritt/angularx-social-login';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { User } from '../_models/user';
import { HttpErrorResponse } from '@angular/common/http';
import { ExternalAuth } from '../_models/externalauth';


@Component({ templateUrl: 'login.component.html' })
export class LoginComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    user!: SocialUser; 
    Role! : string;
    userData : User[];

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService,
        private authService: SocialAuthService,
        private _ngZone: NgZone,
    ) {  }


    ngOnInit(): void {
      this.form = this.formBuilder.group({
          username: ['', Validators.required],
          password: ['', Validators.required]
      });

      this.authService.authState.subscribe((user: any) => {
        debugger;
        console.log(user);
        this.user = user;
        this.externalLogin();
      })

}

externalLogin = () => {
debugger;
    const externalAuth: ExternalAuth = {
      provider: this.user.provider,
      idToken : this.user.idToken
    }
    this.validateExternalAuth(externalAuth);
}

private validateExternalAuth(externalAuth: ExternalAuth ) {
  this.accountService.externalLogin(externalAuth)
    .subscribe({
      next: (res) => {
          localStorage.setItem("token", res.token);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigateByUrl(returnUrl);
    },
      error: (err: HttpErrorResponse) => {
      }
    });
}

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        this.accountService.login(this.f.username.value, this.f.password.value)
           // .pipe(first())
            .subscribe({
                next: (data : any) => {
                    console.log('data',data)
                    this.Role = data.role;
                    this.authenticateUser(this.Role);
                    // get return url from query parameters or default to home page
                  if(this.Role == "Admin")
                  {
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/adminDash/home';
                    this.router.navigateByUrl(returnUrl);
                  }
                  else if(this.Role == "Developer")
                  {
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/devDash/home';
                    this.router.navigateByUrl(returnUrl);
                  }
                  else if(this.Role == "User")
                  {
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/userDash/home';
                    this.router.navigateByUrl(returnUrl);
                  }
                },
                error: (error: any) => {
                  debugger;
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
    signInWithGoogle(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then((x: any) => console.log(x));
  }

    signOut(): void {
    this.authService.signOut();
    } 

    logInWithGoogle(platform: string): void {
      debugger;
      platform = GoogleLoginProvider.PROVIDER_ID;
      //Sign In and get user Info using authService that we just injected
         this.authService.signIn(platform).then(
      (response) => {
      //Get all user details
           console.log(platform + ' logged in user data is= ' , response);
      //Take the details we need and store in an array
           this.userData.push({
            provider: response.provider,
             firstname: response.firstName,
             lastname: response.lastName,
             email: response.email
           });
       },
       (error) => {
         console.log(error);
      }
  )}


    authenticateUser(RoleData : any){
        debugger;
        if(RoleData == "Admin" && RoleData != "Developer" && RoleData != "User"){
          this.router.navigate(['/adminDash']);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/adminDash/home';
          this.router.navigateByUrl(returnUrl);
        } else if(RoleData == "Developer" && RoleData != "Admin" && RoleData != "User"){ 
          this.router.navigate(['/devDash']);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/devDash/home';
          this.router.navigateByUrl(returnUrl);
        } else if(RoleData == "User" && RoleData != "Admin" && RoleData != "Developer"){
          this.router.navigate(['/userDash'])
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/userDash/home';
          this.router.navigateByUrl(returnUrl);
        }
      }
    
}