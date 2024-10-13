import { Injectable } from '@angular/core';
import { AdminDialogManager, StartAdminRegisterUserCommand } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class StartAdminRegisterUserCommandHandlerService extends CommandHandler<StartAdminRegisterUserCommand> {

  constructor(
    private readonly dialogManager: AdminDialogManager,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: StartAdminRegisterUserCommand): void {
    this.dialogManager.openRegisterMenu();
  }
}
