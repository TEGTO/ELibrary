import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { ManagerUpdateOrderCommand, OrderService, ShopDialogManager } from '../../..';
import { CommandHandler, mapOrderToManagerUpdateOrderRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ManagerUpdateOrderCommandHandlerService extends CommandHandler<ManagerUpdateOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
    private readonly dialogManager: ShopDialogManager
  ) {
    super();
  }

  dispatch(command: ManagerUpdateOrderCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.orderService.managerUpdateOrder(mapOrderToManagerUpdateOrderRequest(command.order));
      }
    });
  }

}
