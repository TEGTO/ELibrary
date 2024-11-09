import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AddBookToCartRequest, BaseApiService, Cart, CartBook, CartBookResponse, CartResponse, DeleteCartBookFromCartRequest, mapCartBookResponseToCartBook, mapCartResponseToCart, UpdateCartBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class CartApiService extends BaseApiService {

  getCart(): Observable<Cart> {
    return this.httpClient.get<CartResponse>(this.combinePathWithCartApiUrl(``)).pipe(
      map((response) => mapCartResponseToCart(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getInCartCart(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithCartApiUrl(`/amount`)).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  addBookToCart(request: AddBookToCartRequest): Observable<CartBook> {
    return this.httpClient.post<CartBookResponse>(this.combinePathWithCartApiUrl(`/cartbook`), request).pipe(
      map((response) => mapCartBookResponseToCartBook(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  updateCartBookInCart(request: UpdateCartBookRequest): Observable<CartBook> {
    return this.httpClient.put<CartBookResponse>(this.combinePathWithCartApiUrl(`/cartbook`), request).pipe(
      map((response) => mapCartBookResponseToCartBook(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  deleteBooksFromCart(requests: DeleteCartBookFromCartRequest[]): Observable<Cart> {
    return this.httpClient.put<CartResponse>(this.combinePathWithCartApiUrl(``), requests).pipe(
      map((response) => mapCartResponseToCart(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  private combinePathWithCartApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/cart" + subpath);
  }

}
