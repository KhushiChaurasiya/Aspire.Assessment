import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private accountService: AccountService) {}

    // intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //     return next.handle(request).pipe(catchError(err => {
    //         if ([401, 403].includes(err.status) && this.accountService.userValue) {
    //             // auto logout if 401 or 403 response returned from api
    //             this.accountService.logout();
    //         }

    //         const error = err.error?.message || err.statusText;
    //         console.error(err);
    //         return throwError(() => error);
    //     }))
    // }
    //====================================================
    // intercept(request: HttpRequest<any>,next: HttpHandler): Observable<HttpEvent<any>> {
    //     debugger;
    //     return next.handle(request).pipe(retry(1),catchError((error: HttpErrorResponse) => {
    //                 let errorMessage = '';
    //                 if (error.error instanceof ErrorEvent) {
    //                     // client-side error
    //                     errorMessage = `Error: ${error.error.message}`;
    //                 } else {
    //                     // server-side error
    //                     errorMessage = `Error Status: ${error.status}\nMessage: ${error.message}`;
    //                 }
    //                 console.log(errorMessage);
    //                 return throwError(errorMessage);
    //             })
    //         )
    // }

     errors: any;
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
 
        return next.handle(req).pipe(
          catchError((error) => {
            if(error.error.Message != undefined){
                this.errors = error.error.Message;
            }
            else{
              this.errors=  error.error.errors;
            }
            return throwError(() => this.errors);
          })
        )
    }
}