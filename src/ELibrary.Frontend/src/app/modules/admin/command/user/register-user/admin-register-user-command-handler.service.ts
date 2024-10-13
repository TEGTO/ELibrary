import { Injectable } from '@angular/core';
import { AdminRegisterUserCommand, AdminService, mapAdminRegisterUserCommandToAdminUserRegistrationRequest } from '../../..';
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


  dispatch(command: AdminRegisterUserCommand): void {
    const req = mapAdminRegisterUserCommandToAdminUserRegistrationRequest(command);
    this.adminService.registerUser(req);
    command.matDialogRef.close();
  }
}
