import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AdminDialogManager, AdminService, AdminUpdateClientCommand, mapAdminUpdateClientCommandToUpdateClientRequest } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminUpdateClientCommandHandlerService extends CommandHandler<AdminUpdateClientCommand> {

  constructor(
    private readonly adminService: AdminService,
    private readonly dialogManager: AdminDialogManager,
  ) {
    super();
  }

  dispatch(command: AdminUpdateClientCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        const req = mapAdminUpdateClientCommandToUpdateClientRequest(command);
        this.adminService.updateClientForUser(command.userId, req);
      }
    });
  }
}