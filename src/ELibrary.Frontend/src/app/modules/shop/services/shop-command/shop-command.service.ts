/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { CartService, ClientService, ShopDialogManager } from '../..';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';
import { Book, CartBook, Client, getDefaultClient, mapBookToAddBookToCartRequest, mapCartBookToUpdateCartBookRequest, mapClientToCreateClientRequest, mapClientToUpdateClientRequest, Order, RedirectorService, redirectPathes, SnackbarManager } from '../../../shared';
import { ShopCommand, ShopCommandObject, ShopCommandType } from './shop-command';


@Injectable({
  providedIn: 'root'
})
export class ShopCommandService implements ShopCommand, OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly cartService: CartService,
    private readonly clientService: ClientService,
    private readonly authenticatioService: AuthenticationService,
    private readonly authenticationDialog: AuthenticationDialogManager,
    private readonly shopDialog: ShopDialogManager,
    private readonly redirector: RedirectorService,
    private readonly snackManager: SnackbarManager
  ) { }

  ngOnDestroy(): void {
    this.cleanUp();
  }

  cleanUp(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatchCommand(commandObject: ShopCommandObject, commandType: ShopCommandType, dispatchedFrom: any, ...params: any): void {
    switch (commandObject) {
      case ShopCommandObject.Cart:
        switch (commandType) {
          case ShopCommandType.Add:
            this.addBookToCart(dispatchedFrom, params[0], params);
            break;
          case ShopCommandType.Update:
            this.updateCartBook(dispatchedFrom, params[0], params);
            break;
          case ShopCommandType.Delete:
            this.deleteCartBook(dispatchedFrom, params[0], params);
            break;
          default:
            break;
        }
        break;
      case ShopCommandObject.Client:
        switch (commandType) {
          case ShopCommandType.Add:
            this.addClient(dispatchedFrom, params);
            break;
          case ShopCommandType.Update:
            this.updateClient(dispatchedFrom, params[0], params);
            break;
          case ShopCommandType.Delete:
            this.deleteClient(dispatchedFrom, params);
            break;
          default:
            break;
        }
        break;
      case ShopCommandObject.Order:
        switch (commandType) {
          case ShopCommandType.Add:
            this.addOrder(dispatchedFrom, params[0], params);
            break;
          case ShopCommandType.Update:
            // this.updateOrder(dispatchedFrom, params[0], params);
            break;
          case ShopCommandType.Delete:
            // this.deleteOrder(dispatchedFrom, params[0], params);
            break;
          default:
            break;
        }
        break;
      default:
        break;
    }
  }

  //#region Cart

  addBookToCart(dispatchedFrom: any, book: Book, params: any) {
    this.authenticatioService.getAuthData()
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        if (data.isAuthenticated) {
          this.cartService.addBookToCart(mapBookToAddBookToCartRequest(book));
        }
        else {
          this.authenticationDialog.openLoginMenu();
        }
        this.cleanUp();
      })
  }
  updateCartBook(dispatchedFrom: any, cartBook: CartBook, params: any) {
    this.cartService.updateCartBook(mapCartBookToUpdateCartBookRequest(cartBook));
  }
  deleteCartBook(dispatchedFrom: any, cartBook: CartBook, params: any) {
    this.cartService.deleteCartBook(cartBook.id);
  }

  //#endregion

  //#region Order

  addOrder(dispatchedFrom: any, order: Order | null, params: any) {
    if (!order) {
      this.closeDialogIfPresent(params);
      this.redirector.redirectTo(redirectPathes.client_makeOrder);
    }
  }

  //#endregion

  //#region  Client

  addClient(dispatchedFrom: any, params: any) {
    if (typeof (params[0]) === 'string') {
      this.redirector.redirectTo(redirectPathes.client_addInformation, { redirectTo: params[0] });
    }
    else {
      this.shopDialog.openClientChangeMenu(getDefaultClient())
        .afterClosed().pipe(
          takeUntil(this.destroy$)
        ).subscribe(client => {
          if (client) {
            const req = mapClientToCreateClientRequest(client);
            this.clientService.createClient(req);
          }
          this.cleanUp();
        });
    }
  }
  updateClient(dispatchedFrom: any, client: Client, params: any) {
    const req = mapClientToUpdateClientRequest(client);
    this.clientService.updateClient(req);
  }
  deleteClient(dispatchedFrom: any, params: any) {
    this.clientService.deleteClient();
  }

  //#endregion

  private closeDialogIfPresent(params: any[]): void {
    params.forEach(param => {
      if (param instanceof MatDialogRef) {
        param.close();
      }
    }
    );
  }
}
