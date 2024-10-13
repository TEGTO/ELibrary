import { Injectable } from '@angular/core';
import { CartService, UpdateCartBookCommand } from '../../..';
import { CommandHandler, mapCartBookToUpdateCartBookRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateCartBookCommandHandlerService extends CommandHandler<UpdateCartBookCommand> {

  constructor(
    private readonly cartService: CartService,
  ) {
    super();
  }

  dispatch(command: UpdateCartBookCommand): void {
    this.cartService.updateCartBook(mapCartBookToUpdateCartBookRequest(command.cartBook));
  }

}