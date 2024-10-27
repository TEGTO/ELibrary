import { Inject, Injectable } from '@angular/core';
import { map, of, switchMap, take } from 'rxjs';
import { ADD_CLIENT_COMMAND_HANDLER, AddBookStockOrderCommand, AddClientCommand, BookstockOrderService, ClientService, ShopDialogManager } from '../../..';
import { CommandHandler, CreateStockBookOrderRequest, mapStockBookChangeToStockBookChangeRequest, StockBookChange, StockBookOrderType } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AddBookStockCommandHandlerService extends CommandHandler<AddBookStockOrderCommand> {

  constructor(
    private readonly clientService: ClientService,
    private readonly stockbookService: BookstockOrderService,
    private readonly dialogManager: ShopDialogManager,
    @Inject(ADD_CLIENT_COMMAND_HANDLER) private readonly addClientHandler: CommandHandler<AddClientCommand>
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: AddBookStockOrderCommand): void {
    this.clientService.getClient()
      .pipe(
        take(1),
        switchMap(client => {
          if (client) {
            const dialogRef = this.dialogManager.openReplenishmentMenu();
            return dialogRef.afterClosed().pipe(
              take(1),
              map(changes => ({ client, changes }))
            );
          } else {
            const command: AddClientCommand = { redirectAfter: undefined };
            this.addClientHandler.dispatch(command)
            return of(null);
          }
        })
      )
      .subscribe(result => {
        if (result?.changes) {
          const req: CreateStockBookOrderRequest = {
            type: StockBookOrderType.StockReplenishment,
            clientId: result.client.id,
            stockBookChanges: result.changes.map((x: StockBookChange) =>
              mapStockBookChangeToStockBookChangeRequest(x)
            ),
          };
          this.stockbookService.createStockOrder(req);
        }
      });
  }
}
