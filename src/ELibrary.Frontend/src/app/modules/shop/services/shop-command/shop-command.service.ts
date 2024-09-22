/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CartService } from '../..';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';
import { Book, CartBook, mapBookToAddBookToCartRequest, mapCartBookToUpdateCartBookRequest } from '../../../shared';
import { ShopCommand, ShopCommandObject, ShopCommandType } from './shop-command';

@Injectable({
  providedIn: 'root'
})
export class ShopCommandService implements ShopCommand, OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly cartService: CartService,
    private readonly authenticatioNService: AuthenticationService,
    private readonly authenticationDialog: AuthenticationDialogManager,
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
      default:
        break;
    }
  }

  //#region  Cart

  addBookToCart(dispatchedFrom: any, book: Book, params: any) {
    this.authenticatioNService.getAuthData()
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
}
