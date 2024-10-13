import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AdminDialogManager, AdminService, AdminUpdateUserCommand, mapAdminUpdateUserCommandToAdminUserUpdateDataRequest } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminUpdateUserCommandHandlerService extends CommandHandler<AdminUpdateUserCommand> {

  constructor(
    private readonly adminService: AdminService,
    private readonly dialogManager: AdminDialogManager,
  ) {
    super();
  }

  dispatch(command: AdminUpdateUserCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        const req = mapAdminUpdateUserCommandToAdminUserUpdateDataRequest(command);
        this.adminService.updateUser(req);
      }
    });
  }
}
