import { Injectable } from '@angular/core';
import { CartService, DeleteCartBookCommand } from '../../..';
import { CommandHandler, mapCartBookToDeleteCartBookFromCartRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeleteCartBookCommandHandlerService extends CommandHandler<DeleteCartBookCommand> {

  constructor(
    private readonly cartService: CartService,
  ) {
    super();
  }

  dispatch(command: DeleteCartBookCommand): void {
    const requests = [mapCartBookToDeleteCartBookFromCartRequest(command.cartBook)];
    this.cartService.deleteBooksFromCart(requests);
  }

}