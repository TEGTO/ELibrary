
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { map, Observable } from 'rxjs';
import { CommandHandler } from '../..';
import { ClientService, START_ADDING_CLIENT_COMMAND_HANDLER, StartAddingClientCommand } from '../../../shop';

@Injectable({
  providedIn: 'root'
})
export class ClientGuard implements CanActivate {

  constructor(
    private readonly clientService: ClientService,
    @Inject(START_ADDING_CLIENT_COMMAND_HANDLER) private readonly startAddingHandler: CommandHandler<StartAddingClientCommand>

  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.clientService.getClient().pipe(
      map((client) => {
        if (!client) {
          const command: StartAddingClientCommand =
          {
            redirectAfter: state.url
          }
          this.startAddingHandler.dispatch(command);
          return false;
        }
        return true;
      })
    );
  }
}