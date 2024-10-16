import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService, mapUpdateUserCommandToUserUpdateRequest, UpdateUserCommand } from '../..';
import { CommandHandler, SnackbarManager } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateUserCommandHandlerService extends CommandHandler<UpdateUserCommand> implements OnDestroy {
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

  dispatch(command: UpdateUserCommand): void {
    const req = mapUpdateUserCommandToUserUpdateRequest(command);

    this.authService.updateUserAuth(req).pipe(
      takeUntil(this.destroy$),
      tap(isSuccess => {
        if (isSuccess) {
          this.snackbarManager.openInfoSnackbar('✔️ The update is successful!', 5);
          command.matDialogRef.close();
          this.cleanUp();
        }
      }),
    ).subscribe();
  }
}