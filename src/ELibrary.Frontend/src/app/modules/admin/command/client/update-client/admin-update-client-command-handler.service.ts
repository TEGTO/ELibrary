import { Injectable } from '@angular/core';
import { AdminService, AdminUpdateClientCommand } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminUpdateClientCommandHandlerService extends CommandHandler<AdminUpdateClientCommand> {

  constructor(
    private readonly adminService: AdminService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  dispatch(command: AdminUpdateClientCommand): void {

  }
}