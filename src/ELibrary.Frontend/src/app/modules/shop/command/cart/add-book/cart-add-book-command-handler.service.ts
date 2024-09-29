import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { CartAddBookCommand, CartService } from '../../..';
import { AuthenticationDialogManager, AuthenticationService } from '../../../../authentication';
import { CommandHandler, mapBookToAddBookToCartRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CartAddBookCommandHandlerService extends CommandHandler<CartAddBookCommand> {

  constructor(
    private readonly authenticatioService: AuthenticationService,
    private readonly cartService: CartService,
    private readonly authenticationDialog: AuthenticationDialogManager,
  ) {
    super();
  }

  dispatch(command: CartAddBookCommand): void {
    this.authenticatioService.getUserAuth()
      .pipe(
        take(1)
      )
      .subscribe(data => {
        if (data.isAuthenticated) {
          this.cartService.addBookToCart(mapBookToAddBookToCartRequest(command.book));
        }
        else {
          this.authenticationDialog.openLoginMenu();
        }
      })
  }

}
