import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CartService, ClientAddOrderCommand, OrderService } from '../../..';
import { CommandHandler, getClientOrderHistoryPath, mapOrderBookToDeleteCartBookFromCartRequest, mapOrderToCreateOrderRequest, RedirectorService, SnackbarManager } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ClientAddOrderCommandHandlerService extends CommandHandler<ClientAddOrderCommand> implements OnDestroy {
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly orderService: OrderService,
    private readonly cartService: CartService,
    private readonly redirector: RedirectorService,
    private readonly snackManager: SnackbarManager,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.cleanUp();
  }

  cleanUp() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatch(command: ClientAddOrderCommand): void {
    this.orderService.createOrder(mapOrderToCreateOrderRequest(command.order))
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(
        (result) => {
          if (result.isSuccess) {
            this.redirector.redirectTo(getClientOrderHistoryPath());
            this.snackManager.openInfoSnackbar("✔️ The order was successfully created!", 5)
            const requests = command.order!.orderBooks.map(x => mapOrderBookToDeleteCartBookFromCartRequest(x));
            this.cartService.deleteBooksFromCart(requests);
          }
          else if (!result.isSuccess && result.error !== null) {
            this.cleanUp();
          }
        }
      );
  }
}
