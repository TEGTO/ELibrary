import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CartService, ClientAddOrderCommand, OrderService } from '../../..';
import { CommandHandler, mapOrderBookToDeleteCartBookFromCartRequest, mapOrderToCreateOrderRequest, RedirectorService, redirectPathes, SnackbarManager } from '../../../../shared';

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
    if (!command.order) {
      if (command.matDialogRef) {
        command.matDialogRef.close();
      }
      this.redirector.redirectTo(redirectPathes.client_makeOrder);
    }
    else {
      this.orderService.createOrder(mapOrderToCreateOrderRequest(command.order))
        .pipe(
          takeUntil(this.destroy$)
        )
        .subscribe(
          (result) => {
            if (result.isSuccess) {
              this.redirector.redirectTo(redirectPathes.client_order_history);
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

}
