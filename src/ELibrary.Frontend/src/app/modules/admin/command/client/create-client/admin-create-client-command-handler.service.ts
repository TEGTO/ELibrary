import { Injectable } from '@angular/core';
import { AdminCreateClientCommand, AdminService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminCreateClientCommandHandlerService extends CommandHandler<AdminCreateClientCommand> {

  constructor(
    private readonly adminService: AdminService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  dispatch(command: AdminCreateClientCommand): void {

  }
}