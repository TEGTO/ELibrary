import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AdminDeleteUserCommand, AdminDialogManager, AdminService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AdminDeleteUserCommandHandlerService extends CommandHandler<AdminDeleteUserCommand> {

  constructor(
    private readonly adminService: AdminService,
    private readonly dialogManager: AdminDialogManager,
  ) {
    super();
  }

  dispatch(command: AdminDeleteUserCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.adminService.deleteUser(command.userId);
      }
    });
  }
}
