import { Injectable } from '@angular/core';
import { AuthenticationDialogManager, AuthenticationService, StartLoginCommand } from '../..';
import { CommandHandler } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class StartLoginCommandHandlerService extends CommandHandler<StartLoginCommand> {
  isAuthenticated = false;

  constructor(
    private readonly dialogManager: AuthenticationDialogManager,
    private readonly authService: AuthenticationService,
  ) {
    super();

    this.authService.getUserAuth().subscribe(data => {
      this.isAuthenticated = data.isAuthenticated;
    })
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: StartLoginCommand): void {
    if (this.isAuthenticated) {
      this.dialogManager.openAuthenticatedMenu();
    }
    else {
      this.dialogManager.openLoginMenu();
    }
  }
}
