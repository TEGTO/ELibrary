
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { map, Observable } from 'rxjs';
import { RedirectorService } from '../..';
import { ClientService, ShopCommand, ShopCommandObject, ShopCommandType } from '../../../shop';

@Injectable({
  providedIn: 'root'
})
export class ClientGuard implements CanActivate {

  constructor(
    private readonly clientService: ClientService,
    private readonly shopCommand: ShopCommand,
    private readonly redirector: RedirectorService,
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.clientService.getClient().pipe(
      map((client) => {
        if (!client) {
          this.shopCommand.dispatchCommand(ShopCommandObject.Client, ShopCommandType.Add, this, state.url)
          return false;
        }
        return true;
      })
    );
  }
}