import { Injectable } from '@angular/core';
import { ManagerCancelOrderCommand, OrderService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ManagerCancelOrderCommandHandlerService extends CommandHandler<ManagerCancelOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
  ) {
    super();
  }

  dispatch(command: ManagerCancelOrderCommand): void {
    this.orderService.managerCancelOrder(command.order.id);
  }

}