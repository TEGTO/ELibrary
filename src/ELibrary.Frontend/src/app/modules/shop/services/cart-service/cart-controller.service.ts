import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { addBookToCart, clearCart, deleteCartBook, getCart, getInCartAmount, selectCartAmount, selectCartBooks, updateCartBook } from '../..';
import { AddBookToCartRequest, CartBook, UpdateCartBookRequest } from '../../../shared';
import { CartService } from './cart-service';

@Injectable({
  providedIn: 'root'
})
export class CartControllerService implements CartService {

  constructor(
    private readonly store: Store,
  ) { }

  getCartBooks(): Observable<CartBook[]> {
    this.store.dispatch(getCart());
    return this.store.select(selectCartBooks);
  }
  getInCartAmount(): Observable<number> {
    this.store.dispatch(getInCartAmount());
    return this.store.select(selectCartAmount);
  }
  addBookToCart(req: AddBookToCartRequest): void {
    this.store.dispatch(addBookToCart({ req: req }));
  }
  updateCartBook(req: UpdateCartBookRequest): void {
    this.store.dispatch(updateCartBook({ req: req }));
  }
  deleteCartBook(id: string): void {
    this.store.dispatch(deleteCartBook({ id: id }));
  }
  clearCart(): void {
    this.store.dispatch(clearCart());
  }
}
