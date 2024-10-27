/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { BehaviorSubject, filter, Observable, switchMap, take, throwError } from 'rxjs';
import { AuthenticationService, LOG_OUT_COMMAND_HANDLER, LogOutCommand } from '../..';
import { AuthToken, CommandHandler, ErrorHandler, UserAuth } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
  private authToken: AuthToken | null = null;
  private decodedToken: JwtPayload | null = null;
  private isRefreshing = false;
  private readonly refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private isAuthenticated = false;

  constructor(
    private readonly authService: AuthenticationService,
    @Inject(LOG_OUT_COMMAND_HANDLER) private readonly logOutHandler: CommandHandler<LogOutCommand>,
    private readonly errorHandler: ErrorHandler
  ) {
    this.authService.getUserAuth().subscribe(
      data => {
        this.processAuthData(data);
      }
    );
    this.authService.getAuthErrors().subscribe(errors => {
      if (errors !== null) {
        this.authToken = null;
        this.decodedToken = null;
        this.logOutUserWithError(errors);
      }
    });
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<object>> {
    let authReq = req;
    if (authReq.url.includes('/refresh') || !this.isAuthenticated) {
      return next.handle(authReq);
    }
    if (this.authToken != null) {
      authReq = this.addTokenHeader(req, this.authToken.accessToken);
    }
    if (this.isTokenExpired()) {
      return this.refreshToken(authReq, next);
    }

    return next.handle(authReq);
  }
  private refreshToken(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      if (this.authToken) {
        return this.authService.refreshToken(this.authToken).pipe(
          filter(isSuccess => isSuccess === true),
          take(1),
          switchMap(() => {
            this.isRefreshing = false;
            this.refreshTokenSubject.next(this.authToken!.accessToken);
            return next.handle(this.addTokenHeader(request, this.authToken!.accessToken));
          })
        );
      }
    }
    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap((token) => next.handle(this.addTokenHeader(request, token)))
    );
  }
  private addTokenHeader(req: HttpRequest<any>, token: string) {
    const clonedRequest = req.clone({
      headers: req.headers
        .set('Authorization', `Bearer ${token}`)
    });
    return clonedRequest;
  }
  private logOutUserWithError(errorMessage: string): Observable<never> {
    const command: LogOutCommand = {};
    this.logOutHandler.dispatch(command);
    return throwError(() => new Error(errorMessage));
  }
  private isTokenExpired(): boolean {
    if (this.decodedToken?.exp) {
      const expirationDate = new Date(0);
      expirationDate.setUTCSeconds(this.decodedToken.exp);

      const currentDatePlusMinutes = new Date();

      return expirationDate < currentDatePlusMinutes;
    }
    return false;
  }
  private tryDecodeToken(token: string): JwtPayload | null {
    try {
      return jwtDecode<JwtPayload>(token);
    } catch (error) {
      this.errorHandler.handleError(error);
      return null;
    }
  }
  private processAuthData(data: UserAuth): void {
    this.authToken = data.authToken;
    this.isAuthenticated = data.isAuthenticated;
    if (this.isAuthenticated) {
      this.decodedToken = this.tryDecodeToken(this.authToken.accessToken);
    }
  }
}