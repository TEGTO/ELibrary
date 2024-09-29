import { Injectable } from '@angular/core';
import { ClientUpdateOrderCommand, OrderService } from '../../..';
import { CommandHandler, mapOrderToClientUpdateOrderRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ClientUpdateOrderCommandHandlerService extends CommandHandler<ClientUpdateOrderCommand> {

  constructor(
    private readonly orderService: OrderService,
  ) {
    super();
  }

  dispatch(command: ClientUpdateOrderCommand): void {
    this.orderService.clientUpdateOrder(mapOrderToClientUpdateOrderRequest(command.order));
  }

}
