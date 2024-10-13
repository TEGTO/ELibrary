import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { ClientCancelOrderCommand, OrderService, ShopDialogManager } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ClientCancelOrderCommandHandlerService extends CommandHandler<ClientCancelOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
    private readonly dialogManager: ShopDialogManager,
  ) {
    super();
  }

  dispatch(command: ClientCancelOrderCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1),
    ).subscribe(result => {
      if (result === true) {
        this.orderService.clientCancelOrder(command.order.id);
      }
    });
  }

}
