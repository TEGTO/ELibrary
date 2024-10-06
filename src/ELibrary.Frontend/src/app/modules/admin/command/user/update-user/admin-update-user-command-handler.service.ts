import { Injectable } from '@angular/core';
import { AdminService, AdminUpdateUserCommand } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminUpdateUserCommandHandlerService extends CommandHandler<AdminUpdateUserCommand> {

  constructor(
    private readonly adminService: AdminService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  dispatch(command: AdminUpdateUserCommand): void {

  }
}
