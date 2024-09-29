import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CartAddBookCommand, CartService } from '../../..';
import { AuthenticationDialogManager, AuthenticationService } from '../../../../authentication';
import { CommandHandler, mapBookToAddBookToCartRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CartAddBookCommandHandlerService extends CommandHandler<CartAddBookCommand> implements OnDestroy {
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly authenticatioService: AuthenticationService,
    private readonly cartService: CartService,
    private readonly authenticationDialog: AuthenticationDialogManager,
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
  dispatch(command: CartAddBookCommand): void {
    this.authenticatioService.getAuthData()
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        if (data.isAuthenticated) {
          this.cartService.addBookToCart(mapBookToAddBookToCartRequest(command.book));
        }
        else {
          this.authenticationDialog.openLoginMenu();
        }
        this.cleanUp();
      })
  }

}
