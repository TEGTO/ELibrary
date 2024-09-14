import { Injectable, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { catchError, filter, of, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { SnackbarManager, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from '../../../shared';
import { AuthenticationService } from '../authentication-service/authentication-service';
import { AuthenticationCommand, AuthenticationCommandType } from './authentication-command';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationCommandService implements AuthenticationCommand, OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly authService: AuthenticationService,
    private readonly snackbarManager: SnackbarManager,
  ) { }

  ngOnDestroy(): void {
    this.cleanUp();
  }
  cleanUp(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatchCommand(commandType: AuthenticationCommandType, dispatchedFrom: any, ...params: any[]): void {
    switch (commandType) {
      case AuthenticationCommandType.SignUp:
        this.signUpCommand(dispatchedFrom, params[0], params);
        break;
      case AuthenticationCommandType.SignIn:
        this.singInCommand(dispatchedFrom, params[0], params);
        break;
      case AuthenticationCommandType.LogOut:
        this.logOutCommand(dispatchedFrom, params);
        break;
      case AuthenticationCommandType.Update:
        this.updateCommand(dispatchedFrom, params[0], params);
        break;
      default:
        break;
    }
  }

  private signUpCommand(dispatchedFrom: any, userReq: UserRegistrationRequest, params: any[]) {
    this.authService.registerUser(userReq).pipe(
      takeUntil(this.destroy$),
      tap(isSuccess => {
        if (isSuccess) {
          this.snackbarManager.openInfoSnackbar('✔️ The registration is successful!', 5);
          this.closeDialogIfPresent(params);
          this.cleanUp();
        }
      }),
      switchMap(isSuccess => isSuccess ? of() : this.authService.getRegistrationErrors()),
      tap(errors => {
        if (errors) {
          this.snackbarManager.openErrorSnackbar(errors.split('\n'));
          this.cleanUp();
        }
      }),
      catchError(err => {
        this.snackbarManager.openErrorSnackbar(['An error occurred during registration.']);
        this.cleanUp();
        return of();
      })
    ).subscribe();
  }
  private singInCommand(dispatchedFrom: any, req: UserAuthenticationRequest, params: any[]) {
    this.authService.singInUser(req);
    this.authService.getAuthData().pipe(
      takeUntil(this.destroy$),
      tap(authData => {
        if (authData.isAuthenticated) {
          this.closeDialogIfPresent(params);
          this.cleanUp();
        }
      }),
      filter(authData => !authData.isAuthenticated),
      switchMap(() => this.authService.getAuthErrors()),
      tap(errors => {
        if (errors) {
          this.snackbarManager.openErrorSnackbar(errors.split("\n"));
          this.cleanUp();
        }
      }),
      catchError(err => {
        this.snackbarManager.openErrorSnackbar(['An error occurred during authentication.']);
        this.cleanUp();
        return of();
      })
    ).subscribe();
  }
  private logOutCommand(dispatchedFrom: any, params: any[]) {
    this.authService.logOutUser();
  }
  private updateCommand(dispatchedFrom: any, userReq: UserUpdateRequest, params: any[]) {
    this.authService.updateUserData(userReq).pipe(
      takeUntil(this.destroy$),
      tap(isSuccess => {
        if (isSuccess) {
          this.snackbarManager.openInfoSnackbar('✔️ The update is successful!', 5);
          this.closeDialogIfPresent(params);
          this.cleanUp();
        }
      }),
      switchMap(isSuccess => isSuccess ? of() : this.authService.getAuthErrors()),
      tap(errors => {
        if (errors) {
          this.snackbarManager.openErrorSnackbar(errors.split('\n'));
          this.cleanUp();
        }
      }),
      catchError(err => {
        this.snackbarManager.openErrorSnackbar(['An error occurred during updating.']);
        this.cleanUp();
        return of();
      })
    ).subscribe();
  }
  private closeDialogIfPresent(params: any[]): void {
    for (var i = 0; i < params.length; i++) {
      if (params[i] instanceof MatDialogRef) {
        params[i].close();
      }
    }
  }
}
