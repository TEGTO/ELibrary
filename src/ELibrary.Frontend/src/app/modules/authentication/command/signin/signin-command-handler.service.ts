import { Injectable, OnDestroy } from '@angular/core';
import { catchError, filter, of, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { AuthenticationService, mapSignInCommandToUserAuthenticationRequest, SignInCommand } from '../..';
import { CommandHandler, SnackbarManager } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class SignInCommandHandlerService extends CommandHandler<SignInCommand> implements OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly authService: AuthenticationService,
    private readonly snackbarManager: SnackbarManager,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.cleanUp();
  }

  cleanUp() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatch(command: SignInCommand): void {
    const req = mapSignInCommandToUserAuthenticationRequest(command);

    this.authService.signInUser(req);
    this.authService.getUserAuth().pipe(
      takeUntil(this.destroy$),
      tap(authData => {
        if (authData.isAuthenticated) {
          command.matDialogRef.close();
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
      catchError(() => {
        this.snackbarManager.openErrorSnackbar(['An error occurred during authentication.']);
        this.cleanUp();
        return of();
      })
    ).subscribe();
  }
}