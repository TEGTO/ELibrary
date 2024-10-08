import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AdminCreateClientCommand, AdminDialogManager, AdminService } from '../../..';
import { CommandHandler, getDefaultClient, mapClientToCreateClientRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminCreateClientCommandHandlerService extends CommandHandler<AdminCreateClientCommand> {

  constructor(
    private readonly adminService: AdminService,
    private readonly dialogService: AdminDialogManager,
  ) {
    super();
  }

  dispatch(command: AdminCreateClientCommand): void {
    this.dialogService.openClientChangeMenu(getDefaultClient()).afterClosed().pipe(
      take(1),
    ).subscribe(client => {
      if (client) {
        const req = mapClientToCreateClientRequest(client);
        this.adminService.createClientForUser(command.userId, req);
      }
    });
  }
}