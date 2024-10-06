import { Injectable } from '@angular/core';
import { AdminDeleteUserCommand, AdminService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminDeleteUserCommandHandlerService extends CommandHandler<AdminDeleteUserCommand> {

  constructor(
    private readonly adminService: AdminService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  dispatch(command: AdminDeleteUserCommand): void {

  }
}
