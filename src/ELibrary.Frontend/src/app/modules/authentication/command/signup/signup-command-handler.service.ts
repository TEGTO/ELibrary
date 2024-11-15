import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService, mapSignUpCommandToUserRegistrationRequest, SignUpCommand } from '../..';
import { CommandHandler, RedirectorService, SnackbarManager } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class SignUpCommandHandlerService extends CommandHandler<SignUpCommand> implements OnDestroy {
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly authService: AuthenticationService,
    private readonly snackbarManager: SnackbarManager,
    private readonly redirector: RedirectorService,
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
          this.redirector.redirectToHome();
          this.cleanUp();
        }
      }),
    ).subscribe();
  }

}
