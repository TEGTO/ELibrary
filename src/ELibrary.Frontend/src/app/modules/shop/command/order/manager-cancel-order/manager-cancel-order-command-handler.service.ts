import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { ManagerCancelOrderCommand, OrderService, ShopDialogManager } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ManagerCancelOrderCommandHandlerService extends CommandHandler<ManagerCancelOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
    private readonly dialogManager: ShopDialogManager
  ) {
    super();
  }

  dispatch(command: ManagerCancelOrderCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.orderService.managerCancelOrder(command.order.id);
      }
    });
  }
}