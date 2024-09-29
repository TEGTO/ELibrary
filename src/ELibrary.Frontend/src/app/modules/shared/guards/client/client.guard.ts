
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { map, Observable } from 'rxjs';
import { CommandHandler } from '../..';
import { ADD_CLIENT_COMMAND_HANDLER, AddClientCommand, ClientService } from '../../../shop';

@Injectable({
  providedIn: 'root'
})
export class ClientGuard implements CanActivate {

  constructor(
    private readonly clientService: ClientService,
    @Inject(ADD_CLIENT_COMMAND_HANDLER) private readonly addClientHandler: CommandHandler<AddClientCommand>

  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.clientService.getClient().pipe(
      map((client) => {
        if (!client) {
          const command: AddClientCommand =
          {
            redirectAfter: state.url
          }
          this.addClientHandler.dispatch(command);
          return false;
        }
        return true;
      })
    );
  }
}