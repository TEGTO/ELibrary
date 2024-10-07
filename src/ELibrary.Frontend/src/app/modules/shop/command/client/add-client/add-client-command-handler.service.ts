import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AddClientCommand, ClientService, ShopDialogManager } from '../../..';
import { CommandHandler, getDefaultClient, mapClientToCreateClientRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AddClientCommandHandlerService extends CommandHandler<AddClientCommand> {
  constructor(
    private readonly clientService: ClientService,
    private readonly shopDialog: ShopDialogManager,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: AddClientCommand): void {
    this.shopDialog.openClientChangeMenu(getDefaultClient()).afterClosed().pipe(
      take(1),
    ).subscribe(client => {
      if (client) {
        const req = mapClientToCreateClientRequest(client);
        this.clientService.createClient(req);
      }
    });
  }
}
