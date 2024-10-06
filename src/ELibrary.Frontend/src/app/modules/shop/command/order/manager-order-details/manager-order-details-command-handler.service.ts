import { Injectable } from '@angular/core';
import { ManagerOrderDetailsCommand } from '../../..';
import { CommandHandler, getManagerOrderDetailsPath, RedirectorService } from '../../../../shared';

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
    this.redirector.redirectTo(getManagerOrderDetailsPath(command.order.id));
  }
}
