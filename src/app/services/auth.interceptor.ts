import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";


import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from 'rxjs/operators';


import { AccountService } from "./account.service";
import { EventBusService } from "./event-bus.service";
import { EventData } from "../models/EventData";


@Injectable()
export class AuthIntreceptor implements HttpInterceptor {
    private isRefreshing = false;

    constructor(private authService: AccountService,
        private eventBusService: EventBusService,
    ) {

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem("bearer");

        if (token) {
            req = req.clone({
                setHeaders: { Authorization: `bearer ${token}` },
            });
        }
        return next.handle(req).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse &&
                    !req.url.includes('Account/SignIn') &&
                    error.status === 401
                ) {
                    return this.handle401Error(req, next);
                }
                return throwError(() => error)
            })

        );
    }
    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
            this.isRefreshing = true;

            if (this.authService.isLoggedInCall()) {
                return this.authService.refreshToken().pipe(
                    switchMap(() => {
                        this.isRefreshing = false;

                        return next.handle(request);
                    }),
                    catchError((error) => {
                        this.isRefreshing = false;
                        if (error.status == '403') {
                            this.eventBusService.emit(new EventData('logout', null))
                        }
                        return throwError(() => error)
                    }));
            }
        }
        return next.handle(request);
    }
}

export const httpInterceptorProviders = [
    {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthIntreceptor,
        multi: true
    },
];