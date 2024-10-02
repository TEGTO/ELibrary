import { Injectable } from '@angular/core';
import { ManagerOrderDetailsCommand } from '../../..';
import { CommandHandler, RedirectorService, redirectPathes } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ManagerOrderDetailsCommandHandlerService extends CommandHandler<ManagerOrderDetailsCommand> {

  constructor(
    private readonly redirector: RedirectorService,
  ) {
    super();
  }

  dispatch(command: ManagerOrderDetailsCommand): void {
    this.redirector.redirectTo(`${redirectPathes.manager_orders}/${command.order.id}`);
  }
}
