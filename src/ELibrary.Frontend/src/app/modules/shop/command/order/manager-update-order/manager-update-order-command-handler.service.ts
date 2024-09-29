import { Injectable } from '@angular/core';
import { ManagerUpdateOrderCommand, OrderService } from '../../..';
import { CommandHandler, mapOrderToManagerUpdateOrderRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ManagerUpdateOrderCommandHandlerService extends CommandHandler<ManagerUpdateOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
  ) {
    super();
  }

  dispatch(command: ManagerUpdateOrderCommand): void {
    this.orderService.managerUpdateOrder(mapOrderToManagerUpdateOrderRequest(command.order));
  }

}
