import { Injectable } from '@angular/core';
import { AuthenticationDialogManager, StartRegistrationCommand } from '../..';
import { CommandHandler } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class StartRegistrationCommandHandlerService extends CommandHandler<StartRegistrationCommand> {

  constructor(
    private readonly dialogManager: AuthenticationDialogManager
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: StartRegistrationCommand): void {
    this.dialogManager.openRegisterMenu();
  }
}
