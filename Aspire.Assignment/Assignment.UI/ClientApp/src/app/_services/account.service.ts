import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { TokenResponse, User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { SocialAuthService, SocialUser } from "@abacritt/angularx-social-login";
import { GoogleLoginProvider } from "@abacritt/angularx-social-login";
import { ExternalAuth } from '../_models/externalauth';
// import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthResponseDto } from '../_models/AuthResponseDto';

@Injectable({ providedIn: 'root' })
export class AccountService {
    private userSubject: BehaviorSubject<User | null>;
    public user: Observable<TokenResponse | null>;

    private authChangeSub = new Subject<boolean>();
    private extAuthChangeSub = new Subject<SocialUser>();
    public authChanged = this.authChangeSub.asObservable();
    public extAuthChanged = this.extAuthChangeSub.asObservable();
    public isExternalAuth: boolean;

    constructor(
        private router: Router,
        private http: HttpClient, private externalAuthService: SocialAuthService
    ) {
        this.externalAuthService.authState.subscribe((usr) => {
            console.log(usr);
            this.extAuthChangeSub.next(usr);
            this.isExternalAuth = true;
          })
        this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
        this.user = this.userSubject.asObservable();
    }

    public get userValue() {
        return this.userSubject.value;
    }

    login(username: string, password: string) {
        const user : User = new User();
        user.username = username;
        user.password = password;
        return this.http.post<TokenResponse>(`${environment.apiUrl}/api/Auth`, user)
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
                this.userSubject.next(user);
                console.log('Juser ==> ',user)
                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['/account/login']);
    }

    register(user: User) {  
        user.id =0;
      
        return this.http.post(`${environment.apiUrl}/api/User`, user);
    }

    
  LoginWithGoogle(credentials: string): Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post(`${environment.apiUrl}/api/User/LoginWithGoogle`, JSON.stringify(credentials), { headers: header, withCredentials: true });
  }

    
//   public externalLogin = (body: ExternalAuth) => {
//     return this.http.post<any>(`${environment.apiUrl}/api/Auth/ExternalLogin`, body);
//   }

  public signInWithGoogle = ()=> {
    this.externalAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  public signOutExternal = () => {
    this.externalAuthService.signOut();
  }
  public externalLogin = (route: string, body: ExternalAuth) => {
    return this.http.post<AuthResponseDto>(this.createCompleteRoute(route, environment.apiUrl), body);
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }
}
