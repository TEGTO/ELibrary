import { Injectable } from '@angular/core';
import { AdminRegisterUserCommand, AdminService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminRegisterUserCommandHandlerService extends CommandHandler<AdminRegisterUserCommand> {

  constructor(
    private readonly adminService: AdminService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  dispatch(command: AdminRegisterUserCommand): void {

  }
}
