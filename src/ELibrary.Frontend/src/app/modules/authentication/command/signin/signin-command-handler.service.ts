import { Injectable, OnDestroy } from '@angular/core';
import { filter, Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService, mapSignInCommandToUserAuthenticationRequest, SignInCommand } from '../..';
import { CommandHandler, RedirectorService } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class SignInCommandHandlerService extends CommandHandler<SignInCommand> implements OnDestroy {
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly authService: AuthenticationService,
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

  dispatch(command: SignInCommand): void {
    const req = mapSignInCommandToUserAuthenticationRequest(command);

    this.authService.signInUser(req);
    this.authService.getUserAuth().pipe(
      takeUntil(this.destroy$),
      tap(authData => {
        if (authData.isAuthenticated) {
          command.matDialogRef.close();
          this.redirector.redirectToHome();
          this.cleanUp();
        }
      }),
      filter(authData => !authData.isAuthenticated),
    ).subscribe();
  }
}