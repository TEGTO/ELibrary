import { Injectable, OnDestroy } from '@angular/core';
import { catchError, of, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { AuthenticationService, mapSignUpCommandToUserRegistrationRequest, SignUpCommand } from '../..';
import { CommandHandler, SnackbarManager } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class SignUpCommandHandlerService extends CommandHandler<SignUpCommand> implements OnDestroy {
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

  dispatch(command: SignUpCommand): void {
    const req = mapSignUpCommandToUserRegistrationRequest(command);

    this.authService.registerUser(req).pipe(
      takeUntil(this.destroy$),
      tap(isSuccess => {
        if (isSuccess) {
          this.snackbarManager.openInfoSnackbar('✔️ The registration is successful!', 5);
          command.matDialogRef.close();
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
      catchError(() => {
        this.snackbarManager.openErrorSnackbar(['An error occurred during registration.']);
        this.cleanUp();
        return of();
      })
    ).subscribe();
  }

}
